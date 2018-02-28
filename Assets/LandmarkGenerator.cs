using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkGenerator : MonoBehaviour {

    public GameObject smallLandmarkPrefab;
    public GameObject largeLandmarkPrefab;
    public GameObject stationPrefab;

    enum Type { Small, Large };

    // Use this for initialization
    void Start () {
        SetMoere("MOERENUMA PARK",  43.122119f, 141.425920f);
        SetLandmark("Sapporo Station", 43.0677f, 141.354f, -0.28f, -10f);
        //SetLandmark("Hokkaido Univ.", 43.076247f, 141.338914f, Type.Small);
        SetLandmark("Kita Hiroshima", 42.979210f, 141.563120f, -0.25f, 80);
        SetLandmark("Ebetsu Station", 43.110042f, 141.556608f, -0.28f, -30);
        SetLandmark("Teine Station", 43.120138f, 141.243744f, -0.23f, 25);
        SetLandmark("Eniwa Station", 42.883123f, 141.586257f, -0.23f, 45);
        SetLandmark("Iwamizawa St", 43.202f, 141.759340f, -0.25f, -40);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void SetMoere(string landmarkName, float latitude, float longitude)
    {
        Vector3 pos = GeoCalculator.ToXYZ(latitude, longitude, 0);
        GameObject landmark;
        landmark = Instantiate(largeLandmarkPrefab, pos, Quaternion.identity);
        landmark.layer = 8;
        landmark.transform.parent = transform;
        landmark.transform.Find("Wrapper/Text1").gameObject.GetComponent<TextMesh>().text = landmarkName;
    }

    void SetLandmark(string landmarkName, float latitude, float longitude, float altitude, float angle)
    {
        Vector3 pos = GeoCalculator.ToXYZ(latitude, longitude, 0);

        GameObject landmark;
        landmark = Instantiate(smallLandmarkPrefab, pos, Quaternion.identity);
        landmark.layer = 8;
        landmark.transform.parent = transform;
        landmark.transform.Find("Wrapper/Text1").gameObject.GetComponent<TextMesh>().text = landmarkName;

        Vector3 pos2 = new Vector3(pos.x, altitude, pos.z);
        GameObject station;
        station = Instantiate(stationPrefab, pos2, Quaternion.identity);
        station.transform.parent = transform;
        station.transform.eulerAngles = new Vector3(0, angle, 0);
    }

}
