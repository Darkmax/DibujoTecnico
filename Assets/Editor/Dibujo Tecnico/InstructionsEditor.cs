using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Instructions))]
public class InstructionsEditor : Editor {

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        Instructions myScript = (Instructions)target;
        if (GUILayout.Button("Add Instruction"))
            myScript.addTheory();

        if (GUILayout.Button("Remove Instruction"))
            myScript.removeTheory();

        if (GUILayout.Button("Reset"))
            myScript.resetInstructions();
    }
}
