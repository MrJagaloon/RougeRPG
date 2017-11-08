using UnityEditor;
using UnityEngine;
using System;

namespace CellAuto
{
    public enum RuleType { VON_NEUMANN = 4, MOORE = 8 }

    [Serializable]
    public class CANeighborRules
    {
        public RuleType ruleType = RuleType.MOORE;
        public bool[] rules;

        public int RuleCount { get { return (int)ruleType + 1; } }

        public bool this[int key]
        {
            get { return rules[key]; }
            set { rules[key] = value; }
        }

        public CANeighborRules(bool[] rules)
        {
            if (rules.Length != RuleCount)
                throw new Exception("rules");
            this.rules = rules;
        }

        public CANeighborRules()
        {
            rules = new bool[RuleCount];
        }

        [CustomPropertyDrawer(typeof(CANeighborRules))]
        public class CARulesInspector : PropertyDrawer
        {
            bool expanded = true;

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                SerializedProperty rulesProp = property.FindPropertyRelative("rules");
                SerializedProperty ruleTypeProp = property.FindPropertyRelative("ruleType");
                int ruleCount = ruleTypeProp.intValue + 1;

                rulesProp.arraySize = ruleCount;

                EditorGUI.BeginProperty(position, label, property);

                position.height = 16;

                expanded = EditorGUI.Foldout(position, expanded, label, true);
                position.y += 16;

                if (expanded)
                {
                    EditorGUI.PropertyField(position, ruleTypeProp, new GUIContent("Type"));
                    position.y += 16;

                    for (int i = 0; i < ruleCount; ++i)
                    {
                        Rect labelPos = new Rect(position.x + (i + 1) * 16, position.y, 12, 16);
                        GUI.Label(labelPos, i.ToString());

                        Rect fieldPos = new Rect(position.x + i * 16, position.y + 16, 32, 16);
                        EditorGUI.PropertyField(fieldPos, rulesProp.GetArrayElementAtIndex(i), GUIContent.none);
                    }

                    position.y += 32;
                }

                EditorGUI.EndProperty();
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                if (expanded)
                    return 64;
                return 16;
            }
        }
    }

}
