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

	public Transform target;

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
		acc = Vector2.ClampMagnitude(CalculateSteering() * accelerationTweaker, maxAcc);

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

	/// ------	Steerings	------

	/// <summary>
	/// Calculate Steerings
	/// </summary>
	protected virtual Vector2 CalculateSteering(){
		return Vector2.zero;
	}

	/// <summary>
	/// Seek steering
	/// </summary>
	protected Vector2 Seek(Vector2 target){
		Vector2 desiredVelocity = (target - (Vector2)transform.position).normalized * maxSpeed;

		return (desiredVelocity - velocity);
	}

	/// <summary>
	/// Flee steering
	/// </summary>
	protected Vector2 Flee(Vector2 target){
		Vector2 relativePos = (Vector2)transform.position - target;
		if(relativePos.sqrMagnitude < fleeRange)
			return relativePos.normalized * maxSpeed - velocity;
		else
			return Vector2.zero;
	}

	/// <summary>
	/// Arrive steering (Linear deceleration)
	/// </summary>
	protected Vector2 Arrive(Vector2 target){
		Vector2 relativePos = target - (Vector2)transform.position;
		if(relativePos.sqrMagnitude > decelerateRange)
			return Seek(target);
		else{
			return relativePos.normalized / decelerateRange * maxSpeed - velocity;
		}
	}

	/// <summary>
	/// Pursuit steering
	/// </summary>
	protected Vector2 Pursuit(Vector2 target, Vector2 tVelocity){
		Vector2 relativePos = target - (Vector2)transform.position;

		float lookAheadTime = relativePos.magnitude / (maxSpeed + tVelocity.magnitude);
		float turnAroundTime = Vector2.Dot(relativePos.normalized, transform.up) * turnRateTweaker;
		lookAheadTime += turnAroundTime;

		return Seek((Vector2)target + tVelocity * lookAheadTime);
	}
}
