using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameraController : MonoBehaviour {

    Camera subCamera;
    static Rect rect;
    static SubCameraController Instance;

    public GameObject target;

    float pxMarginHorz = 20f;
    float pxMarginVert = 40f;
    float pxHorz = 415f;
    float rtVert = 0.4f;

    // Use this for initialization
    void Start () {
        Instance = this;

        subCamera = gameObject.GetComponent<Camera>();

        // 横20pixelは何パーセントか
        float marginHorz = pxMarginHorz / Screen.width;

        // 縦40pixelは何パーセントか
        float marginVert = pxMarginVert / Screen.height;

        // 縦のパーセンテージ
        float height = rtVert;
        // 縦40%は何ピクセルか
        float pxVert = Screen.height * height;
        // 縦40%のピクセル数は横何パーセントか
        //float width = px40vert / Screen.width;
        // 横215ピクセルは横何パーセントか
        float width = pxHorz / Screen.width;

        subCamera.rect = new Rect(1f - (marginHorz + width), 1f - (marginVert + height), width, height);

        float x = Screen.width - (pxMarginHorz + pxHorz);
        float y = Screen.height - (pxMarginVert + pxVert);
        rect = new Rect(x, y, pxHorz, pxVert);
    }
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(target.transform.position, Vector3.up);
	}

    public static bool Contains(Vector2 mousePosition)
    {
        if (rect.Contains(mousePosition))
        {
            return true;
        }

        return false;
    }

    public static void Zoom(float val)
    {
        Instance.ZoomImpl(val);
    }

    void ZoomImpl(float val)
    {
        float zoom = subCamera.orthographicSize + val * 2f;
        zoom = Mathf.Clamp(zoom, 1f, 19f);
        subCamera.orthographicSize = zoom;

    }
}
