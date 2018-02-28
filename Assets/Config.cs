using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour {

	public bool ShowBalloonLabel = true;
	public bool ShowLandmarkLabel = true;
	public bool Use3DMap = true;
	public bool SatelliteMap = true;
	public bool AutoTrackingCamera = true;
	public bool SoundEffect = true;
    public bool DemoMode = false;

    public Material matIllust;
    public Material matPhoto;

    // 制御用
    public ConfigWindow configWindow;
    GameObject balloonLabel;
    GameObject map3D;
    GameObject map2D;

    static Config instance;

    // 起動時に読み込み、終了時に保存したい

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        balloonLabel = GameObject.Find("/Balloon/Infobox");
        map3D = GameObject.Find("/3Dmap");
        map2D = GameObject.Find("/2Dmap");

        // 他のオブジェクトの準備ができてから初期化処理をおこなう
        StartCoroutine(DelayedInit());
    }

    private IEnumerator DelayedInit()
    {
        yield return new WaitForSeconds(0.3f);
        Init();
    }

    private void Init()
    {
        SetShowBalloonLabel(ShowBalloonLabel);
        SetShowLandmarkLabel(ShowLandmarkLabel);
        SetUse3DMap(Use3DMap);
        SetSatelliteMap(SatelliteMap);
        SetAutoTrackingCamera(AutoTrackingCamera);
        SetSoundEffect(SoundEffect);
    }

    public static Config GetInstance()
    {
        return instance;
    }

    public void SetShowBalloonLabel(bool flag)
    {
        ShowBalloonLabel = flag;
        balloonLabel.SetActive(flag);
    }

    public void SetShowLandmarkLabel(bool flag)
    {
        ShowLandmarkLabel = flag;
        GameObject ground = GameObject.Find("/ground");
        if (flag)
        {
            ground.BroadcastMessage("ShowLabels");              
        }
        else
        {
            ground.BroadcastMessage("HideLabels");                              
        }
    }

    public void SetUse3DMap(bool flag)
    {
        Use3DMap = flag;
        map3D.SetActive(flag);
        map2D.SetActive(!flag);
    }

    public void SetSatelliteMap(bool flag)
    {
        SatelliteMap = flag;

        Renderer renderer2D = map2D.transform.GetChild(0).gameObject.GetComponent<Renderer>();
        Renderer renderer3D = map3D.transform.GetChild(0).gameObject.GetComponent<Renderer>();
        if (flag)
        {
            renderer2D.material = matPhoto;
            renderer3D.material = matPhoto;
        }
        else
        {
            renderer2D.material = matIllust;
            renderer3D.material = matIllust;
        }
    }

    public void SetAutoTrackingCamera(bool flag)
    {
        AutoTrackingCamera = flag;
    }

    public void SetSoundEffect(bool flag)
    {
        SoundEffect = flag;
    }

    public void SetDemoMode(bool flag)
    {
        DemoMode = flag;
    }

    void Update () {
        if (Input.GetKeyUp(KeyCode.B))
        {
            bool flag = !ShowBalloonLabel;
            SetShowBalloonLabel(flag);
            configWindow.SetShowBalloonLabel(flag);
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            bool flag = !ShowLandmarkLabel;
            SetShowLandmarkLabel(flag);
            configWindow.SetShowLandmarkLabel(flag);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            bool flag = !Use3DMap;
            SetUse3DMap(flag);
            configWindow.SetUse3DMap(flag);
        }       
        if (Input.GetKeyUp(KeyCode.M))
        {
            bool flag = !SatelliteMap;
            SetSatelliteMap(flag);
            configWindow.SetSatelliteMap(flag);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            bool flag = !SoundEffect;
            SetSoundEffect(flag);
            configWindow.SetSoundEffect(flag);
        }       
    }

}
