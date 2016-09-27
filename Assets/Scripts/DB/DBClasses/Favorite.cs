using System.Collections;

public class Favorite {

	public Favorite(int numberChapter, string nameChapter, string typeSubject, int idExercise, int numberExercise)
    {
        this.numberChapter = numberChapter;
        this.nameChapter = nameChapter;
        this.typeSubject = typeSubject;
        this.idExercise = idExercise;
        this.numberExercise = numberExercise;
    }

    public int numberChapter { get; set; }
    public string nameChapter { get; set; }
    public string typeSubject { get; set; }
    public int idExercise { get; set; }
    public int numberExercise { get; set; }
}
