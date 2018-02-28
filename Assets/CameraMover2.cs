using UnityEngine;

/// <summary>
/// The camera added this script will follow the specified object.
/// The camera can be moved by left mouse drag and mouse wheel.
/// </summary>
[ExecuteInEditMode, DisallowMultipleComponent]
public class CameraMover2 : MonoBehaviour
{
    Config cfg;

    public GameObject target; // an object to follow
    public Vector3 offset; // offset form the target object

    [SerializeField]
    private float distance = 20.0f; // distance from following object
    [SerializeField]
    private float polarAngle = 67.0f; // angle with y-axis
    [SerializeField]
    private float azimuthalAngle = 26.0f; // angle with x-axis

    [SerializeField]
    private float minDistance = 1.0f;
    [SerializeField]
    private float maxDistance = 100.0f;
    [SerializeField]
    private float minPolarAngle = 5.0f;
    [SerializeField]
    private float maxPolarAngle = 130.0f;
    [SerializeField]
    private float mouseXSensitivity = 5.0f;
    [SerializeField]
    private float mouseYSensitivity = 5.0f;
    [SerializeField]
    private float scrollSensitivity = 5.0f;

    void Start()
    {
        cfg = Config.GetInstance();
    }

    private void Update()
    {
//        if ((cfg != null) && (cfg.DemoMode))
//        {
            updateAngle(Time.deltaTime * 0.6f, 0f);
//        }
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            updateAngle(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        updateDistance(Input.GetAxis("Mouse ScrollWheel"));

        var lookAtPos = target.transform.position + offset;
        updatePosition(lookAtPos);
        transform.LookAt(lookAtPos);
    }

    void updateAngle(float x, float y)
    {
        x = azimuthalAngle - x * mouseXSensitivity;
        azimuthalAngle = Mathf.Repeat(x, 360);

        y = polarAngle + y * mouseYSensitivity;
        polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
    }

    void updateDistance(float scroll)
    {
        if (SubCameraController.Contains(Input.mousePosition))
        {
            SubCameraController.Zoom(scroll);
        }
        else
        {
            scroll = distance - scroll * scrollSensitivity;
            distance = Mathf.Clamp(scroll, minDistance, maxDistance);
        }
    }

    void updatePosition(Vector3 lookAtPos)
    {
        var da = azimuthalAngle * Mathf.Deg2Rad;
        var dp = polarAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
            Mathf.Max(lookAtPos.y + distance * Mathf.Cos(dp), 1.0f),
            lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }
}
