using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vehicle class
/// </summary>
public class Vehicle {

	public static List<Vehicle> AllVehicles = new List<Vehicle>();
	private static Vehicle VirtualVehicle = new Vehicle(0, 0, 0, 0, 0, Vector2.zero, Vector2.zero, Vector2.zero);

	/// ------	Public Variables	------
	public float mass;
	public float maxSpeed;
	public float maxForce;
	public float maxTurnRate;
	public float accTweaker;

	public Vector2 position;
	public Vector2 heading;
	public Vector2 velocity;

	public Vector2 side{
		get{
			return Vector3.Cross(heading, Vector3.forward);
		}
	}
	public float speed{
		get{
			return velocity.sqrMagnitude;
		}
	}

	public Vehicle(float m, float mS, float mF, float mTR, float tweaker, Vector2 initPos, Vector2 initHeading, Vector2 initVel){
		mass = m;
		maxSpeed = mS;
		maxForce = mF;
		maxTurnRate = mTR;
		accTweaker = tweaker;
		position = initPos;
		velocity = initVel;
		heading = initHeading;	

		if(m > Mathf.Epsilon)
			AllVehicles.Add(this);	
	}
	
	public void Update(Vector2 steering){
		Vector2 acceleration = Vector2.ClampMagnitude(steering * accTweaker, maxForce) / mass;
		if(acceleration.magnitude > Mathf.Epsilon){
			velocity += acceleration * Time.deltaTime;
			velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
		}

		position += velocity * Time.deltaTime;
		
		if(velocity != Vector2.zero)
			heading = velocity.normalized;
	}

	/// <summary>
	/// Transform point from local to world
	/// <summary>
	public Vector2 TransformPoint(Vector2 point){
		return position + heading * point.y + side * point.x;
	}

	/// <summary>
	/// Transform point from world to local
	/// </summary>
	public Vector2 InverseTransformPoint(Vector2 point){
		Vector2 relative = point - position;
		float tmp = heading.y * side.x - heading.x * side.y;
		float x = (heading.y * relative.x - heading.x * relative.y) / tmp;
		float y = (relative.y * side.x - relative.x * side.y) / tmp;

		return new Vector2(x, y);
	}

	public Vector2 TransformDirection(Vector2 point){
		return TransformPoint(point) - position;
	}

	public static Vehicle CreateVirtualVehicle(Vector2 pos){
		VirtualVehicle.position = pos;

		return VirtualVehicle;
	}
}
