using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekAgent : Agent {

	protected override Vector2 CalculateSteering(){
		if(target)
			return Seek(target.position);
		else
			return Vector2.zero;
	}
}
