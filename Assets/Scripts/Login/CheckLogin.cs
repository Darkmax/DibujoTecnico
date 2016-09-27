using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CheckLogin : MonoBehaviour {

    public UIInput inputUser;
    public UIInput inputPass;
    public UILabel lblStatus;
    public string nextScene;

    private DB db;

    void Start()
    {
        db = GameObject.FindGameObjectWithTag("Backend").GetComponent<DB>();
    }

    public void OnClick()
    {
        if (string.IsNullOrEmpty(inputUser.value) || string.IsNullOrEmpty(inputPass.value))
            lblStatus.text = "Error check user or password are entered correctly";
        else
        {
            if (db.checkLogin(inputUser.value, inputPass.value, true))
                SceneManager.LoadSceneAsync(nextScene);
            else
                lblStatus.text = "User or password incorrect";
        }
    }
}
