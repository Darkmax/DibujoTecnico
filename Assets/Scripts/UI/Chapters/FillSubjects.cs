using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FillSubjects : MonoBehaviour {

    private DB db;
    private int idChapter;
    public UIScrollView scrollView;
    public UIGrid gridSubjects;
    public GameObject prefabSubject;

    void Awake()
    {
        db = GameObject.FindGameObjectWithTag("Backend").GetComponent<DB>();
    }

    public void fillSubjects(int idChapter)
    {
        this.idChapter = idChapter;
        List<Subject> listSubjects = db.getSubjectsFromChapter(idChapter);

        foreach(Subject s in listSubjects)
        {
            GameObject subject = NGUITools.AddChild(gridSubjects.gameObject, prefabSubject);
            subject.name = s.number + "_subject";
            RunSubject scriptSubject = subject.GetComponent<RunSubject>();
            scriptSubject.SetupSubject(idChapter, s.idSubject, s.name, s.sprite, s.color, s.file);
        }
        gridSubjects.Reposition();
        scrollView.ResetPosition();
    }
}
