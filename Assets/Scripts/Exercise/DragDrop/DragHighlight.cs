using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragHighlight : MonoBehaviour {

    public GameObject Highlight;

    void Start()
    {
        NGUITools.SetActive(Highlight, false);
    }

    void OnDragOver(GameObject dragObject)
    {
        NGUITools.SetActive(Highlight, true);
    }

    void OnDragOut(GameObject dragObject)
    {
        NGUITools.SetActive(Highlight, false);
    }
}
