using UnityEngine;
using System.Collections.Generic;

public class OptionEvent : MonoBehaviour
{
    static public OptionEvent current;

    [HideInInspector]
    public int numCircle;
    [HideInInspector]
    public int itComes; //0 = base, 1 = view
    [HideInInspector]
    public GameObject parent;
    public List<EventDelegate> onPress = new List<EventDelegate>();
    public List<EventDelegate> onRelease = new List<EventDelegate>();

    public bool isColliderEnabled
    {
        get
        {
            Collider c = GetComponent<Collider>();
            if (c != null) return c.enabled;
            Collider2D b = GetComponent<Collider2D>();
            return (b != null && b.enabled);
        }
    }

    void OnPress(bool pressed)
    {
        if (current != null || !isColliderEnabled) return;
        current = this;
        if (pressed) EventDelegate.Execute(onPress);
        else EventDelegate.Execute(onRelease);
        current = null;
    }
}
