using UnityEngine;
using System.Collections;

public class SetupMain : MonoBehaviour {

    public UILabel menuName;
    public UILabel mainName;

    private DB db;

	void Awake()
    {
        db = GameObject.FindGameObjectWithTag("Backend").GetComponent<DB>();
    }

    void Start()
    {
        //SetupName
        menuName.text = db.user.name;
        mainName.text += db.user.name;

        //Setup main window on control
        GameObject.FindGameObjectWithTag("Control").GetComponent<Control>().activeWindow = gameObject;
    }
}
