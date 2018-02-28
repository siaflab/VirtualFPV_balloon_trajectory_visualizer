using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataWindow : MonoBehaviour {

    Text txCount;
    Text txTime;
    Text txLatitude;
    Text txLongitude;
    Text txAltitude;
    Text txClimingSpeed;
    Text txRSSI;

	void Start () {
        txCount = transform.Find("Labels/VL_COUNT").gameObject.GetComponent<Text>();
        txTime = transform.Find("Labels/VL_TIME").gameObject.GetComponent<Text>();
        txLatitude = transform.Find("Labels/VL_LATITUDE").gameObject.GetComponent<Text>();
        txLongitude = transform.Find("Labels/VL_LONGITUDE").gameObject.GetComponent<Text>();
        txAltitude = transform.Find("Labels/VL_ALTITUDE").gameObject.GetComponent<Text>();
        txClimingSpeed = transform.Find("Labels/VL_CLIMING_SPEED").gameObject.GetComponent<Text>();
        txRSSI = transform.Find("Labels/VL_RSSI").gameObject.GetComponent<Text>();

        Reset();		
	}

    public void Reset()
    {
        txCount.text = "";
        txTime.text = "";
        txLatitude.text = "";
        txLongitude.text = "";
        txAltitude.text = "";
        txClimingSpeed.text = "";
        //txRSSI.text = "";
    }

    public void SetData(string count, string time, string lat, string lon, string alt, string clim, string rssi)
    {
        txCount.text = count;
        txTime.text = time;
        txLatitude.text = lat;
        txLongitude.text = lon;
        txAltitude.text = alt;
        txClimingSpeed.text = clim;
        //txRSSI.text = rssi;
    }
}
