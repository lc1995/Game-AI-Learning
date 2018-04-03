using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/InterposeAI")]
public class InterposeAI : _SteeringAI {

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		if(targets.Count == 2)
			return Steering.Interpose(agent, targets);

		return Vector2.zero;
	}
}
