using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour {

	Material material;

	float time = 0;
	float scale = 1;
	float speed = 12f;
	float alpha = 1f;

	enum Mode {Idle, Zoom, LoopWait};
	Mode mode; 

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(1, 1, 1);
		mode = Mode.Idle;
		material = gameObject.GetComponent<Renderer>().material;

		alpha = 0f;
        material.color = new Color(1f, 1f, 1f, alpha);
	}
	
	// Update is called once per frame
	void Update () {

		switch (mode)
		{
			case Mode.Idle:
				break;

			case Mode.Zoom:
				time += Time.deltaTime;
				scale = scale * (1f + Time.deltaTime * speed);
//				alpha = Mathf.Min(1f / Mathf.Pow(1.03f, scale), 0.6f);
				alpha = 0.6f / Mathf.Pow(1.03f, scale);
		        material.color = new Color(1f, 1f, 1f, alpha);
				transform.localScale = new Vector3(scale, scale, scale);
				if (scale > 300f)
				{
					mode = Mode.LoopWait;
					scale = 0;
					time = 0;
					transform.localScale = new Vector3(scale, scale, scale);
				}
				break;

			case Mode.LoopWait:
				time += Time.deltaTime;
				if (time > 1.0f)
				{
					time = 0;
					scale = 1;
					mode = Mode.Idle;
				}
				break;
		}
	}

	public void Ping()
	{
		time = 0;
		scale = 1;
		alpha = 0f;
        material.color = new Color(1f, 1f, 1f, alpha);
		mode = Mode.Zoom;
	}

}
