using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ExerciseManager))]
public class ExerciseManagerEditor : Editor {

    ExerciseManager myScript;
    private SerializedProperty m_list;
    private SerializedObject m_Object;

    void OnEnable()
    {
        myScript = (ExerciseManager)target;
        m_Object = new SerializedObject(target);
        m_list = m_Object.FindProperty("exerciseList");
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        myScript.title = EditorGUILayout.TextField("Title:", myScript.title);
        EditorGUILayout.PropertyField(m_list, new GUIContent("Exercises:"), true);
        m_Object.ApplyModifiedProperties();
        // Flag Unity to save the changes to to the prefab to disk
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
