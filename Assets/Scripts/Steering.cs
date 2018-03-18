using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This static provides base steering bahaviors.
/// </summary>
public static class Steering{

	/// <summary>
	/// Seek steering
	/// </summary>
	public static Vector2 Seek(Vector2 pos, Vector2 tPos, Vector2 vel, float maxSpeed){
		Vector2 desiredVelocity = (tPos - pos).normalized * maxSpeed;

		return (desiredVelocity - vel);
	}

	/// <summary>
	/// Flee steering
	/// </summary>
	public static Vector2 Flee(Vector2 pos, Vector2 tPos, Vector2 vel, float maxSpeed, float fleeRange){
		Vector2 relativePos = pos - tPos;
		if(relativePos.sqrMagnitude < fleeRange)
			return relativePos.normalized * maxSpeed - vel;
		else
			return Vector2.zero;
	}

	/// <summary>
	/// Arrive steering (Linear deceleration)
	/// </summary>
	public static Vector2 Arrive(Vector2 pos, Vector2 tPos, Vector2 vel, float maxSpeed, float decelerateRange){
		Vector2 relativePos = tPos - pos;
		if(relativePos.sqrMagnitude > decelerateRange)
			return Steering.Seek(pos, tPos, vel, maxSpeed);
		else{
			return relativePos.normalized / decelerateRange * maxSpeed - vel;
		}
	}

	/// <summary>
	/// Pursuit steering
	/// </summary>
	public static Vector2 Pursuit(Vector2 pos, Vector2 tPos, Vector2 vel, float maxSpeed, Vector2 tVel, Vector2 upDir, float predictTweaker){
		Vector2 relativePos = tPos - pos;

		float lookAheadTime = relativePos.magnitude / (maxSpeed + tVel.magnitude);
		float turnAroundTime = Vector2.Dot(relativePos.normalized, upDir) * predictTweaker;
		lookAheadTime += turnAroundTime;

		return Seek(pos, tPos + tVel * lookAheadTime, vel, maxSpeed);
	}
}
