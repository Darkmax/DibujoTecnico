using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ViewOption))]
public class ViewOptionEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ViewOption myScript = (ViewOption)target;
        //serializedObject.Update();

        //ViewOptionEditor.ShowArrayProperty(serializedObject.FindProperty("useList"));
        if (GUI.changed)
        {
            myScript.disableGrids();
            EditorUtility.SetDirty(target);
        }
    }

    public static void ShowArrayProperty(SerializedProperty list)
    {
        EditorGUILayout.PropertyField(list);

        EditorGUI.indentLevel += 1;
        for (int i = 0; i < list.arraySize; i++)
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent("Grid " + (i + 1).ToString()));
        EditorGUI.indentLevel -= 1;        
    }
}
