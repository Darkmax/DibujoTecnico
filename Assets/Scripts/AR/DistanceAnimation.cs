using UnityEngine;
using System.Collections;

public class DistanceAnimation : MonoBehaviour {

    private Animation animationScript;
    private Control control;
    public float minDistance = 2500;
    public float maxDistance = 4000;

    void Start()
    {
        control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();

        animationScript = GetComponentInChildren<Animation>();
        animationScript.Play();
        animationScript[animationScript.clip.name].speed = 0;
        animationScript.enabled = true;
    }

	void Update()
    {
        float distance = Vector3.Distance(control.vuforiaBehaviour.transform.localPosition, control.targetAR.transform.parent.localPosition);
        float lerp = map(distance, minDistance, maxDistance, 0, 1);
        if (lerp < 1 && lerp > 0f && !animationScript.isPlaying) animationScript.Play();
        animationScript[animationScript.clip.name].time = lerp;
    }

    private float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        if (x < in_min) return out_min;
        else if (x > in_max) return out_max;
        else
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
