using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoAnim : MonoBehaviour {
/*
     (-81,78)                  (78, 78)
           (-35,56)      (32, 56)
 (-137,23)         (0, 23)            (135, 23)
           (-35,-10)     (32,-10)
     (-81,-32)                 (78,-32)

r = 55
center2 = (78,23)


           ___
       ___    |
    ___       | 66
  ____________|
      66

*/

	Vector2 pos;
	enum Mode { HIDE, R2L, LRotation, L2R, RRotation_1, RRotation_2};
	Mode mode;
	float theta;
	float theta_zero;
	float speed = 12.0f;
	float liner_speed;
	float wait_time;

	RectTransform rt;
	Image im;

	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform>();
		im = GetComponent<Image>();

		mode = Mode.HIDE;
		wait_time = 0;
		im.enabled = false;

		pos = new Vector2(78, -32);
		theta = Mathf.PI * 1.5f;
		theta_zero = Mathf.PI / 4;

		float rot_speed = speed * 2f * Mathf.PI * 55f;
		liner_speed = rot_speed / Mathf.Sqrt(2) / 5;

	}
	
	// Update is called once per frame
	void Update () {
		float delta;

		switch (mode) {
			case Mode.HIDE:
				wait_time += Time.deltaTime;
				if (wait_time > 4f) {
					mode = Mode.RRotation_2;
					im.enabled = true;
				}
				break;
			case Mode.R2L:
				delta = Time.deltaTime * liner_speed;
				pos = new Vector2(pos.x - delta , pos.y + delta);
				if (pos.x <= -35) {
					mode = Mode.LRotation;
					theta = theta_zero;
				}
				break;
			case Mode.LRotation:
				theta += Time.deltaTime * speed;
				if (theta > Mathf.PI * 2) {
					theta -= Mathf.PI * 2;
				}
				pos = new Vector2(-81 + Mathf.Cos(theta) * 55, 23 + Mathf.Sin(theta) * 55);
				if (pos.x > -35) {
					mode = Mode.L2R;
				}
				break;
				break;
			case Mode.L2R:
				delta = Time.deltaTime * liner_speed;
				pos = new Vector2(pos.x + delta , pos.y + delta);
				if (pos.x >= 32) {
					mode = Mode.RRotation_1;
					theta = Mathf.PI - theta_zero;
				}
				break;
			case Mode.RRotation_1:
				theta -= Time.deltaTime * speed;
				if (theta < 0) {
					theta += Mathf.PI * 2;
				}
				pos = new Vector2(78 + Mathf.Cos(theta) * 55, 23 + Mathf.Sin(theta) * 55);
				if ((pos.x < 78) && (pos.y < 23))
				{
					mode = Mode.HIDE;
					wait_time = 0;
					im.enabled = false;
				}
				break;
			case Mode.RRotation_2:
				theta -= Time.deltaTime * speed;
				if (theta < 0) {
					theta += Mathf.PI * 2;
				}
				pos = new Vector2(78 + Mathf.Cos(theta) * 55, 23 + Mathf.Sin(theta) * 55);
				if (pos.x < 32) {
					mode = Mode.R2L;
				}
				break;
		}
		rt.anchoredPosition = pos;
	}
}
