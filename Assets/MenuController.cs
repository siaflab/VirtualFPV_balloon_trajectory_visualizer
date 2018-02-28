using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    Button btnFile;
    Button btnEdit;
    GameObject mnFile;
    GameObject mnEdit;
    Button btnExit;
    Button btnSimulation;
    Button btnRunDemo;
    Button btnSetting;

    bool showMode = false;

    public GameObject ConfigWindow;
    public DummySender dummySender;
    GameObject graph;

    Config cfg;

	// Use this for initialization
	void Start () {
        cfg = Config.GetInstance();

        btnFile = transform.Find("File").gameObject.GetComponent<Button>();
        btnEdit = transform.Find("Edit").gameObject.GetComponent<Button>();
        mnFile = transform.Find("FileMenu").gameObject;
        mnEdit = transform.Find("EditMenu").gameObject;
        btnExit = transform.Find("FileMenu/Exit").gameObject.GetComponent<Button>();
        btnSimulation = transform.Find("EditMenu/Simulation").gameObject.GetComponent<Button>();
        btnRunDemo = transform.Find("EditMenu/RunDemo").gameObject.GetComponent<Button>();
        btnSetting = transform.Find("EditMenu/Setting").gameObject.GetComponent<Button>();
        graph = GameObject.Find("/Canvas/GraphAlti");


        btnFile.onClick.AddListener(() => {
            showMode = !mnFile.activeSelf;
            mnFile.SetActive(!mnFile.activeSelf);
        });
        btnEdit.onClick.AddListener(() => {
            showMode = !mnEdit.activeSelf;
            mnEdit.SetActive(!mnEdit.activeSelf);
        });
        btnExit.onClick.AddListener(() => {
            CloseAll();
            Application.Quit();
        });
        btnSimulation.onClick.AddListener(() => {
            CloseAll();
            dummySender.SimulationMode();
        });

        btnRunDemo.onClick.AddListener(() => {
            CloseAll();
            cfg.SetDemoMode(!cfg.DemoMode);
            dummySender.DemoMode(cfg.DemoMode);
        });
        
        btnSetting.onClick.AddListener(() => {
            CloseAll();
            graph.SetActive(false);
            ConfigWindow.SetActive(true);
        });

        CloseAll();
    }

    void CloseAll()
    {
        showMode = false;
        mnFile.SetActive(false);
        mnEdit.SetActive(false);
    }

    public void ShowFileMenu()
    {
        if (!showMode)
        {
            return;
        }

        if (!mnFile.activeSelf)
        {
            mnFile.SetActive(true);
        }
        if (mnEdit.activeSelf)
        {
            mnEdit.SetActive(false);
        }
    }

    public void ShowWindowMenu()
    {
        if (!showMode)
        {
            return;
        }

        if (mnFile.activeSelf)
        {
            mnFile.SetActive(false);
        }
        if (!mnEdit.activeSelf)
        {
            mnEdit.SetActive(true);
        }
    }

}
