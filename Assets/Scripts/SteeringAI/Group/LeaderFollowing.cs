using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/LeaderFollowing")]
public class LeaderFollowing : _SteeringAI {

	public float unitDistance = 3.0f;
	public float neighborRadius = 10.0f;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		List<Vehicle> neighbors = new List<Vehicle>();

		foreach(Vehicle v in Vehicle.AllVehicles){
			if(v != agent && (v.position - agent.position).magnitude < neighborRadius){
				neighbors.Add(v);
			}
		}

		Vector2 separation = Steering.Separation(agent, neighbors);
		Vector2 offsetPursuit = Steering.OffsetPursuit(agent, targets[0], new Vector2(0, -unitDistance));

		return (separation * 3f + offsetPursuit);
	}
}
