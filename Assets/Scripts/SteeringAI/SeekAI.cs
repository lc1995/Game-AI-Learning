using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/SeekAI")]
public class SeekAI : _SteeringAI {

	public override Vector2 CalculateSteering(Transform agent, Transform target){
		Agent agentData = agent.GetComponent<Agent>();

		return Steering.Seek(agent.position, target.position, agentData.velocity, agentData.maxSpeed);
	}
}
