using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DiscardViewsExercise))]
public class DiscardViewsExerciseEditor : Editor {

    SerializedObject m_Object;
    SerializedProperty m_raObject;
    SerializedProperty m_raFinalized;

    void OnEnable()
    {
        m_Object = new SerializedObject(target);
        m_raObject = m_Object.FindProperty("raModel");
        m_raFinalized = m_Object.FindProperty("raModelFinish");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DiscardViewsExercise myScript = (DiscardViewsExercise)target;
        EditorGUILayout.LabelField("Views", myScript.s_numViews.ToString());

        if (GUILayout.Button("Add View"))
            myScript.AddView();

        if (GUILayout.Button("Remove View"))
            myScript.RemoveView();

        if (GUILayout.Button("Reset Views"))
            myScript.ResetViews();

        EditorGUILayout.PropertyField(m_raObject, new GUIContent("RA Model:"));
        EditorGUILayout.PropertyField(m_raFinalized, new GUIContent("RA Finalized:"));
        m_Object.ApplyModifiedProperties();
    }
}
