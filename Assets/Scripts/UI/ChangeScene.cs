using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene : MonoBehaviour {

    public GameObject loading;
    public string scene;

	void OnClick()
    {
        NGUITools.SetActive(loading, true);
        SceneManager.LoadSceneAsync(scene);
    }
}
