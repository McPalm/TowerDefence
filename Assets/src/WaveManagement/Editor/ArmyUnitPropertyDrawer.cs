using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(WaveManagement.Army.Unit))]
public class ArmyUnitPropertyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var enemyProperty = property.FindPropertyRelative("enemy");
        var enemy = enemyProperty.objectReferenceValue as GameObject;

        label.text = enemy != null ? enemy.name : " ";

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        

        var qtyRect = new Rect(position.x, position.y, 30, position.height);
        var lvlRect = new Rect(position.x + 32, position.y, 30, position.height);
        var sequenceRect = new Rect(position.x + 65, position.y, 50, position.height);
        var prefabRect = new Rect(position.x + 120, position.y, position.width - 120, position.height);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUI.PropertyField(qtyRect, property.FindPropertyRelative("qty"), GUIContent.none);
        EditorGUI.PropertyField(lvlRect, property.FindPropertyRelative("level"), GUIContent.none);
        EditorGUI.PropertyField(sequenceRect, property.FindPropertyRelative("spawnRate"), GUIContent.none);
        EditorGUI.PropertyField(prefabRect, property.FindPropertyRelative("enemy"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}