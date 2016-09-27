using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DiscardView : MonoBehaviour {

    [HideInInspector]
    public DiscardViewsExercise discardViewManager;
    [SerializeField]
    public bool correct = false;
    [HideInInspector, SerializeField]
    private bool _oldCorrect = false;
    private UISprite sprite;

    void Start()
    {
        sprite = GetComponentInChildren<UISprite>();
    }

    void OnClick()
    {
        if (discardViewManager.checkedList.Contains(this))
        {
            discardViewManager.checkedList.Remove(this); //remove the view from the ckecked list
            sprite.enabled = false;
        }
        else
        {
            discardViewManager.checkedList.Add(this); //add the view to the checked list
            sprite.color = Color.green;
            sprite.enabled = true;
        }

        //Check if already finish
        if (discardViewManager.checkedList.Count >= discardViewManager.listCorrectView.Count)
            discardViewManager.EndExercise();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (_oldCorrect != correct && Application.isEditor)
        {
            if (correct && !discardViewManager.listCorrectView.Contains(this))
            {
                sprite.color = Color.green;
                discardViewManager.listCorrectView.Add(this);
            }
            else
            {
                sprite.color = Color.red;
                discardViewManager.listCorrectView.Remove(this);
            }
            _oldCorrect = correct;
        }
#endif
    }
}
