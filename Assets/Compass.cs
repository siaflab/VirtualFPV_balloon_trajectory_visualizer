using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {

	public Material material;
	GameObject compassObj;
	Transform parent;
	float theta = 0;

	float bias = 183f;

	// Use this for initialization
	void Start () {
		compassObj = RingGenerator.Create(0.25f, 0.15f, 100, material, false);
		compassObj.transform.parent = transform;
		compassObj.transform.localPosition = Vector3.zero;
		compassObj.transform.localEulerAngles = Vector3.zero;
		compassObj.transform.localScale = new Vector3(2f, 3f, 2f);

		parent = transform.parent.transform;
	}
	
	// Update is called once per frame
	void Update () {
		theta += 1f;
		//compassObj.transform.localEulerAngles = new Vector3(0f, theta, 0f);
		compassObj.transform.localEulerAngles = new Vector3(0f, parent.localEulerAngles.y + bias, 0f);
	}
}
