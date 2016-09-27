using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour {

    public GameObject prefabInstruction;
    public UIGrid pivotInstruction;
    public GameObject prefabBullet;
    public UIGrid pivotBullet;
    public GameObject btnStart;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public int i = 0;
    public int num = -1;
    public GameObject activeBullet;

    void Start()
    {
        if (num == -1)
            NGUITools.SetActive(gameObject, false);
    }

    void finishDraggable()
    {
        i = pivotInstruction.GetIndex(pivotInstruction.GetComponent<UICenterOnChild>().centeredObject.transform);
        updateArrows();
        updateBullet();
        updateButton();
    }

    private void updateArrows()
    {
        if (i > 0)
            NGUITools.SetActive(leftArrow, true);
        else
            NGUITools.SetActive(leftArrow, false);

        if (i <= num - 1)
            NGUITools.SetActive(rightArrow, true);
        else
            NGUITools.SetActive(rightArrow, false);
    }

    private void updateBullet()
    {
        activeBullet.GetComponent<UISprite>().color = new Color(0.56f, 0.56f, 0.56f, 1f);
        activeBullet = pivotBullet.GetChild(i).gameObject;
        activeBullet.GetComponent<UISprite>().color = Color.white;
    }

    private void updateButton()
    {
        if (i == num )
            NGUITools.SetActive(btnStart, true);
        else
            NGUITools.SetActive(btnStart, false);
    }

	public void addTheory()
    {
        num++;
        GameObject instruction = NGUITools.AddChild(pivotInstruction.gameObject, prefabInstruction);
        instruction.name = num + "_instruction";
        EventDelegate.Add(instruction.GetComponent<UIEventTrigger>().onRelease, finishDraggable);
        pivotInstruction.Reposition();
        pivotInstruction.GetComponentInParent<UIScrollView>().ResetPosition();

        //Add bullet
        GameObject bullet = NGUITools.AddChild(pivotBullet.gameObject, prefabBullet);
        bullet.name = num + "_bullet";
        if (num == 0)
        {
            bullet.GetComponent<UISprite>().color = Color.white;
            activeBullet = bullet;
            NGUITools.SetActive(btnStart, true);
        }else if (num > 0)
        {
            NGUITools.SetActive(rightArrow, true);
            NGUITools.SetActive(btnStart, false);
        }

        pivotBullet.Reposition();
    }

    public void removeTheory()
    {
        NGUITools.Destroy(pivotInstruction.transform.GetChild(pivotInstruction.transform.childCount - 1).gameObject);
        NGUITools.Destroy(pivotBullet.transform.GetChild(pivotBullet.transform.childCount - 1).gameObject);
        pivotInstruction.Reposition();
        pivotInstruction.GetComponentInParent<UIScrollView>().ResetPosition();
        pivotBullet.Reposition();
        num--;
    }

    public void resetInstructions()
    {
        while (pivotInstruction.transform.childCount > 0)
            NGUITools.Destroy(pivotInstruction.transform.GetChild(0).gameObject);
        while (pivotBullet.transform.childCount > 0)
            NGUITools.Destroy(pivotBullet.transform.GetChild(0).gameObject);

        activeBullet = null;
        pivotInstruction.Reposition();
        pivotBullet.Reposition();
        pivotInstruction.GetComponentInParent<UIScrollView>().ResetPosition();

        NGUITools.SetActive(leftArrow, false);
        NGUITools.SetActive(rightArrow, false);
        NGUITools.SetActive(btnStart, false);
        num = -1;
    }
}
