using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetFavoriteSubject : MonoBehaviour {

    public GameObject prefabExercise;
    public UIGrid gridExercises;
    public Color myColor;
    private DB db;

    void Awake()
    {
        db = GameObject.FindGameObjectWithTag("Backend").GetComponent<DB>();
    }

    public void setExercise(int idSubject, Color color)
    {
        List<FavoriteExercise> listExercises = db.getFavoriteExercise(idSubject);
        myColor = color;
        foreach(FavoriteExercise fe in listExercises)
        {
            GameObject exercise = NGUITools.AddChild(gridExercises.gameObject, prefabExercise);
            exercise.name = fe.number + "_exercise";
            UIButton script_button = exercise.GetComponent<UIButton>();
            script_button.defaultColor = color;
            script_button.pressed = color - new Color(0.15f, 0.15f, 0.15f, 0f);
            script_button.hover = color - new Color(0.15f, 0.15f, 0.15f, 0f);
            script_button.UpdateColor(true);

            UILabel labelExercise = exercise.GetComponentInChildren<UILabel>();
            labelExercise.text = fe.number.ToString();
            labelExercise.color = color;
        }
        gridExercises.Reposition();
    }
}
