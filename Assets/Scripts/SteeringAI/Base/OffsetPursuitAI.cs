using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/OffsetPursuitAI")]
public class OffsetPursuitAI : _SteeringAI {

	public Vector2 offset = Vector2.zero;
	public float predictTweaker = 1f;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		if(targets.Count > 0)
			return Steering.OffsetPursuit(agent, targets[0], offset, predictTweaker);
		else
			return Vector2.zero;
	}
}
