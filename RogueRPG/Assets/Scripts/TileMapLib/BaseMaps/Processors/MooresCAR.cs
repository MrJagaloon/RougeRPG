using UnityEngine;
using UnityEditor;

namespace TileMapLib.BaseMaps
{
    [System.Serializable]
    public class MooresCAR : ICellularAutomataRule
    {
        public bool[] bornRules;
        public bool[] surviveRules;

        public MooresCAR(bool[] bornRules, bool[] surviveRules)
        {
            this.bornRules = bornRules;
            this.surviveRules = surviveRules;
        }

        /* Return the new state that the cell should have
         */
        public bool NextCellState(BaseMap<bool> map, int x, int y)
        {
            int adjFullCount = GetAdjacentTrueCount(map, x, y);

            bool cell = map.GetCellValue(x, y);

            // Check if full
            if (cell)
                // Cell is full, check if it should survive.
                cell = surviveRules[adjFullCount];
            else
                // Cell is empty, check if it should be born.
                cell = bornRules[adjFullCount];
            return cell;
        }

        int GetAdjacentTrueCount(BaseMap<bool> map, int x, int y)
        {
            int count = 0;
            for (int adjX = x - 1; adjX <= x + 1; ++adjX)
            {
                for (int adjY = y - 1; adjY <= y + 1; ++adjY)
                {
                    if (adjX == x && adjY == y)
                        continue;

                    // Cells outside the bounds are considered full
                    if (adjX < 0 || adjX >= map.cols || adjY < 0 || adjY >= map.rows)
                        ++count;

                    else if (map.GetCellValue(adjX, adjY) == true)
                        ++count;
                }
            }
            return count;
        }
    }

    [CustomPropertyDrawer(typeof(MooresCAR))]
    public class CARulesInspector : PropertyDrawer
    {
        const int RULE_COUNT = 9;
        const float LINE_SIZE = 16;
        const float INDENT_SIZE = 16;
        float propDrawerHeight;
        bool expanded = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int lineCount = 0;

            position.height = LINE_SIZE;

            SerializedProperty bornRulesProp = property.FindPropertyRelative("bornRules");
            SerializedProperty surviveRulesProp = property.FindPropertyRelative("surviveRules");

            bornRulesProp.arraySize = RULE_COUNT;
            surviveRulesProp.arraySize = RULE_COUNT;

            EditorGUI.BeginProperty(position, label, property);

            expanded = EditorGUI.Foldout(position, expanded, label, true);
            position.y += LINE_SIZE;
            ++lineCount;

            if (expanded)
            {
                position.x += INDENT_SIZE * 2;

                // Display born rules
                GUI.Label(position, "Born Rules");
                position.y += LINE_SIZE;
                for (int i = 0; i < RULE_COUNT; ++i)
                {
                    Rect labelPos = new Rect(position.x + (i + 1) * 16, position.y, 12, 16);
                    GUI.Label(labelPos, i.ToString());

                    Rect fieldPos = new Rect(position.x + i * 16, position.y + 16, 32, 16);
                    EditorGUI.PropertyField(fieldPos, bornRulesProp.GetArrayElementAtIndex(i), GUIContent.none);
                }
                position.y += LINE_SIZE * 2;
                lineCount += 3;

                // Display survive rules
                GUI.Label(position, "Survive Rules");
                position.y += LINE_SIZE;
                for (int i = 0; i < RULE_COUNT; ++i)
                {
                    Rect labelPos = new Rect(position.x + (i + 1) * 16, position.y, 12, 16);
                    GUI.Label(labelPos, i.ToString());

                    Rect fieldPos = new Rect(position.x + i * 16, position.y + 16, 32, 16);
                    EditorGUI.PropertyField(fieldPos, surviveRulesProp.GetArrayElementAtIndex(i), GUIContent.none);
                }
                position.y += LINE_SIZE * 2;
                lineCount += 3;
            }

            propDrawerHeight = LINE_SIZE * lineCount;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return propDrawerHeight;
        }
    }
}