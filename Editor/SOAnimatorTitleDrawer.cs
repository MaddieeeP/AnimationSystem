using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SecondOrderAnimator))]
public class SOAnimatorTitleDrawer : PropertyDrawer
{
    private float _dynamicFrequency;
    private float _dynamicDampening;
    private float _dynamicResponse;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty k1Property = property.FindPropertyRelative("_k1");
        SerializedProperty k2Property = property.FindPropertyRelative("_k2");
        SerializedProperty k3Property = property.FindPropertyRelative("_k3");

        SecondOrderDynamics.InternalValuesToParameters(k1Property.floatValue, k2Property.floatValue, k3Property.floatValue, out _dynamicFrequency, out _dynamicDampening, out _dynamicResponse); //Reduce calls if possible. Account for other scripts changing the values whilst inspector is open

        Rect dynamicFrequencyRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect dynamicDampeningRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
        Rect dynamicResponseRect = new Rect(position.x, position.y + 2 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), position.width, EditorGUIUtility.singleLineHeight);

        _dynamicFrequency = EditorGUI.FloatField(dynamicFrequencyRect, new GUIContent("Dynamic Frequency"), _dynamicFrequency);
        _dynamicDampening = EditorGUI.FloatField(dynamicDampeningRect, new GUIContent("Dynamic Dampening"), _dynamicDampening);
        _dynamicResponse = EditorGUI.FloatField(dynamicResponseRect, new GUIContent("Dynamic Response"), _dynamicResponse);

        SecondOrderDynamics.SanitizeParameters(ref _dynamicFrequency, ref _dynamicDampening, ref _dynamicResponse);

        if (GUI.changed)
        {
            SecondOrderDynamics.ParametersToInternalValues(_dynamicFrequency, _dynamicDampening, _dynamicResponse, out float k1, out float k2, out float k3);
            k1Property.floatValue = k1;
            k2Property.floatValue = k2;
            k3Property.floatValue = k3;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 3 * EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
    }
}