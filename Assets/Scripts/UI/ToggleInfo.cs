using UnityEngine;
using System.Collections;

public class ToggleInfo : MonoBehaviour {

    public TweenPosition tween;
    public GameObject cover;
    private bool toggle = false;

    void Start()
    {
        Vector3 ajuste = tween.to - tween.from;
        tween.from = tween.transform.localPosition;
        tween.to = tween.from + ajuste;
    }

    public void OnClick()
    {
        toggle = !toggle;
        tween.Toggle();
        NGUITools.SetActive(cover, toggle);
    }
}
