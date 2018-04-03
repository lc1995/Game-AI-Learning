using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class SelectTargetFromMouse : MonoBehaviour {

	private Agent virtualAgent;

	// Use this for initialization
	void Start () {
		virtualAgent = gameObject.GetComponent<Agent>();
	}
	
	// Update is called once per frame
	void Update () {
		virtualAgent.SetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10));
	}
}
