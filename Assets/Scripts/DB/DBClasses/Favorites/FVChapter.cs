using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FVChapter {

    public int idFavorite { get; set; }
    public Chapter chapter { get; set; }
    public List<FVSubject> subjects { get; set; }
    public List<int> exercises { get; set; }

    //public FVChapter (int idFavorite, Chapter chapter, List<Subject> subjects, List<int> exercises)
    //{
    //    this.idFavorite = idFavorite;
    //    this.chapter = chapter;
    //    this.subjects = subjects;
    //    this.exercises = exercises;
    //}
}
