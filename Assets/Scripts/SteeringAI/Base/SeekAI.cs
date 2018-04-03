using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/SeekAI")]
public class SeekAI : _SteeringAI {

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		if(targets.Count == 0)
			return Vector2.zero;
		else
			return Steering.Seek(agent, targets[0]);
	}
}
