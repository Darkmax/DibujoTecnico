using UnityEngine;
using System.Collections;
using Vuforia;

public class Control : MonoBehaviour {

    public Color defaultColor;
    public GameObject background;
    public GameObject topBar;
    public GameObject topMain;
    public GameObject windowChapters;
    public GameObject windowExercises;
    public VuforiaBehaviour vuforiaBehaviour;
    public GameObject targetAR;

    //[HideInInspector]
    public GameObject activeWindow;

    public void ShowDefault()
    {
        NGUITools.SetActive(topMain, true);
        setDefaultColor();
    }

    void setDefaultColor()
    {
        topBar.GetComponent<UISprite>().color = defaultColor;
    }

    public void ToggleTopBar(bool value)
    {
        NGUITools.SetActive(topBar, value);
    }
}
