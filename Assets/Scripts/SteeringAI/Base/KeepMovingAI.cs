using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/KeepMovingAI")]
public class KeepMovingAI : _SteeringAI {

	public Vector2 direction = Vector2.zero;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		return direction;
	}
}
