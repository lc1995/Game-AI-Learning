using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SteeringAI/PathFollowingAI")]
public class PathFollowingAI : _SteeringAI {

	public List<Vector2> path = new List<Vector2>();
	public float maxDistance = 1f;
	public float decelerationDistance = 5f;
	public bool loop = false;

	public override Vector2 CalculateSteering(Vehicle agent, List<Vehicle> targets){
		if(path.Count < 2)
			return Vector2.zero;
		else
			return Steering.PathFollowing(agent, path, maxDistance, decelerationDistance, loop);
	}
}
