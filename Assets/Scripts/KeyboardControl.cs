using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour {

	// ------	Public Variables	------
	public Agent agent;
	public float maxSpeed = 10.0f;
	public Vector2 velocity;

	// ------	Private Variables	------
	private GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

		velocity = new Vector2(h, v) * maxSpeed;
		transform.position += new Vector3(h, v, 0) * maxSpeed * Time.deltaTime;
	}
}
