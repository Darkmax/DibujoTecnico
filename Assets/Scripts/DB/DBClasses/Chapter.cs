using System.Collections;

public class Chapter {

    public Chapter(int idChapter, int number, string name)
    {
        this.idChapter = idChapter;
        this.number = number;
        this.name = name;
    }

    public int idChapter { get; set; }
    public int number { get; set; }
    public string name { get; set; }
}
