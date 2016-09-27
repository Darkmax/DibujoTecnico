using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

[ExecuteInEditMode]
public class ExerciseManager : MonoBehaviour {

    //Public variables
    public int idChapter;
    public int idSubject;
    public string title;
    public GameObject rootExercise;
    public UILabel lblTitle;
    public UILabel lblNumExercise;
    public GameObject instructions;
    public UIToggle toggleAR;
    public UIToggle toggleFavorite;

    public UIGrid gridExercise;
    public GameObject prefabNumExcercise;

    [SerializeField]
    public List<GameObject> exerciseList;
    [SerializeField]
    public ResultWindow resultWindow;
    
    //Private variables
    private DB db;
    private Control control;
    private List<GameObject> listNum;
    private int _num; //num of current exercise
    private VuforiaBehaviour vuforiaBehaviour;

    void Awake()
    {
        db = GameObject.FindGameObjectWithTag("Backend").GetComponent<DB>();
        control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
    }

    void Start()
    {
        vuforiaBehaviour = control.vuforiaBehaviour;
        //load menu exercise menu
    }

//    void Start()
//    {
//#if UNITY_EDITOR
//        if(exerciseList.Count > 0)
//            LoadExercise(0);    //Load First Exercise
//#endif
//    }

    void OnEnable()
    {
        lblTitle.text = title;
    }

    public void LoadExercise(int numExercise)
    {
        //Set last not completed exercise
        if (numExercise == -1)
        {
            List<int> listCompleted = db.getCompletedExercises(idSubject);
            //If the user has completed all exercises just get the first one
            if (listCompleted.Count == exerciseList.Count)
                _num = 0;
            else
            {
                for (int i = 0; i < exerciseList.Count; i++)
                {
                    if (!listCompleted.Contains(i))
                    {
                        _num = i;
                        break;
                    }                        
                }
            }
        }else
            _num = numExercise; //Set the specific num exercise

        RemoveExercise(); //Remove if there a exercise loaded before


        //Check if the exercise is valid
        if (exerciseList.Count > 0 && _num >= 0 && _num < exerciseList.Count)
        {
            GameObject exercise = NGUITools.AddChild(rootExercise, exerciseList[_num]);
            exercise.name = _num + "_exercise";

            lblNumExercise.text = (_num + 1).ToString() + "/" + exerciseList.Count;

            //Check if exercise is favorited
            if (db.isFavoritedExercise(idSubject, (_num + 1)))
                toggleFavorite.value = true;
            else
                toggleFavorite.value = false;
        }
        else
            Debug.LogWarning("No more exercises");
    }

    public void LoadNext()
    {
        _num++;
        LoadExercise(_num);
    }

    public void LoadPrevious()
    {
        _num--;
        LoadExercise(_num);
    }

    public void ExitExercise()
    {
        control.ShowDefault();
        NGUITools.SetActive(control.windowChapters, true);
        control.activeWindow = control.windowChapters;
        NGUITools.Destroy(gameObject);
        Resources.UnloadUnusedAssets();
    }

    public void NextExercise()
    {
        resultWindow.HideWindow();
        LoadNext();
    }

    public void AgainExercise()
    {
        resultWindow.HideWindow();
        LoadExercise(_num);
    }

    private void RemoveExercise()
    {
        if (rootExercise.transform.childCount > 0)
            NGUITools.Destroy(rootExercise.transform.GetChild(0).gameObject);        
        if (control.targetAR.transform.childCount > 0)
            NGUITools.DestroyChildren(control.targetAR.transform);

        Resources.UnloadUnusedAssets();
    }

    //Show result window depending of the result of the exercise
    public void ShowResult(bool result)
    {
        //If user complete the exercise correctly
        if(result)
            db.addCompletedExercise(idSubject, _num); //Save that user complete this exercise

        resultWindow.SetupWindow(result, (_num < exerciseList.Count-1));
        resultWindow.ShowWindow();
    }

    public void ShowAR()
    {
        if (vuforiaBehaviour.enabled) //Desactivate AR
        {
            NGUITools.SetActive(resultWindow.gameObject, true, false);
            NGUITools.SetActive(rootExercise, true);
            NGUITools.SetActive(instructions, true, false);
            NGUITools.SetActive(control.background, true);

            NGUITools.SetActive(control.vuforiaBehaviour.gameObject, false);
            NGUITools.SetActive(control.targetAR.transform.parent.gameObject, false);
            toggleAR.value = false;
        }
        else //Activate AR
        {
            NGUITools.SetActive(resultWindow.gameObject, false);
            NGUITools.SetActive(rootExercise, false);
            NGUITools.SetActive(instructions, false, false);
            NGUITools.SetActive(control.background, false);

            NGUITools.SetActive(control.vuforiaBehaviour.gameObject, true);
            NGUITools.SetActive(control.targetAR.transform.parent.gameObject, true);
        }
        vuforiaBehaviour.enabled = !vuforiaBehaviour.enabled;
    }

    public void FavoriteExercise()
    {
        if(db.isFavoritedExercise(idSubject, _num + 1) ^ UIToggle.current.value)
            db.toggleFavorite(idChapter, idSubject, (_num + 1), UIToggle.current.value);
    }

    public void loadExerciseMenu()
    {
        for(int i=0; i<exerciseList.Count; i++)
        {
            GameObject numExercise = NGUITools.AddChild(gridExercise.gameObject, prefabNumExcercise);
            numExercise.GetComponent<ChangeExercise>().setup(i + 1, this);
            listNum.Add(numExercise);
        }
        gridExercise.Reposition();
    }
}
