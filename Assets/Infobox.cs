using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infobox : MonoBehaviour {

    TextMesh text1;
    TextMesh text2;
    TextMesh text3;
    TextMesh text4;

    // Use this for initialization
    void Start () {
        text1 = transform.Find("Wrapper/Text1").gameObject.GetComponent<TextMesh>();
        text2 = transform.Find("Wrapper/Text2").gameObject.GetComponent<TextMesh>();
        text3 = transform.Find("Wrapper/Text3").gameObject.GetComponent<TextMesh>();
        //text4 = transform.Find("Wrapper/Text4").gameObject.GetComponent<TextMesh>();

        text1.text = "Latitude:";
        text2.text = "Longitude:";
        text3.text = "Altitude:";
        //text4.text = "RSSI:";
    }

    // Update is called once per frame
    void Update () {
        Vector3 cameraPos = Camera.main.transform.position;
        transform.LookAt(cameraPos);
        Vector3 rot = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, rot.y, 0);
	}

    public void SetData(string latitude, string longitude, string altitude, string rssi)
    {
        text1.text = "Latitude: " + latitude;
        text2.text = "Longitude: " + longitude;
        text3.text = "Altitude: " + altitude;
        //text4.text = "RSSI: " + rssi;
    }

    public void Reset()
    {
        SetData("", "", "", "");
    }
}
