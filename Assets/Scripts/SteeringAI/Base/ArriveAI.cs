using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/ArriveAI")]
public class ArriveAI : _SteeringAI {

	public float deceleration = 0.6f;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		if(targets.Count == 0)
			return Vector2.zero;
		else
			return Steering.Arrive(agent, targets[0], deceleration);
	}
}
