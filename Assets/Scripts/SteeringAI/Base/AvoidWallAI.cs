using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="SteeringAI/AvoidWall")]
public class AvoidWallAI : _SteeringAI {

	public float feelerDegree = 20.0f;
	public float feelerLength = 3.0f;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		Vector2 steering = Steering.AvoidWall(agent, feelerDegree, feelerLength, Wall.AllWalls);
		if(steering != Vector2.zero)
			return steering;
		else
			return agent.heading * 5f;
	}
}
