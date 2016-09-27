using UnityEngine;
using System.Collections;

public class BackExercise : MonoBehaviour {

    public ExerciseManager manager;
    private Control control;

    void Start()
    {
        //manager = transform.parent.parent.GetComponent<DragDropExerciseManager>();
        control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
    }

    void OnClick()
    {
        //Delete exercise
        if (control.vuforiaBehaviour.enabled)
            manager.ShowAR();
        else
        {
            if (control.windowExercises.transform.childCount > 0)
                NGUITools.Destroy(control.windowExercises.transform.GetChild(0).gameObject);

            if (control.targetAR.transform.childCount > 0)
                NGUITools.Destroy(control.targetAR.transform.GetChild(0).gameObject);

            Resources.UnloadUnusedAssets();

            control.ShowDefault();

            //Hide window of exercises and active chapters
            NGUITools.SetActive(control.windowExercises, false);
            control.activeWindow = control.windowChapters;
            NGUITools.SetActive(control.windowChapters, true);
        }
    }

#if UNITY_ANDROID
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnClick();
    }
#endif
}
