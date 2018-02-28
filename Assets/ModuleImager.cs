using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleImager : MonoBehaviour {

    // Color alpha = new Color(0, 0, 0, 0.01f);
    // renderer.material.color -= alpha;
    // RSSI: -66 〜 -117 (-40 〜 -130  -90で1.0 -150で0.0)

    public Sprite spBalloon;
    public float fadeSpeed = 0.002f;
    Image moduleImage;

    float maxAlpha;
    float minAlpha = 0.1f;
    float currentAlpha;
    float time;
    Material material;

    // Use this for initialization
    void Start () {
        moduleImage = gameObject.GetComponent<Image>();
        material = GetComponent<CanvasRenderer>().GetMaterial();
        Reset();
    }

    // Update is called once per frame
    void Update () {
        if (currentAlpha > minAlpha)
        {
            time += Time.deltaTime;
            if (time >= 0.1f)
            {
                SetColor(currentAlpha - fadeSpeed);
            }
        }
	}

    public void Reset()
    {
        maxAlpha = 1f;
        SetColor(maxAlpha);
//        moduleImage.sprite = spBalloon;
        time = 0f;
    }

    public void SetData(float rssi)
    {
        StartCoroutine(DelayedSetData(0.3f, rssi));
    }

    IEnumerator DelayedSetData(float second, float rssi)
    {
        yield return new WaitForSeconds(second);
        maxAlpha = NormalizeAlpha(rssi);
        SetColor(maxAlpha);
        Debug.Log("maxAlphat = " + maxAlpha.ToString());
        time = 0f;
    }

    float NormalizeAlpha(float rssi)
    {
        float max = -90;
        float min = -120;
        float zero = -130;

        float val;
        val = Mathf.Clamp(rssi, min, max);
        Debug.Log("rssi = " + rssi.ToString());
        Debug.Log("na = " + ((val - zero) / (max - zero)).ToString());
        return (val - zero) / (max - zero);
    }

    void SetColor(float alpha)
    {
        currentAlpha = alpha;
        currentAlpha = Mathf.Max(currentAlpha, minAlpha);
        moduleImage.color = new Color(1f, 1f, 1f, currentAlpha);
    }
} 

