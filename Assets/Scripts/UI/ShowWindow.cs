using UnityEngine;
using System.Collections;

public class ShowWindow : MonoBehaviour {

    public GameObject activeWindow;
    public GameObject deactiveWindow;
    private Control control;
    public bool hideMenu;
    public ToggleInfo info;

    void Start()
    {
        control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
    }

    void OnClick()
    {
        deactiveWindow = control.activeWindow;
        NGUITools.SetActive(deactiveWindow, false);
        control.activeWindow = activeWindow;
        NGUITools.SetActive(activeWindow, true);

        if(activeWindow.GetComponent<FillChapters>())
            activeWindow.GetComponent<FillChapters>().InitChapters();

        if (hideMenu && info)
        {
            info.OnClick();
            info.GetComponent<UIPlaySound>().Play();
        }
    }
}
