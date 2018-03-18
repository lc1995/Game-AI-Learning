using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	// ------	Public Variables	------
	public float mass = 1.0f;

	public float maxSpeed = 10.0f;
	public float maxAcc = 20.0f;
	public float accelerationTweaker = 1.0f;
	public float turnRateTweaker = 1.0f;

	public List<Transform> targets = new List<Transform>(1);

	public _SteeringAI steeringAI;

	[Header("Flee")]
	public float fleeRange = 30.0f;
	[Header("Arrive")]
	public float decelerateRange = 10.0f;

	// ------	Shared Variables	------
	public Vector2 acc {get; protected set;}
	public Vector2 velocity {get; protected set;}

	// ------	Protected Variables	------
	protected Vector2 lastPos;

	// ------	Required Components	------
	protected LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
		acc = Vector2.zero;
		velocity = Vector2.zero;
		lastPos = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if(steeringAI == null || targets.Count == 0)
			acc = Vector2.zero;
		else if(targets.Count == 1)
			acc = steeringAI.CalculateSteering(transform, targets[0]);
		else
			acc = steeringAI.CalculateSteering(transform, targets);

		acc = Vector2.ClampMagnitude(acc * accelerationTweaker, maxAcc);

		if(acc.sqrMagnitude > Mathf.Epsilon){
			velocity += acc * Time.deltaTime;
			velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
		}
		else
			velocity = Vector2.zero;

		transform.position += (Vector3)velocity * Time.deltaTime;

		if(velocity.sqrMagnitude > Mathf.Epsilon)
			transform.rotation = Quaternion.LookRotation(Vector3.forward, velocity);
	}
}
