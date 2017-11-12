using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct IntRange
{
    public int min;
    public int max;

    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public bool IsInRange(float f)
    {
        return min <= f && f <= max;
    }

    public float Difference()
    {
        return max - min;
    }

    public override string ToString()
    {
        return string.Format("[IntRange: ({0}, {1})]", min, max);
    }
}

[CustomPropertyDrawer(typeof(IntRange))]
public class IntRangeValueInpector : PropertyDrawer
{
    bool expanded;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty minProp = property.FindPropertyRelative("min");
        SerializedProperty maxProp = property.FindPropertyRelative("max");

        EditorGUI.BeginProperty(position, label, property);

        position.height = 16;

        expanded = EditorGUI.Foldout(position, expanded, label, true);
        position.y += 16;

        if (expanded)
        {
            position.x += 16;
            position.width -= 16;

            EditorGUI.PropertyField(position, minProp, new GUIContent("Min"));
            position.y += 16;

            EditorGUI.PropertyField(position, maxProp, new GUIContent("Max"));
            position.y += 16;
        }

        if (maxProp.intValue < minProp.intValue)
            maxProp.intValue = minProp.intValue;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (expanded)
            return 48;
        return 16;
    }
}