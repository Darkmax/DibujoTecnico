using UnityEngine;
using System.Collections;

public class ShowAR : MonoBehaviour {

    private Control control;

    void Start()
    {
        control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
    }

	void OnClick()
    {
        control.windowExercises.GetComponentInChildren<ExerciseManager>().ShowAR();
    }
}
