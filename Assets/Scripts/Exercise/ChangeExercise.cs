using UnityEngine;
using System.Collections;

public class ChangeExercise : MonoBehaviour {

    private int num;
    public UILabel lblnum;
    private ExerciseManager exerciseManager;

    public void setup(int num, ExerciseManager exerciseManager)
    {
        this.num = num;
        this.exerciseManager = exerciseManager;
        lblnum.text = this.num.ToString();
    }

	void OnClick()
    {
        exerciseManager.LoadExercise(num-1);
    }
}
