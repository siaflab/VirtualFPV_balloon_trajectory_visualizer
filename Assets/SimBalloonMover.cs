using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimBalloonMover : MonoBehaviour {

    List<Vector3> positions = new List<Vector3>();
    public Material material;
    float width = 0.3f;

    GameObject trail;
    LineRenderer line;

    Vector3 lastPosition;
    Vector3 newPosition;

    float lerpTime = 1f;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void SetData(float latitude, float longitude, float altitude)
    {
        if (trail == null)
        {
            Reset();
        }

        newPosition = GeoCalculator.ToXYZ(latitude, longitude, altitude);
        lastPosition = transform.position;
        positions.Add(newPosition);
        DrawLine();
    }

    public bool Drawed()
    {
        if (trail != null)
        {
            if (line.positionCount > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void Reset()
    {
        if (trail != null)
        {
            Destroy(trail);
        }

        positions = new List<Vector3>();

        trail = new GameObject();
        trail.name = "simtrail";
        trail.layer = 8;
        line = trail.AddComponent<LineRenderer>();
        line.material = material;
        line.startColor = new Color(0f, 1f, 1f);
        line.endColor = new Color(0f, 1f, 1f);
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.widthMultiplier = width;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.positionCount = 0;
    }

    void DrawLine()
    {
        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());

    }

}
