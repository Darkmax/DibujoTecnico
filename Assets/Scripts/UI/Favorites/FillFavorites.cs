using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FillFavorites : MonoBehaviour {

    public GameObject prefabChapter;
    public UIScrollView scrollView;

    private DB db;
    private int y = 0;

	// Use this for initialization
	void Start () 
    {
        db = GameObject.FindGameObjectWithTag("Backend").GetComponent<DB>();
        fillFavorites();
	}
	
    void fillFavorites()
    {
        List<FVChapter> favoriteChapters = db.getFavorites();// db.getFavoriteChapters();
        Debug.LogWarning(favoriteChapters.Count);

        foreach (FVChapter fc in favoriteChapters)
        {
            GameObject chapter = NGUITools.AddChild(scrollView.gameObject, prefabChapter);
            chapter.transform.localPosition = new Vector3(0, -y, 0);
            chapter.name = fc.chapter.number + "_chapter";
            chapter.GetComponent<FavoriteChapter>().setChapter(fc);
            //SetFavoriteChapter scriptChapter = chapter.GetComponent<SetFavoriteChapter>();
            //scriptChapter.setSubject(fc.idChapter, fc.number, fc.name, scrollView.transform);
            //y += 70 + 260 * fc.numSubjects + 35;
        }
        scrollView.ResetPosition();
    }
}
