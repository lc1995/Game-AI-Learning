using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/FleeAI")]
public class FleeAI : _SteeringAI {

	public float fleeRange = 20.0f;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		if(targets.Count == 0)
			return Vector2.zero;
		else
			return Steering.Flee(agent, targets[0], fleeRange);
	}
}
