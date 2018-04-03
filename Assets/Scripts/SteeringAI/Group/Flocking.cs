using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/Flocking")]
public class Flocking : _SteeringAI {

	public float neighborRadius = 10.0f;
	[Range(0f, 3f)]
	public float separation = 0.5f;
	[Range(0f, 3f)]
	public float alignment = 0.5f;
	[Range(0f, 3f)]
	public float cohesion = 0.5f;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		List<Vehicle> neighbors = new List<Vehicle>();

		foreach(Vehicle v in Vehicle.AllVehicles){
			if(v != agent && (v.position - agent.position).magnitude < neighborRadius){
				neighbors.Add(v);
			}
		}

		Vector2 sepSteering = Steering.Separation(agent, neighbors) * separation;
		Vector2 aliSteering = Steering.Alignment(agent, neighbors) * alignment;
		Vector2 cohSteering = Steering.Cohesion(agent, neighbors) * cohesion;
		Vector2 wanSteering = Steering.Wander(agent, 2f, 2f, 1f);

		Vector2 steering = Vector2.zero;
		if(AccumulateForce(agent, sepSteering, ref steering) && AccumulateForce(agent, cohSteering, ref steering)
			&& AccumulateForce(agent, aliSteering, ref steering) && AccumulateForce(agent, wanSteering, ref steering))
			return steering;

		return steering;
	}

	private bool AccumulateForce(Vehicle agent, Vector2 force, ref Vector2 steering){
		if(steering.magnitude >= agent.maxForce){
			steering = Vector2.ClampMagnitude(steering, agent.maxForce);
			return false;
		}

		steering += force;
		if(steering.magnitude >= agent.maxForce){
			steering = Vector2.ClampMagnitude(steering, agent.maxForce);
			return false;
		}else{
			return true;
		}
	}
}
