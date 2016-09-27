using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FillChapters : MonoBehaviour {

    public UIScrollView scrollView;
    public UIGrid gridChapters;
    public GameObject prefabChapter;

    private bool chaptersComplete = false;
    private DB db;

    void Awake()
    {
        db = GameObject.FindGameObjectWithTag("Backend").GetComponent<DB>();
    }

    public void InitChapters()
    {
        if (chaptersComplete)
            scrollView.ResetPosition();
        else
            fillChapters();
    }

    private void fillChapters()
    {
        List<Chapter> listChapters = db.getChapters();

        foreach(Chapter c in listChapters)
        {
            GameObject chapter = NGUITools.AddChild(gridChapters.gameObject, prefabChapter);
            chapter.name = c.number + "_chapter";
            chapter.GetComponentInChildren<UILabel>().text = c.number + " - " + c.name;
            FillSubjects script_chapter = chapter.GetComponent<FillSubjects>();
            script_chapter.fillSubjects(c.idChapter);
        }
        gridChapters.Reposition();
        scrollView.ResetPosition();
        chaptersComplete = true;
    }
}
