using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FavoriteChapter : MonoBehaviour {

    public UILabel titleChapter;
    public GameObject prefabSubject;
    public UIGrid subjectGrid;
    private DB db;

    void Awake()
    {
        db = GameObject.FindGameObjectWithTag("Backend").GetComponent<DB>();
    }

    public void setChapter(FVChapter fvChapter)
    {
        Transform anchorTarget = transform.parent;
        titleChapter.text = fvChapter.chapter.number + " - " + fvChapter.chapter.name;
        titleChapter.leftAnchor.target = anchorTarget;
        titleChapter.leftAnchor.absolute = 46;
        titleChapter.rightAnchor.target = anchorTarget;
        titleChapter.rightAnchor.absolute = -46;
        titleChapter.ResetAnchors();

        List<FVSubject> listSubjects = db.getFavoriteSubjects(fvChapter.chapter.idChapter);
        int i = 0;
        foreach (FVSubject fs in listSubjects)
        {
            GameObject subject = NGUITools.AddChild(subjectGrid.gameObject, prefabSubject);
            subject.name = fvChapter.chapter.number + "_" + i + "_subject";
            //SetFavoriteSubject script_subject = subject.GetComponent<SetFavoriteSubject>();
            //script_subject.setExercise(fs.idSubject, fs.color);
            UISprite sprite = subject.GetComponentInChildren<UISprite>();
            sprite.spriteName = fs.sprite;
            sprite.color = fs.color;

            //Set Anchors
            UIWidget widget = subject.GetComponent<UIWidget>();
            widget.leftAnchor.target = anchorTarget;
            widget.leftAnchor.absolute = 46;
            widget.rightAnchor.target = anchorTarget;
            widget.rightAnchor.absolute = -46;
            widget.ResetAnchors();
            i++;
        }
        subjectGrid.Reposition();
    }
}
