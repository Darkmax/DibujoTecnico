using System.Collections;

public class FavoriteExercise {

	public FavoriteExercise(int idExercise, int number)
    {
        this.idExercise = idExercise;
        this.number = number;
    }

    public int idExercise { get; set; }
    public int number { get; set; }
}
