using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _SteeringAI : ScriptableObject {

	public virtual Vector2 CalculateSteering(Transform agent, Transform target){
		return Vector2.zero;
	}

	public virtual Vector2 CalculateSteering(Transform agent, List<Transform> targets){
		return Vector2.zero;
	}
}