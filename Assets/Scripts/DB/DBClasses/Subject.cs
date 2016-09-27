using UnityEngine;
using System.Collections;

public class Subject {

    public Subject (int idSubject, int number, string name, Color color, string sprite, string file)
    {
        this.idSubject = idSubject;
        this.number = number;
        this.name = name;
        this.color = color;
        this.sprite = sprite;
        this.file = file;
    }

    public int idSubject { get; set; }
    public int number { get; set; }
    public string name { get; set; }
    public Color color { get; set; }
    public string sprite { get; set; }
    public string file { get; set; }
}
