using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/SeparationArrive")]
public class SeparationArrive : _SteeringAI {

	public float neighborRadius = 10.0f;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		List<Vehicle> neighbors = new List<Vehicle>();

		foreach(Vehicle v in Vehicle.AllVehicles){
			if(v != agent && (v.position - agent.position).magnitude < neighborRadius){
				neighbors.Add(v);
			}
		}

		Vector2 separation = Steering.Separation(agent, neighbors);
		Vector2 arrive = Steering.Arrive(agent, targets[0], 0.6f);

		return (separation * 3f + arrive);
	}
}
