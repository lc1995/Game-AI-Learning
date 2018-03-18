using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour {

	// ------	Public Variables	------
	public Agent agent;
	public GameObject prefab;

	// ------	Private Variables	------
	private GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			if(target != null)
				Destroy(target);

			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target = Instantiate(prefab, pos, Quaternion.identity);
			if(agent.targets.Count == 0)
				agent.targets.Add(target.transform);
			else
				agent.targets[0] = target.transform;
		}
	}
}
