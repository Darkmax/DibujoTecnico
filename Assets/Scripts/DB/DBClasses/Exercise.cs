using System.Collections;

public class Exercise {

    public Exercise (int idExercise, string file)
    {
        this.idExercise = idExercise;        
        this.file = file;
    }

    public int idExercise { get; set; }
    public string file { get; set; }
}
