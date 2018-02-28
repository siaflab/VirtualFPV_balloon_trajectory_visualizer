using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonMover : MonoBehaviour {

    List<Vector3> positions = new List<Vector3>();
    public Material material;
    float width = 0.3f;

    GameObject trail;
    LineRenderer line;

    Vector3 lastPosition;
    Vector3 newPosition;

    Vector3 lastCameraPosition;
    Vector3 newCameraPosition;

    float lerpTime = 1f;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (lerpTime < 1f)
        {
            // バルーンとカメラをなめらかに移動
            lerpTime += Time.deltaTime * 0.4f;
            transform.position = Vector3.Lerp(lastPosition, newPosition, lerpTime);
            Camera.main.transform.position = Vector3.Lerp(lastCameraPosition, newCameraPosition, lerpTime);
            if (lerpTime > 0.4f)
            {
                // 軌跡移動
                positions.Add(newPosition);
                DrawLine();
            }
        }
    }

    public void SetData(float latitude, float longitude, float altitude, float power, string time)
    {
        if (trail == null)
        {
            Reset();
        }

        newPosition = GeoCalculator.ToXYZ(latitude, longitude, altitude);

        // カメラとの位置関係を記憶
        Vector3 offset = Camera.main.transform.position - transform.position;

        lastPosition = transform.position;
        lerpTime = 0f;

        // 位置関係を保持するようにカメラを移動する
        lastCameraPosition = Camera.main.transform.position;
        newCameraPosition = new Vector3();
        newCameraPosition.x = newPosition.x + offset.x;
        newCameraPosition.y = Mathf.Max(newPosition.y + offset.y, 1f);
        newCameraPosition.z = newPosition.z + offset.z;
    }

    public void Reset()
    {
        if (trail != null)
        {
            Destroy(trail);
        }

        positions = new List<Vector3>();

        trail = new GameObject();
        trail.name = "trail";
        trail.layer = 8;
        line = trail.AddComponent<LineRenderer>();
        line.material = material;
        line.startColor = new Color(0.3f, 0.3f, 0.3f);
        line.endColor = new Color(1f, 1f, 1f);
        line.startWidth = 0;
        line.endWidth = width;
        line.widthMultiplier = width;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.positionCount = 0;
        line.receiveShadows = false;
    }

    void DrawLine()
    {
        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());

    }

}
