using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow {

    static InfoWindow Instance;
    GameObject scrollView;
    GameObject contentArea;
    ScrollRect scrollRect;
    static bool enabled = true;

    public static void SetScrollView(GameObject view)
    {
        GetInstance().SetScrollViewImpl(view);
        if (!enabled)
        {
            view.SetActive(false);
        }
        GetInstance().scrollRect = view.GetComponent<ScrollRect>();
    }

    public static void SetData(string text)
    {
        InfoWindow iw = GetInstance();
        iw.AddGuiText(text);
    }

    public static void Reset()
    {
        InfoWindow iw = GetInstance();
        iw.Clear();
    }

    static InfoWindow GetInstance()
    {
        if (Instance == null)
        {
            Instance = new InfoWindow();
        }
        return Instance;
    }

    public static void SetActive(bool flag)
    {
        enabled = flag;
        if (GetInstance().scrollView != null)
        {
            GetInstance().scrollView.SetActive(flag);
        }
    }

    void SetScrollViewImpl(GameObject view)
    {
        scrollView = view;
        Image img = scrollView.GetComponent<Image>();
        //img.color = new Color(0, 0, 0, 0.1f);

        contentArea = scrollView.transform.GetChild(0).GetChild(0).gameObject;
        ContentSizeFitter fitter = contentArea.AddComponent<ContentSizeFitter>();
        VerticalLayoutGroup layout = contentArea.AddComponent<VerticalLayoutGroup>();

        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        layout.padding = new RectOffset(10, 10, 15, 15);
        layout.childAlignment = TextAnchor.UpperLeft;
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
    }

    void AddGuiText(string text)
    {
        int fontSize = 13;

        GameObject obj = new GameObject();
        obj.transform.SetParent(contentArea.transform);
        obj.name = "text";
        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.anchoredPosition = new Vector2(0, 0);
        rt.sizeDelta = new Vector2(400, fontSize + 2);
        rt.pivot = new Vector2(0.5f, 0.5f);

        obj.AddComponent<CanvasRenderer>();
        Text textComponent = obj.AddComponent<Text>();
        textComponent.text = text;

//        textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComponent.font = Resources.Load<Font>("Font/Play-Regular");

        textComponent.fontSize = fontSize;
        textComponent.alignment = TextAnchor.UpperLeft;
        textComponent.color = new Color(1f, 1f, 1f, 194f / 255f);

        scrollRect.verticalNormalizedPosition = 0;
    }

    void Clear()
    {
        foreach (Transform text in contentArea.transform)
        {
            GameObject.Destroy(text.gameObject);
        }
    }

}
