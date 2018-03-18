using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/PursuitAI")]
public class PursuitAI : _SteeringAI {

	public float predictTweaker = 1.0f;

	public override Vector2 CalculateSteering(Transform agent, Transform target){
		Agent agentData = agent.GetComponent<Agent>();
		Agent targetData = target.GetComponent<Agent>();

		return Steering.Pursuit(agent.position, target.position, agentData.velocity, agentData.maxSpeed,
			targetData.velocity, agent.up, predictTweaker);
	}
}
