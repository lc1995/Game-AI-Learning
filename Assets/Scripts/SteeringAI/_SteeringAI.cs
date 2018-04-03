using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _SteeringAI : ScriptableObject {

	public abstract Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets);
}