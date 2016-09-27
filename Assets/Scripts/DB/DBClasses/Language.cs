using UnityEngine;
using System.Collections;

public class Language {

	public Language(int idLanguage, string name, string code)
    {
        this.idLanguage = idLanguage;
        this.name = name;
        this.code = code;
    }

    public int idLanguage { get; set; }
    public string name { get; set; }
    public string code { get; set; }
}
