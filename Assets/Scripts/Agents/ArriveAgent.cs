using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveAgent : Agent {

	protected override Vector2 CalculateSteering(){
		if(target)
			return Arrive(target.position);
		else
			return Vector2.zero;
	}
}
