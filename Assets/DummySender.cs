// #define GWANGJU

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DummySender : MonoBehaviour {

    public SimBalloonMover simMover;
    public Dropdown demotime;

    List<string> logdata;
    List<string> simdata;
    float timer;
    float simTimer;
    int count;
    int simCount;
    OSCHandler osc;

    float starttime;
    float currentTime;
    float lastTime;
    float wait;

    enum Mode { Stop, Idle, Start, Demo, Simulation };
    Mode mode;

    System.DateTime startDateTime;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        simTimer = 0;
        count = 0;
        simCount = 0;
        mode = Mode.Stop;

        demotime.value = 2;
    }

    public void RestartDemoMode()
    {
        if (mode == Mode.Demo)
        {
            osc.SendMessageToClient<string>("MySelf", "/reset", "");
            mode = Mode.Stop;
        }
        DemoMode(true);
    }

    public void DemoMode(bool flag)
    {

        if (mode == Mode.Demo)
        {
            if (!flag)
            {
                osc.SendMessageToClient<string>("MySelf", "/reset", "");
                mode = Mode.Stop;
            }
        }
        else
        {
            if (flag)
            {
                ReadLogfile();
                if ((logdata != null) && (logdata.Count > 0))
                {
                    count = 0;
                    timer = 0;
                    currentTime = -1;
                    mode = Mode.Start;
                    startDateTime = System.DateTime.Now;
                    starttime = GetSecond(logdata[0]);
                    SetWait2(logdata[0]);
                }
            }
        }
    }

    public void SimulationMode()
    {

        if (simMover.Drawed())
        {
            simMover.Reset();
        }
        else
        {
            simTimer = 0;
            simCount = 0;
            simMover.Reset();
            ReadSimlationCSV();
            if ((simdata != null) && (simdata.Count > 0))
            {
                mode = Mode.Simulation;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case Mode.Stop:
                return;

            case Mode.Start:
                SendReset();
                count = 0;
                timer = 99999;
                mode = Mode.Demo;
                break;

            case Mode.Idle:
                timer += Time.deltaTime;
                if (timer > 10.0f)
                {
                    SendReset();
                    count = 0;
                    timer = 99999;
                    mode = Mode.Demo;
                }
                break;

            case Mode.Demo:
                timer += Time.deltaTime;
                if (timer > wait)
                {
                    SendData(logdata[count]);
                    count++;
                    timer = 0;

                    if (count >= logdata.Count)
                    {
                        count = 0;
                        timer = 0;
                        mode = Mode.Stop;
                    }
                    else
                    {
#if (GWANGJU)
                        SetWait2(logdata[count]);
#else
                        SetWait(count);
#endif
                    }
                }
                break;

            case Mode.Simulation:
                simTimer += Time.deltaTime;
                if (simTimer > 0.01f)
                {
                    Sendsimdata(simdata[simCount]);
                    simCount++;
                    simTimer = 0;

                    if (simCount >= simdata.Count)
                    {
                        simCount = 0;
                        simTimer = 0;
                        mode = Mode.Stop;
                    }
                }
                break;
        }
    }

    void SendData(string s)
    {
        if (osc == null)
        {
            osc = OSCHandler.Instance;
        }
        osc.SendMessageToClient<string>("MySelf", "/data", s);
    }

    void Sendsimdata(string s)
    {
        string[] data = s.Split(',');
        float latitude = float.Parse(data[1]);
        float longitude = float.Parse(data[2]);
        float altitude = float.Parse(data[3]);
        simMover.SetData(latitude, longitude, altitude);
    }

    void SendReset()
    {
        if (osc == null)
        {
            osc = OSCHandler.Instance;
        }
        osc.SendMessageToClient<string>("MySelf", "/reset", "");
    }


    void ReadLogfile()
    {
        try
        {
            string projectFolder = System.IO.Directory.GetCurrentDirectory();
            logdata = new List<string>();
            
            using (var sr = new System.IO.StreamReader("logdata/gps_data.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    logdata.Add(line);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }


    void ReadSimlationCSV()
    {
        try
        {
            string projectFolder = System.IO.Directory.GetCurrentDirectory();
            simdata = new List<string>();
            
            using (var sr = new System.IO.StreamReader("simdata/flight_path.csv"))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    simdata.Add(line);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

    }

    void SetWait2(string s)
    {
        float targetSec = GetSecond(s) - starttime;
        Debug.Log("target");
        Debug.Log(targetSec);

        // 開始時点を０としたとき現在時刻（秒）
        System.TimeSpan span = System.DateTime.Now - startDateTime;
        // 目標時刻
        float currentSec = (float)span.TotalSeconds;
        Debug.Log("current");
        Debug.Log(currentSec);
        // 現在時刻から目標時刻までの差分
        wait = Mathf.Max(targetSec - currentSec, 0f);

        // デバッグ用
        //wait = 1;
    }

    void SetWait(int n)
    {
        // lasttimeを設定するため固定秒数でもGetDiffTime()を呼んでおく
        float difftime = GetDiffTime(n);
        switch (demotime.value)
        {
            case 0:
                wait = 1f;
                break;
            case 1:
                wait = 3f;
                break;
            case 2:
                wait = 5f;
                break;
            case 3:
                wait = 10f;
                break;
            case 4:
                wait = difftime / 30f;
                break;
            case 5:
                wait = difftime / 10f;
                break;
            case 6:
                wait = difftime / 5f;
                break;
            case 7:
                wait = difftime;
                break;
        }
    }

    float GetSecond(string s)
    {
        string[] data = s.Split(',');
        string[] timeToken = data[3].Split(':');
        float ret = float.Parse(timeToken[0]) * 3600f
                    + float.Parse(timeToken[1]) * 60f
                    + float.Parse(timeToken[2]);
        return ret;
    }

    float GetDiffTime(int n)
    {
        string[] data = logdata[n].Split(',');
        string[] timeToken = data[3].Split(':');
        float time = float.Parse(timeToken[0]) * 3600
                    + float.Parse(timeToken[1]) * 60
                    + float.Parse(timeToken[2]);

        if (n == 0)
        {
            lastTime = time - 1f;
        } else {
            lastTime = currentTime;
        }
        currentTime = time;

        if (timeToken[0] == "00")
        {
            if (currentTime < lastTime)
            {
                lastTime -= 3600 * 24;
            }
        }

        Debug.Log(logdata[n]);
        Debug.Log(currentTime.ToString() + " " + lastTime.ToString());
        return currentTime - lastTime;
    }

}
