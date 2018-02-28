using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConfigWindow : MonoBehaviour {

	Config cfg;

    Toggle tglShowBalloonLabel;
    Toggle tglSoundEffect;
    Toggle tglShowLandmarkLabel;
    Toggle tgl3DMap;
    Toggle tglSatelliteMap;

    bool muteEvent = false;

    public Button btnClose;
    GameObject graph;

	void Start () {
		cfg = Config.GetInstance();

        tglShowBalloonLabel = transform.Find("ItemGroup/ShowBalloonLabel").gameObject.GetComponent<Toggle>();
        tglSoundEffect = transform.Find("ItemGroup/SoundEffect").gameObject.GetComponent<Toggle>();
        tglShowLandmarkLabel = transform.Find("ItemGroup/ShowLandmarkLabel").gameObject.GetComponent<Toggle>();
        tgl3DMap = transform.Find("ItemGroup/3DMap").gameObject.GetComponent<Toggle>();
        tglSatelliteMap = transform.Find("ItemGroup/SatelliteMap").gameObject.GetComponent<Toggle>();
        graph = GameObject.Find("/Canvas/GraphAlti");

        btnClose.onClick.AddListener(()=>
		{
            graph.SetActive(true);
			gameObject.SetActive(false);
		});

		tglShowBalloonLabel.onValueChanged.AddListener((bool val)=>
		{
			if (muteEvent) return;
			cfg.SetShowBalloonLabel(val);
		});

        tglSoundEffect.onValueChanged.AddListener((bool val) =>
        {
			if (muteEvent) return;
            cfg.SetSoundEffect(val);
        });

        tglShowLandmarkLabel.onValueChanged.AddListener((bool val) =>
        {
			if (muteEvent) return;
            cfg.SetShowLandmarkLabel(val);
        });

        tgl3DMap.onValueChanged.AddListener((bool val) =>
        {
			if (muteEvent) return;
        	cfg.SetUse3DMap(val);
        });

        tglSatelliteMap.onValueChanged.AddListener((bool val) =>
        {
			if (muteEvent) return;
        	cfg.SetSatelliteMap(val);
        });

        gameObject.SetActive(false);
    }
	
    void OnEnable()
    {
    	// 起動時は何もしない
    	if (cfg == null)
    	{
    		return;
    	}

    	// ウィンドウ表示時
    	// 初期状態を設定するときもイベント処理が呼ばれてしまう
    	// そのためイベント処理本体が実行されないようにフラグで抑制する
		SetShowBalloonLabel(cfg.ShowBalloonLabel);
		SetShowLandmarkLabel(cfg.ShowLandmarkLabel);
		SetUse3DMap(cfg.Use3DMap);
		SetSoundEffect(cfg.SoundEffect);

    }

    public void SetShowBalloonLabel(bool flag)
    {
    	muteEvent = true;
    	tglShowBalloonLabel.isOn = flag;
    	muteEvent = false;
    }

    public void SetShowLandmarkLabel(bool flag)
    {
    	muteEvent = true;
    	tglShowLandmarkLabel.isOn = flag;
    	muteEvent = false;
    }

    public void SetUse3DMap(bool flag)
    {
    	muteEvent = true;
    	tgl3DMap.isOn = flag;
    	muteEvent = false;
    }

    public void SetSatelliteMap(bool flag)
    {
    	muteEvent = true;
    	tglSatelliteMap.isOn = flag;
    	muteEvent = false;
    }

    public void SetAutoTrackingCamera(bool flag)
    {
    }

    public void SetSoundEffect(bool flag)
    {
    	muteEvent = true;
    	tglSoundEffect.isOn = flag;
    	muteEvent = false;
    }


}
