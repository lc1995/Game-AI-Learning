using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitAgent : Agent {

	protected override Vector2 CalculateSteering(){
		if(target){
			Vector2 tVelocity = target.GetComponent<Agent>().velocity;
			return Pursuit(target.position, tVelocity);
		}
		else
			return Vector2.zero;
	}
}
