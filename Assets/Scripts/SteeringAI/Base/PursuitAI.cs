using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/PursuitAI")]
public class PursuitAI : _SteeringAI {

	public float predictTweaker = 1.0f;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		if(targets.Count == 0)
			return Vector2.zero;
		else
			return Steering.Pursuit(agent, targets[0], predictTweaker);
	}
}
