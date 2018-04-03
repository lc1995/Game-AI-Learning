using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	public static List<Agent> AllAgents = new List<Agent>();

	/// ------	Public Variables	------
	public float mass = 1.0f;
	public float maxSpeed = 5.0f;
	public float maxForce = 10.0f;
	public float maxTurnRate = 5.0f;
	public float accTweaker = 1.0f;

	public List<Agent> targets = new List<Agent>();
	public _SteeringAI ai = null;

	/// ------	Shared Variables	------
	public Vehicle data{get; private set;}

	void Start(){
		Vector2 position = transform.position;
		Vector2 heading = transform.up;
		data = new Vehicle(mass, maxSpeed, maxForce, maxTurnRate, accTweaker, position, heading, Vector2.zero);

		AllAgents.Add(this);
	}

	void Update(){
		List<Vehicle> targetsData = new List<Vehicle>();
		foreach(Agent t in targets)
			targetsData.Add(t.data);

		Vector2 steering;
		if(ai)
			steering = ai.CalculateSteering(data, targetsData);
		else
			steering = Vector2.zero;
		data.Update(steering);

		transform.position = data.position;
		transform.rotation = Quaternion.FromToRotation(Vector3.up, data.velocity);
	}

	/// <summary>
	/// Directly change the position of the agent
	/// </summary>
	public void SetPosition(Vector2 newPos){
		data.position = newPos;
		transform.position = newPos;
	}
}
