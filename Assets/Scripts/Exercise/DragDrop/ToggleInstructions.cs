using UnityEngine;
using System.Collections;

public class ToggleInstructions : MonoBehaviour {

    public TweenPosition tween;
    public GameObject toggleObject;
    public GameObject background;
    private bool visible = true;

    void Start()
    {
        Vector3 ajuste = tween.to - tween.from;
        tween.from = tween.transform.localPosition;
        tween.to = tween.from + ajuste;
    }

    public void OnClick()
    {
        if (visible)
            EventDelegate.Add(tween.onFinished, delegate() { NGUITools.SetActive(toggleObject, true); }, true);
        else
            NGUITools.SetActive(toggleObject, false);
        visible = !visible;
        NGUITools.SetActive(background, visible);
        tween.Toggle();
    }


}
