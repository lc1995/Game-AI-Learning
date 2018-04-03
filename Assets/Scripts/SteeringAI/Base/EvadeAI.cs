using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/EvadeAI")]
public class EvadeAI : _SteeringAI {

	public float predictTweaker = 1.0f;
	public float fleeRange = 5.0f;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		if(targets.Count == 0)
			return Vector2.zero;
		else
			return Steering.Evade(agent, targets[0], predictTweaker, fleeRange);
	}
}
