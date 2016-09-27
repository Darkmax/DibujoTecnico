using UnityEngine;
using System.Collections;

public class ResultWindow : MonoBehaviour {

    public GameObject resultWindow;
    public UILabel textLabel;
    public UIButton btn_exit;
    public UIButton btn_again;
    public UIButton btn_next;
    public UIGrid grid;
    public ExerciseManager exerciseManager;

    public void SetupWindow(bool result, bool more)
    {
        if(result)
        {
            //check if there is more exercise
            textLabel.GetComponent<UILocalize>().key = "result_content_right";

            if (!more)
                NGUITools.SetActive(btn_next.gameObject, false);
        }
        else
        {
            textLabel.GetComponent<UILocalize>().key = "result_content_wrong";
            NGUITools.SetActive(btn_next.gameObject, true);
        }

        grid.Reposition();
    }

    //public void ResultOption()
    //{
    //    HideWindow();
    //    if (_result)
    //        dragdropExerciseManager.LoadNext();
    //    else
    //        dragdropExerciseManager.ExitExercise();
    //}

	public void ShowWindow()
    {
        NGUITools.SetActive(resultWindow, true);
        grid.Reposition();
    }

    public void HideWindow()
    {
        NGUITools.SetActive(resultWindow, false);
    }
}
