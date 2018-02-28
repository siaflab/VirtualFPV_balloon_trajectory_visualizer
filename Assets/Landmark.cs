using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Landmark : MonoBehaviour {

    TextMesh text1;
    GameObject child;

    // Use this for initialization
    void Start () {
        text1 = transform.Find("Wrapper/Text1").gameObject.GetComponent<TextMesh>();
        child = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update () {
        Vector3 cameraPos = Camera.main.transform.position;
        transform.LookAt(cameraPos);
        Vector3 rot = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, rot.y, 0);
	}

    public void ShowLabels()
    {
        child.SetActive(true);
    }

    public void HideLabels()
    {
        child.SetActive(false);
    }
}
