/*
 *   Canvas
 *     +- Panel
 *          +- GameObject (UILineRendererとGraphGeneratorをアタッチ)
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class GraphGenerator : MonoBehaviour {

    // TODO
    // Axisは別のGraphGenerator/UILineRendererペアを用意する方が良さそう
    // グリッドも別のペアを使うと良さそうだけど重いかも
    // 重そうなら軸とグリッドは画像で
    // タイトルも極小フォントを使った画像にするとフォントを埋め込まなくて良い

    GameObject background;
    public float lineWidth = 1f;
    public Color lineColor = new Color(1f, 1f, 1f, 0.8f);

    RectTransform rt;
    Rect rect;

    UILineRenderer lineRenderer;

    public float marginTop = 20f;
    public float marginLeft = 25f;
    public float marginRight = 10f;
    public float marginBottom = 10f;

    int grpMaxPoint = 90;
    float grpMin;
    float grpMax;
    float grpRange;
    float grpStep;
    List<Vector2> points = new List<Vector2>();
    Vector2[] grpAxes;

	// Use this for initialization
	void Start () {

	}

    // 必須
    // Start()よりも先に呼ばれてしまう可能性があるため初期化処理は全部ここでおこなう
    public void Init(int maxSecond, float min, float max)
    {
        background = transform.parent.gameObject;
        rt = background.GetComponent<RectTransform>();
        float x;
        float y;
        if (rt.anchorMin.x == 0)
        {
            x = rt.anchoredPosition.x + marginLeft;
            y = rt.anchoredPosition.y + marginBottom;
        }
        else
        {
            x = Screen.width - rt.localPosition.x - rt.anchoredPosition.x + marginLeft;
            y = rt.anchoredPosition.y + rt.localPosition.y - marginBottom;
        }

        rect = new Rect(
            x,
            y,
            rt.sizeDelta.x - (marginLeft + marginRight),
            rt.sizeDelta.y - (marginTop + marginBottom));
        lineRenderer = gameObject.GetComponent<UILineRenderer>();
        lineRenderer.color = lineColor;
        lineRenderer.LineThickness = lineWidth;
        lineRenderer.LineList = true;

        //Test();

        //grpMaxPoint = maxPoint;
        grpMin = min;
        grpMax = max;
        grpRange = max - min;
        grpStep = rect.width / (float)maxSecond;
    }

    // 省略可
    public void AddAxes(Vector2[] axes)
    {
        grpAxes = axes;
    }

    // 必須
    public void AddPoint(float time, float value)
    {
        value = Mathf.Clamp(value, grpMin, grpMax);

        points.Add(new Vector2(grpStep * time, (value -grpMin) * rect.height / grpRange));
        if (points.Count >= grpMaxPoint)
        {
            points.RemoveAt(0);
        }

        Vector2 offset = new Vector2(marginLeft, marginBottom);

        Vector2[] PointArr = new Vector2[grpAxes.Length / 2 + points.Count * 2];
        int n = 0;
        for (int i = 0; i < grpAxes.Length; i++)
        {
            PointArr[n++] = new Vector2(grpAxes[i].x * rect.width, grpAxes[i].y * rect.height) + offset;
        }

        float last_x = 0;
        for (int i = 1; i < points.Count; i++)
        {
            PointArr[n++] = new Vector2(last_x, points[i - 1].y) + offset;
            PointArr[n++] = new Vector2(last_x + points[i].x, points[i].y) + offset;
            last_x += points[i].x;
        }

        lineRenderer.Points = PointArr;
        lineRenderer.SetAllDirty();
    }

    // 一度消して再描画するとき用
    // 軸情報は消去しない
    public void Reset()
    {
        points = new List<Vector2>();
    }


    void Test()
    {
        Init(50, 0, 30000);
        AddAxes(new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(0, 0),
            new Vector2(1, 0)
        });

        StartCoroutine("TestSub");
    }

    IEnumerator TestSub()
    {
        float time = 0f;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            AddPoint(time, Random.Range(0f, 30000f));
            time += 5f;
        }
    }
}


