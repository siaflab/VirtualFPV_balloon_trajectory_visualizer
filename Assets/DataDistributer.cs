using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDistributer : MonoBehaviour {

    public BalloonMover mover;
    public SimBalloonMover simMover;
    public Infobox balloonLabel;
    public GameObject logWindow;
    public DataWindow dataWindow;
    public GraphGenerator altitudeGraph;
    public GraphGenerator climingSpeedGraph;
    public ModuleImager moduleImager;
    public Signal signal;
    AudioSource se;

    int count;
    int currentTime;
    int lastTime;
    float currentAltitude;
    float lastAltitude;
    float climingSpeed;
    float lastClimingSpeed;

    // フレーム重複チェック用
    float checkAltitude = 0f;
    float checkLatitude = 0f;
    float checkLongitude = 0f;

    Config cfg;

    // Use this for initialization
    void Start () {
        cfg = Config.GetInstance();

        count = 0;
        currentTime = 0;
        lastTime = 0;
        currentAltitude = 0;
        lastAltitude = 0;
        climingSpeed = 0;
        lastClimingSpeed = 0;
        InfoWindow.SetScrollView(logWindow);

        altitudeGraph.marginLeft = 30;
        altitudeGraph.Init(45 * 60, 0f, 35000f);
        altitudeGraph.AddAxes(new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0) });

        climingSpeedGraph.Init(45 * 60, 0f, 15f);
        climingSpeedGraph.AddAxes(new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 0f), new Vector2(1, 0f) });

        se = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetData(string s)
    {
        string[] data = s.Split(',');
        char[] unit = { 'M' };
        float latitude = float.Parse(data[4]);
        float longitude = float.Parse(data[5]);
        float altitude = float.Parse(data[6].TrimEnd(unit));

        // 前回のGPS情報と比較して一致したらreturn
        if ((latitude == checkLatitude)
         && (longitude == checkLongitude)
         && (altitude == checkAltitude))
        {
//            return;            
        }
        checkLatitude = latitude;
        checkLongitude = longitude;
        checkAltitude = altitude;

        // 重複データではないので描画
        count++;
        lastTime = currentTime;
        lastAltitude = currentAltitude;
        currentAltitude = altitude;

        float rssi = float.Parse(data[2]);
        string strTime = data[3];
        string[] timeToken = strTime.Split(':');

        int iHour = int.Parse(timeToken[0]);
        int iMinute = int.Parse(timeToken[1]);
        int iSecond = int.Parse(timeToken[2]);


        currentTime = iHour * 3600
                    + iMinute * 60
                    + iSecond;
        if (timeToken[0] == "00")
        {
            if (currentTime < lastTime)
            {
                lastTime -= 3600 * 24;
            }
        }

        strTime = iHour.ToString("00") + ":"
                       + iMinute.ToString("00") + ":"
                       + iSecond.ToString("00");


        float deltaTime = (float)(currentTime - lastTime);
        lastClimingSpeed = climingSpeed;
        if ((deltaTime > 0f) && (deltaTime < 3600f))
        {
            if (currentAltitude == lastAltitude)
            {
                // センサの返す標高が前回と変わらないことがあるので
                // その場合は前回速度を使う
                climingSpeed = lastClimingSpeed;
            }
            else
            {
                climingSpeed = (currentAltitude - lastAltitude) / deltaTime;
            }
        }
        else
        {
            climingSpeed = 0f;
        }

        string sign = " ";
        if (rssi < 0)
        {
            sign = "-";
            rssi = Mathf.Abs(rssi);
        }
        string strCount = count.ToString("###");
        string strLatitude = latitude.ToString("###.00000");
        string strLongitude = longitude.ToString("###.00000");
        string strAltitude = currentAltitude.ToString("##,###") + " m";
        string strzAltitude = currentAltitude.ToString("00,000") + " m";
        string strRSSI = sign + rssi.ToString("###.00 dBm");
        string strzRSSI = sign + rssi.ToString("000.00 dBm");
        string strClimingSpeed = climingSpeed.ToString("##0.00 m/s");

        // バルーン位置更新
        mover.SetData(latitude, longitude, currentAltitude, rssi, strTime);

        // バルーンラベル更新
        balloonLabel.SetData(strLatitude, strLongitude, strAltitude, strRSSI);

        // ログウィンドウ更新
        InfoWindow.SetData("2017/07/01  " + strTime + "  " + strLatitude + "  " + strLongitude + "  " + strzAltitude);

        // データウィンドウ更新
        dataWindow.SetData(strCount, strTime, strLatitude, strLongitude, strAltitude, strClimingSpeed, strRSSI);

        // 標高グラフ更新
        altitudeGraph.AddPoint(deltaTime, currentAltitude);

        // 上昇速度グラフ更新
        climingSpeedGraph.AddPoint(deltaTime, climingSpeed);

        // モジュールイメージ更新
        moduleImager.SetData(-rssi);

        // 電波拡散表示
        signal.Ping();

        // サウンド更新
        if (cfg.SoundEffect)
        {
            if (!se.isPlaying)
            {
                se.PlayDelayed(0.3f);
            }
        }
    }

    public void SetSimulationData(string s)
    {
        string[] data = s.Split(',');
        float latitude = float.Parse(data[1]);
        float longitude = float.Parse(data[2]);
        float altitude = float.Parse(data[3]);
        simMover.SetData(latitude, longitude, altitude);
    }

    public void ClearSimulationData()
    {
        simMover.Reset();
    }

    public void Reset()
    {
        count = 0;
        currentTime = 0;
        lastTime = 0;
        currentAltitude = 0;
        lastAltitude = 0;
        climingSpeed = 0;
        lastClimingSpeed = 0;

        // バルーン位置クリア
        mover.Reset();

        // バルーンラベルクリア
        balloonLabel.SetData("", "", "0 m", " dBm");

        // ログウィンドウクリア
        InfoWindow.Reset();

        // データウィンドウクリア
        dataWindow.Reset();

        // 標高グラフクリア
        altitudeGraph.Reset();

        // 上昇速度グラフクリア
        climingSpeedGraph.Reset();

        // モジュールイメージクリア
        moduleImager.Reset();

        // シミュレーション描画クリア
        //simMover.Reset();
    }

}
