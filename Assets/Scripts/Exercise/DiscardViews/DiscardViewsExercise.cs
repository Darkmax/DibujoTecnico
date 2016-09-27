using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscardViewsExercise : MonoBehaviour {

    //Setup
    public int s_numViews = 0;
    public GameObject rootViews;
    public GameObject s_prefabView;
    public GameObject raModel;
    public GameObject raModelFinish;
    public ExerciseManager exerciseManager;
    public List<DiscardView> listView;
    [SerializeField, HideInInspector]
    public List<DiscardView> listCorrectView;

    //Logic
    public List<DiscardView> checkedList;

    void Start()
    {
        exerciseManager = transform.parent.parent.GetComponent<ExerciseManager>();
        StartExercise();
    }

    public void AddView()
    {
        //Add a view
        s_numViews++;
        GameObject view = NGUITools.AddChild(rootViews, s_prefabView);
        view.name = s_numViews + "_View";
        view.GetComponent<DiscardView>().discardViewManager = this;
        listView.Add(view.GetComponent<DiscardView>());
    }

    public void RemoveView()
    {
        s_numViews--; //Remove View
        DiscardView view = listView[s_numViews];
        listView.Remove(view);
        NGUITools.Destroy(view.gameObject);
        Resources.UnloadUnusedAssets();
    }

    public void ResetViews()
    {
        s_numViews = 0;
        foreach (DiscardView view in listView)
            NGUITools.Destroy(view.gameObject);
        Resources.UnloadUnusedAssets();
        listView.Clear();
        listCorrectView.Clear();
    }

    private void StartExercise()
    {
        //Iterate through all views to make alpha 0
        foreach (DiscardView view in listView)
        {
            view.gameObject.GetComponentInChildren<UISprite>().color = Color.red;
            view.gameObject.GetComponentInChildren<UISprite>().enabled = false;
        }
    }

    public void EndExercise()
    {
        //Check if all options are on the correct position
        bool value = true;
        foreach (DiscardView view in checkedList)
        {
            if (!listCorrectView.Contains(view))
                value = false;
        }
        exerciseManager.ShowResult(value);
    }
}
