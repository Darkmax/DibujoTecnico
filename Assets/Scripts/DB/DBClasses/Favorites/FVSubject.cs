using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FVSubject {

    public FVSubject(int idSubject, int snumber, string sname, Color color, string sprite, string file)
    {
        this.idSubject = idSubject;
        this.snumber = snumber;
        this.sprite = sprite;
        this.color = color;
        this.file = file;
    }

    public int idSubject { get; set; }
    public int snumber { get; set; }
    public string sprite { get; set; }
    public Color color { get; set; }
    public string file { get; set; }
    public List<int> exercises { get; set; }
}
