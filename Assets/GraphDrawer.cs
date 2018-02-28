using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDrawer : MonoBehaviour {

    public Material material;
    Texture2D lineTex;

    RectTransform rt;
    float areaWidth;
    float areaHeight;
    float margin = 10f;

    Vector2 gMin;
    Vector2 gMax;

    GameObject GraphLines;

	// Use this for initialization
	void Start () {
        rt = gameObject.GetComponent<RectTransform>();
        areaWidth = rt.sizeDelta.x;
        areaHeight = rt.sizeDelta.y;

        gMin = new Vector2(Screen.width -areaWidth + margin, Screen.height -areaHeight - margin);
        //gMin = new Vector2(600, 200);
                gMax = new Vector2(gMin.x + areaWidth - margin, gMin.y + areaHeight - margin);
        //gMax = new Vector2(gMin.x +100, gMin.y + 100);
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
        Debug.Log(gMin);
        Debug.Log(gMax);

        GraphLines = new GameObject();
        GraphLines.name = "line";
        GraphLines.transform.parent = transform;
        GraphLines.transform.localPosition = Vector3.zero;
    }
	
    void OnGUI()
    {
        DrawLines(new Vector2[4] {
            gMin,
            new Vector2(gMin.x, gMax.y),
            gMax,
            new Vector2(gMax.x, gMin.y)
        }, 2);
    }

	// Update is called once per frame
	void Update () {
		
	}

    void DrawLines(Vector2[] data, float width)
    {

        /*
                LineRenderer line = obj.AddComponent<LineRenderer>();
                line.material = material;
                line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                line.startColor = Color.white;
                line.endColor = Color.white;
                line.startWidth = width;
                line.endWidth = width;
                line.widthMultiplier = width;
                line.positionCount = data.Length;
                line.SetPositions(data);
        */

        for (int i = 0; i < data.Length - 1; i++)
        {
            DrawLine(gMin + data[i], gMin + data[i + 1], Color.white, 2);
        }
    }

    public void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
    {
        pointA /= 2f;
        pointB /= 2f;

        // Save the current GUI matrix, since we're going to make changes to it.
        Matrix4x4 matrix = GUI.matrix;

        // Generate a single pixel texture if it doesn't exist
        if (!lineTex) { lineTex = new Texture2D(1, 1); }

        // Store current GUI color, so we can switch it back later,
        // and set the GUI color to the color parameter
        Color savedColor = GUI.color;
        GUI.color = color;

        // Determine the angle of the line.
        float angle = Vector3.Angle(pointB - pointA, Vector2.right);

        // Vector3.Angle always returns a positive number.
        // If pointB is above pointA, then angle needs to be negative.
        if (pointA.y > pointB.y) { angle = -angle; }

        // Use ScaleAroundPivot to adjust the size of the line.
        // We could do this when we draw the texture, but by scaling it here we can use
        //  non-integer values for the width and length (such as sub 1 pixel widths).
        // Note that the pivot point is at +.5 from pointA.y, this is so that the width of the line
        //  is centered on the origin at pointA.
        GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));

        // Set the rotation for the line.
        //  The angle was calculated with pointA as the origin.
        GUIUtility.RotateAroundPivot(angle, pointA);

        // Finally, draw the actual line.
        // We're really only drawing a 1x1 texture from pointA.
        // The matrix operations done with ScaleAroundPivot and RotateAroundPivot will make this
        //  render with the proper width, length, and angle.
        GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1, 1), lineTex);

        // We're done.  Restore the GUI matrix and GUI color to whatever they were before.
        GUI.matrix = matrix;
        GUI.color = savedColor;
    }

}
