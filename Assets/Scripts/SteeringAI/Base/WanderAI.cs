using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/WanderAI")]
public class WanderAI : _SteeringAI {

	public float distance = 3.0f;
	public float radius = 2.0f;
	public float jilter = 1.0f;

	private Dictionary<Vehicle, Vector2> lastTargetPos = new Dictionary<Vehicle, Vector2>();

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		return Steering.Wander(agent, distance, radius, jilter);
	}
}
