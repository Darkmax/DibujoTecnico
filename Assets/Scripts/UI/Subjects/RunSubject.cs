using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunSubject : MonoBehaviour {

    private DB db;
    private Control control;
    private int idChapter;
    private int idSubject;
    public UISprite spriteIcon;
    public UILabel labelSubject;
    private TweenColor tweenColor;
    private string file_name;

    void Start()
    {
        db = GameObject.FindGameObjectWithTag("Backend").GetComponent<DB>();
        control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
    }

    void OnClick()
    {
        LoadExercises();
    }

    void OnHover(bool isOver)
    {
        tweenColor.Toggle();
    }

	public void SetupSubject(int idChapter, int idSubject, string text, string sprite, Color color, string file_name)
    {
        this.idChapter = idChapter;
        this.idSubject = idSubject;

        this.file_name = file_name;

        //Change sprite and color
        spriteIcon.spriteName = sprite;
        spriteIcon.color = color;

        //Change color of button
        UIButton buttonScript = GetComponent<UIButton>();
        tweenColor = GetComponentInChildren<TweenColor>();

        buttonScript.defaultColor = color;
        tweenColor.from = color;
        buttonScript.pressed = color - new Color(0.15f, 0.15f, 0.15f, 0);
        buttonScript.hover = color - new Color(0.15f, 0.15f, 0.15f, 0);
        tweenColor.to = color - new Color(0.15f, 0.15f, 0.15f, 0);
        buttonScript.UpdateColor(true);

        //Change label text and color
        labelSubject.text = text;
        labelSubject.color = color;
    }

    void LoadExercises()
    {
        NGUITools.SetActive(control.windowChapters, false); //Hide Chapters Window
        NGUITools.SetActive(control.topMain, false);    //Hide Top bar info
        control.topBar.GetComponent<UISprite>().color = spriteIcon.color;   //change color of top bar
        string folder = db.getFolderSubject(idSubject);
        GameObject mainExercise = NGUITools.AddChild(control.windowExercises, Resources.Load<GameObject>("Exercises/" + folder + "/" + file_name));
        mainExercise.name = file_name;

        NGUITools.SetActive(control.windowExercises, true); //Show Exercise Window
        control.activeWindow = control.windowExercises; //Set exercise to be active window
        ExerciseManager script = mainExercise.GetComponent<ExerciseManager>();
        script.idChapter = idChapter;
        script.idSubject = idSubject;
        script.LoadExercise(-1); //Load first exercise that hasn't completed
    }
}