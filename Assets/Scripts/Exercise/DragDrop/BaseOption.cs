using UnityEngine;
using System.Collections.Generic;

public class BaseOption : MonoBehaviour {

    [HideInInspector]
    public DragDropExercise dragDropExercise;
    [HideInInspector]
    public GameObject s_prefabGrid;
    [HideInInspector]
    public GameObject prefabOption;
    [HideInInspector]
    public List<GameObject> gridOptions;
    [HideInInspector]
    public List<int> countList;

    void Start()
    {
        //Hide all sprites in base grids
        foreach (GameObject gridOption in gridOptions)
        {
            gridOption.GetComponent<UISprite>().alpha = 1;
            gridOption.GetComponent<UISprite>().enabled = false;
        }
    }

    public void s_AddGrid()
    {
        GameObject grid = NGUITools.AddChild(gameObject, s_prefabGrid);
        int num = gridOptions.Count + 1;
        grid.name = num + "_grid";
        gridOptions.Add(grid);
        countList.Add(0);
    }

    public void s_RemoveGrid()
    {
        GameObject grid = gridOptions[gridOptions.Count - 1];
        gridOptions.RemoveAt(gridOptions.Count - 1);
        NGUITools.Destroy(grid);
        countList.RemoveAt(countList.Count - 1);
    }

    public void AddOption(int position, int size)
    {
        if(gridOptions[position].transform.childCount == 0 && countList[position] > 0)
        {
            GameObject option = NGUITools.AddChild(gridOptions[position], prefabOption);
            int number = position + 1;
            option.name = number + "_option";

            //Changing size
            option.GetComponent<UISprite>().width = size;
            option.GetComponent<UISprite>().height = size;
            option.GetComponentInChildren<UILabel>().fontSize = Mathf.RoundToInt(size * 0.75f);
            option.GetComponentInChildren<UILabel>().text = number.ToString();

            OptionEvent eventScript = option.GetComponent<OptionEvent>();
            eventScript.parent = gridOptions[position];
            eventScript.itComes = 0; //set base
            eventScript.numCircle = position;
            EventDelegate.Set(eventScript.onPress, dragDropExercise.checkOnPress);
            EventDelegate.Set(eventScript.onRelease, dragDropExercise.checkRelease);
            countList[position]--;
        }
    }

    public bool DeleteOption(int position)
    {
        if(gridOptions[position].transform.childCount > 0)
        {
            foreach (Transform child in gridOptions[position].transform)
                NGUITools.Destroy(child.gameObject);
            
            return true;
        }
        return false;
    }

    public void changeSize(int size)
    {
        foreach (GameObject grid in gridOptions)
        {
            grid.GetComponent<UISprite>().width = size;
            grid.GetComponent<UISprite>().height = size;
        }
    }

    public void tweenSmooth(int position, bool value)
    {
        gridOptions[position].GetComponent<UIGrid>().animateSmoothly = value;
        if(value)
            gridOptions[position].GetComponent<UIGrid>().Reposition();
    }
}
