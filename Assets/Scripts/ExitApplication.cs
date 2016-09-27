using UnityEngine;
using System.Collections;

public class ExitApplication : MonoBehaviour {

	void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit(); 
#endif
    }
}
