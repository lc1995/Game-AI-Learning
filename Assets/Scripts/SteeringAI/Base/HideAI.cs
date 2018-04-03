using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/HideAI")]
public class HideAI : _SteeringAI {

	public float hideDistance = 3.0f;
	public float hideThreshold = 20.0f;
	public float fleeRange = 5.0f;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		return Steering.Hide(agent, targets[0], Obstacle.AllObstacles, hideDistance, hideThreshold, fleeRange);
	}
}
