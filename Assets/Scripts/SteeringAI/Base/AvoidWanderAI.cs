using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="SteeringAI/AvoidWander")]
public class AvoidWanderAI : _SteeringAI {

	public float innerRadius = 2f;
	public float outerRadius = 6f;

	public float radius = 2f;
	public float distance = 2f;
	public float jilter = 2f;

	private Dictionary<Vehicle, Vector2> lastTargetPos = new Dictionary<Vehicle, Vector2>();

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		Vector2 avoidSteering = Steering.AvoidObstacle(agent, innerRadius, outerRadius, Obstacle.AllObstacles);

		if(avoidSteering != Vector2.zero)
			return avoidSteering;
		else
			return Steering.Wander(agent, distance, radius, jilter);
	}
}
