using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragDropExercise : MonoBehaviour {
    
    public int options
    {
        get { return s_options; }
        set { s_options = value; ConfigureBase(); }
    }

    public int size
    {
        get { return s_size; }
        set { s_size = value; ChangeSize(); }
    }

    //Setup
    [SerializeField]
    private int s_options;
    [SerializeField]
    private int s_size = 65;
    public int s_numViews = 0;
    public GameObject s_prefabView;
    public GameObject raModel;
    public GameObject raModelFinish;
    public ExerciseManager exerciseManager;

    //Logic
    public BaseOption baseOption;
    public List<ViewOption> listView;

    public int sum; //Sum of all option set on views
    private int total_sum; //Total sum for all options
    
    private Control control;

    void Start()
    {
        exerciseManager = transform.parent.parent.GetComponent<ExerciseManager>();
        control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
        StartExercise();
        LoadAR(false);
    }
    
    public void ConfigureBase()
    {
        if (baseOption.gridOptions.Count != s_options && s_options >= 0)
        {
            int count = s_options - baseOption.gridOptions.Count;
            if (count != 0)//Check if options has changed
            {
                //If the user has set more options than before
                if (count > 0)
                {
                    //Adding grids to base
                    for (int i = 0; i < count; i++)
                        baseOption.s_AddGrid();

                    //Adding grid to view
                    foreach (ViewOption view in listView)
                    {
                        for (int i = 0; i < count; i++)
                            view.s_AddGrid(s_size);
                    }

                }
                else if (count < 0) //if the user has remove options than before
                {
                    for (int i = count; i < 0; i++)
                        baseOption.s_RemoveGrid();

                    //Removing grid from view
                    foreach (ViewOption view in listView)
                    {
                        for (int i = count; i < 0; i++)
                            view.s_RemoveGrid();
                    }
                }
            }
        }
        else if (s_options < 0)
            s_options = 0;
    }

    private int calculateBaseOptions(int numOption)
    {
        int result = 0;
        foreach (ViewOption view in listView)
        {
            if (view.useList[numOption])
                result++;
        }
        return result;
    }

    public void ChangeSize()
    {
        if(s_options > 0)
        {
            baseOption.changeSize(s_size); //Changing the size on the base
            foreach (ViewOption view in listView)
                view.changeSize(s_size); //Changing the size on each view
        }
    }

    public void AddView()
    {
        //Add a view
        s_numViews++;
        GameObject view = NGUITools.AddChild(gameObject.transform.GetChild(0).gameObject, s_prefabView);
        view.name = s_numViews + "_View";
        ViewOption viewScript = view.GetComponent<ViewOption>();
        listView.Add(view.GetComponent<ViewOption>());

        //Adding grids on the view
        for (int i = 0; i < baseOption.gridOptions.Count; i++)
            viewScript.s_AddGrid(s_size);
    }

    public void RemoveView()
    {
        //Remove a view
        if (s_numViews > 0)
        {
            s_numViews--;
            GameObject view = listView[s_numViews].gameObject;
            listView.RemoveAt(s_numViews);
            NGUITools.Destroy(view);
        }
    }

    public void ResetViews()
    {
        //Destroy all views
        for (int i = listView.Count - 1; i >= 0; i--)
        {
            GameObject view = listView[i].gameObject;
            listView.RemoveAt(i);
            NGUITools.Destroy(view);
        }
        s_numViews = 0;
        //for (int i = 0; i < baseOption.countList.Count; i++)
        //    baseOption.countList[i] = s_numViews;
    }

    private void StartExercise()
    {
        //Set the sums to zero
        sum = 0;
        total_sum = 0;

        //Add the drag options to the base
        for (int i = 0; i < baseOption.gridOptions.Count; i++)
        {
            //Calculate how many options will have each grid on the base
            baseOption.countList[i] = calculateBaseOptions(i);

            //Add to the total sum
            total_sum += baseOption.countList[i];

            //Creat the first option
            baseOption.AddOption(i, s_size);
        }
    }

    public void EndExercise()
    {
        //Check if all options are on the correct position
        bool value = true;
        foreach (ViewOption view in listView)
        {
            if (!view.checkView())
                value = false;
        }
        exerciseManager.ShowResult(value);

        //If the user completed the exercise correct
        if(value)
        {
            DeleteAR();
            LoadAR(true);
        }
    }

    public void checkOnPress()
    {
        GameObject parent = OptionEvent.current.transform.parent.gameObject;
        OptionEvent.current.GetComponent<UISprite>().color += new Color(0.15f, 0.15f, 0.15f, 0.0f);
        if (parent.GetComponent<UIGrid>())
            parent.GetComponent<UIGrid>().animateSmoothly = false;
    }

    public void checkRelease()
    {
        OptionEvent.current.GetComponent<UISprite>().color -= new Color(0.15f, 0.15f, 0.15f, 0.0f);
        GameObject obj = OptionEvent.current.gameObject;
        GameObject parent = obj.transform.parent.gameObject;
        OptionEvent optionScript = obj.GetComponent<OptionEvent>();
        int numberGrid = optionScript.numCircle;

        //If release on any other surface that is not view or base
        if (parent == gameObject)
        {
            bool deleteChild = baseOption.DeleteOption(numberGrid); //Delete child if has already one
            optionScript.parent = baseOption.gridOptions[numberGrid];
            obj.transform.SetParent(optionScript.parent.transform); //set parent to base
            if(optionScript.itComes == 1)
                sum--;
            optionScript.itComes = 0; //Set base
            baseOption.tweenSmooth(numberGrid, true); //activate smooth
            if(deleteChild)
                baseOption.countList[numberGrid]++; //add another to count list
        }
        else if(parent.transform.parent.name.Contains("View")) //Check if releases on a view
        {
            //Check if is free the option of the view
            if(parent.transform.childCount == 2)
            {
                //Check if it comes from base
                if (optionScript.itComes == 0)
                {
                    baseOption.AddOption(numberGrid, s_size); //Add another option
                    optionScript.itComes = 1;    //set parent now to view
                    optionScript.parent = parent;
                    sum++;
                }
                parent.GetComponent<UIGrid>().animateSmoothly = true;  //activate smooth

                //Check if has finished the exercise
                if (sum == total_sum)
                    EndExercise();
            }
            else //if is not free
            {
                obj.transform.SetParent(optionScript.parent.transform);
                optionScript.parent.GetComponent<UIGrid>().animateSmoothly = true;
                optionScript.parent.GetComponent<UIGrid>().Reposition();
            }
        }
    }

    void LoadAR(bool finished)
    {
        if (raModel)
        {
            if (finished)
                NGUITools.AddChild(control.targetAR, raModelFinish);
            else
                NGUITools.AddChild(control.targetAR, raModel);
                
        }
    }

    void DeleteAR()
    {
        if (control.targetAR.transform.childCount > 0)
            NGUITools.DestroyChildren(control.targetAR.transform);
    }

    void DeleteModel()
    {
        GameObject model = control.targetAR.transform.GetChild(0).gameObject;
        NGUITools.Destroy(model);
        Resources.UnloadUnusedAssets();
    }
}
