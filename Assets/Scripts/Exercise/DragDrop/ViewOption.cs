using UnityEngine;
using System.Collections.Generic;

public class ViewOption : MonoBehaviour {

    [HideInInspector]
    public GameObject s_gridPrefab;
    //[HideInInspector]
    public List<GameObject> gridList;
    public List<bool> useList;

    void Awake()
    {
        disableGrids(); //Disable the grids that doesn't need to be in this view
    }

    /// <summary>
    /// Method to add a new grid to the view list
    /// </summary>
    /// <param name="size"></param>
    public void s_AddGrid(int size)
    {
        GameObject grid = NGUITools.AddChild(gameObject, s_gridPrefab);
        grid.name = (gridList.Count + 1) + "_grid";
        changeSize(size);
        //grid.GetComponentInChildren<UIWidget>().width = Mathf.RoundToInt(size * 1.3f);
        //grid.GetComponentInChildren<UIWidget>().height = Mathf.RoundToInt(size * 1.3f);
        gridList.Add(grid);
        useList.Add(true);
    }

    /// <summary>
    /// Method to remove a grid from the view
    /// </summary>
    public void s_RemoveGrid()
    {
        GameObject grid = gridList[gridList.Count - 1];
        gridList.RemoveAt(gridList.Count - 1);
        NGUITools.Destroy(grid);
    }

    public void disableGrids()
    {
        for(int i=0; i<gridList.Count; i++)
            NGUITools.SetActive(gridList[i], useList[i]);   //activate only the grid that going to be used on this view
    }

    /// <summary>
    /// Method to change the size of sprite on the view options
    /// </summary>
    /// <param name="size"></param>
    public void changeSize(int size)
    {
        foreach (GameObject grid in gridList)
        {
            grid.GetComponentInChildren<UISprite>().width = size;
            grid.GetComponentInChildren<UISprite>().height = size;
        }
    }

    /// <summary>
    /// Method to check if all the grid are placed with correct number
    /// </summary>
    /// <returns></returns>
    public bool checkView()
    {
        bool value = true;
        //Iterate through all the grid list
        for(int i = 0; i<gridList.Count; i++)
        {
            if(useList[i])
            {
                int numCircle = gridList[i].GetComponentInChildren<OptionEvent>().numCircle;    //Get the number of the circle
                //Check if number of the circle is different and is suppose to be used
                if (i != numCircle)
                {
                    value = false;
                    break;
                }
            }
        }
        return value;
    }
}
