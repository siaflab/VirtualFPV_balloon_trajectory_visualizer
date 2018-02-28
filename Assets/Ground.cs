using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

    public Material material;
    public GameObject labelPrefab;

    float startLatitude = 42.85f;
    float endLatitude = 43.25f;
    float startLongitude = 141.3f;
    float endLongitude = 141.8f;
    float width = 0.2f;

    void Start()
    {
        Vector3 startPos;
        Vector3 endPos;

        for (float latitude = startLatitude; latitude <= endLatitude; latitude += 0.05f)
        {
            startPos = GeoCalculator.ToXYZ(latitude, startLongitude, 0f);
            endPos = GeoCalculator.ToXYZ(latitude, endLongitude, 0f);
            DrawLine(startPos, endPos);
            PutLabel(new Vector3(startPos.x - 1.0f, startPos.y, startPos.z + 0.2f), latitude);
            PutLabel(new Vector3(endPos.x + 0.1f, endPos.y, endPos.z + 0.2f), latitude);
        }
        for (float longitude = startLongitude; longitude <= endLongitude + 0.01; longitude += 0.05f)
        {
            startPos = GeoCalculator.ToXYZ(startLatitude, longitude, 0f);
            endPos = GeoCalculator.ToXYZ(endLatitude, longitude, 0f);
            DrawLine(startPos, endPos);
            PutLabel(new Vector3(startPos.x - 0.5f, startPos.y, startPos.z - 0.1f), longitude);
            PutLabel(new Vector3(endPos.x - 0.5f, endPos.y, endPos.z + 0.4f), longitude);
        }


    }

    void PutLabel(Vector3 pos, float value) {
        GameObject label = Instantiate(labelPrefab, pos, Quaternion.identity);
        label.layer = 8;
        label.transform.parent = transform;
        label.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = value.ToString("###.00");
    }

    void DrawLine(Vector3 startpoint, Vector3 endpoint)
    {
        GameObject obj = new GameObject();
        obj.name = "line";
        obj.layer = 8;
        obj.transform.parent = transform;
        LineRenderer line = obj.AddComponent<LineRenderer>();
        line.material = material;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.startColor = Color.blue;
        line.endColor = Color.blue;
        line.startWidth = width;
        line.endWidth = width;
        line.widthMultiplier = width;
        line.positionCount = 2;
        line.SetPositions(new Vector3[2]{startpoint, endpoint});

    }


}
