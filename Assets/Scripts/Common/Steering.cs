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
	public static Vector2 Seek(Vehicle agent, Vehicle target){
		Vector2 relativePos = target.position - agent.position;
		if(relativePos.magnitude < 1f)
			return Vector2.zero;

		Vector2 desiredVelocity = relativePos.normalized * agent.maxSpeed;

		return (desiredVelocity - agent.velocity);
	}

	/// <summary>
	/// Flee steering
	/// </summary>
	public static Vector2 Flee(Vehicle agent, Vehicle target, float fleeRange){
		Vector2 relativePos = agent.position - target.position;
		if(relativePos.magnitude < fleeRange)
			return relativePos.normalized * agent.maxSpeed - agent.velocity;
		else
			return Vector2.zero;
	}

	/// <summary>
	/// Arrive steering (Linear deceleration)
	/// </summary>
	public static Vector2 Arrive(Vehicle agent, Vehicle target, float deceleration){
		Vector2 relativePos = target.position - agent.position;
		float dist = relativePos.magnitude;

		if(dist > 0){
			float speed = dist / deceleration;
			speed = Mathf.Min(speed, agent.maxSpeed);

			Vector2 desiredVelocity = relativePos.normalized * speed;

			return (desiredVelocity - agent.velocity);
		}

		return Vector2.zero;
	}

	/// <summary>
	/// Pursuit steering
	/// </summary>
	public static Vector2 Pursuit(Vehicle agent, Vehicle target, float predictTweaker){
		Vector2 relativePos = target.position - agent.position;

		float lookAheadTime = relativePos.magnitude / (agent.maxSpeed + target.speed);
		float turnAroundTime = Vector2.Dot(relativePos.normalized, agent.heading) * predictTweaker;
		lookAheadTime += turnAroundTime;

		Vector2 targetPosition = target.position + lookAheadTime * target.velocity;

		return Seek(agent, Vehicle.CreateVirtualVehicle(targetPosition));
	}

	/// <summary>
	/// Evade steering with prediction
	/// </summary>
	public static Vector2 Evade(Vehicle agent, Vehicle target, float predictTweaker, float fleeRange){
		Vector2 relativePos = target.position - agent.position;

		float lookAheadTime = relativePos.magnitude / (agent.maxSpeed + target.speed);
		float turnAroundTime = Vector2.Dot(relativePos.normalized, agent.heading) * predictTweaker;
		lookAheadTime += turnAroundTime;

		Vector2 targetPosition = target.position + lookAheadTime * target.velocity;

		return Flee(agent, Vehicle.CreateVirtualVehicle(targetPosition), fleeRange);
	}

	/// Dictionary used for Wander to store the last target position of an given agent
	private static Dictionary<Vehicle, Vector2> lastTargetPosition = new Dictionary<Vehicle, Vector2>();
	/// <summary>
	/// Wander steering
	/// </summary>
	public static Vector2 Wander(Vehicle agent, float distance, float radius, float jitter){
		Vector2 target = Vector2.zero;
		lastTargetPosition.TryGetValue(agent, out target);

		target += new Vector2(Random.Range(-1f, 1f) * jitter, Random.Range(-1f, 1f) * jitter);
		target.Normalize();
		target *= radius;
		if(lastTargetPosition.ContainsKey(agent))
			lastTargetPosition[agent] = target;
		else
			lastTargetPosition.Add(agent, target);

		Vector2 localTarget = target + new Vector2(0, distance);
		Vector2 worldTarget = agent.TransformPoint(localTarget);

		return (worldTarget - agent.position);
	}

	/// <summary>
	/// Avoid Obstacles Steering
	/// </summary>
	public static Vector2 AvoidObstacle(Vehicle agent, float innerRadius, float outerRadius, List<Obstacle> obs){
		// Tag obstacles
		List<Obstacle> innerObs = new List<Obstacle>();
		foreach(Obstacle ob in obs){
			float sqrDistance = (ob.pos - agent.position).sqrMagnitude;
			float sqrRadius = Mathf.Pow(outerRadius + ob.radius, 2);

			if(sqrDistance < sqrRadius)
				innerObs.Add(ob);
		}

		// 1. Transform from world space to local space
		// 2. Discard obstacles
		// 3. Find closest obstacle
		Obstacle closestOb = null;
		float closestDis = 10000f;
		for(int i = 0; i < innerObs.Count; i++){
			Obstacle ob = innerObs[i];
			ob.localPosition = agent.InverseTransformPoint(ob.pos);

			if(ob.localPosition.y < 0 || Mathf.Abs(ob.localPosition.x) > ob.radius + innerRadius){
				continue;
			}

			float intersectDis = ob.localPosition.y - Mathf.Sqrt(Mathf.Pow(ob.radius + innerRadius, 2) - Mathf.Pow(ob.localPosition.x, 2));

			if(intersectDis < closestDis){
				closestOb = ob;
				closestDis = intersectDis;
			}
		}

		// Calculate steering
		if(closestOb && closestDis < 9999f){
			
			Vector2 steeringForce = new Vector2();

			float multiplier = 1f + (outerRadius - closestOb.localPosition.y) / outerRadius;
			steeringForce.x = (Mathf.Abs(closestOb.localPosition.x) - closestOb.radius - innerRadius) * Mathf.Sign(closestOb.localPosition.x)
				* multiplier;

			float breakingWeight = 0.2f;
			// steeringForce.y = (closestOb.radius - closestOb.localPosition.y) * breakingWeight;
			steeringForce.y = -breakingWeight / Mathf.Abs(closestOb.localPosition.y - closestOb.radius);

			steeringForce = agent.TransformDirection(steeringForce);

			return steeringForce;
		}

		return Vector2.zero;
	}

	/// <summary>
	/// Avoid Wall
	/// </summary>
	public static Vector2 AvoidWall(Vehicle agent, float degree, float length, List<Wall> walls){
		// Generate feelers (default three)
		Vector2[] feelers = new Vector2[3];
		feelers[0] = agent.position + agent.heading * length;
		feelers[1]= agent.position + new Vector2(-Mathf.Sin(degree), Mathf.Cos(degree)) * length;
		feelers[2] = agent.position + new Vector2(Mathf.Sin(degree), Mathf.Cos(degree)) * length;

		Vector2 point = Vector2.zero;
		Vector2 closestPoint = Vector2.zero;
		Wall closestWall = null;
		float maxOvershoot = -1f;
		foreach(Wall w in walls){
			foreach(Vector2 f in feelers){
				if(GetFLinesIntersection(agent.position, f, w.from, w.to, out point)){
					float overshoot = (f - point).sqrMagnitude;
					if(overshoot > maxOvershoot){
						maxOvershoot = Mathf.Sqrt(overshoot);
						closestPoint = point;
						closestWall = w;
					}
				}
			}
		}

		if(closestWall){
			Vector3 auxiliaryNormal = Vector3.Cross((closestWall.to - closestWall.from), closestPoint - agent.position);
			Vector2 normal = Vector3.Cross(auxiliaryNormal, (closestWall.to - closestWall.from)).normalized;

			return -normal * maxOvershoot * 20f;
		}

		return Vector2.zero;
	}

	/// <summary>
	/// Interpose
	/// </summary>
	public static Vector2 Interpose(Vehicle agent, List<Vehicle> targets){
		Vehicle t1 = targets[0];
		Vehicle t2 = targets[1];

		Vector2 midpoint = t1.position * 0.5f + t2.position * 0.5f;
		float time = (agent.position - midpoint).magnitude / agent.maxSpeed;

		Vector2 t1New = t1.position + t1.velocity * time;
		Vector2 t2New = t2.position + t2.velocity * time;
		midpoint = t1New * 0.5f + t2New * 0.5f;

		return Seek(agent, Vehicle.CreateVirtualVehicle(midpoint));
	}

	/// <summary>
	/// Hide
	/// </summary>
	public static Vector2 Hide(Vehicle agent, Vehicle target, List<Obstacle> obs, float hideDistance, 
	float thresholdDistance, float fleeRange){
		Vector2 closestPoint = Vector2.zero;
		float closestDistance = Mathf.Epsilon;
		foreach(Obstacle ob in obs){
			Vector2 point = GetHidingPosition(target.position, ob, hideDistance);
			float sqDis = (point - agent.position).sqrMagnitude;

			if(sqDis > Mathf.Epsilon){
				closestPoint = point;
				closestDistance = Mathf.Sqrt(sqDis);
			}
		}

		if(closestDistance > 2 * Mathf.Epsilon && closestDistance < thresholdDistance){
			return Seek(agent, Vehicle.CreateVirtualVehicle(closestPoint));
		}

		return Flee(agent, target, fleeRange);
	}

	/// Dictionary used for PathFollowing to store the path for given agent
	private static Dictionary<Vehicle, int> currentPoint = new Dictionary<Vehicle, int>();
	/// <summary>
	/// Path Following
	/// </summary>
	public static Vector2 PathFollowing(Vehicle agent, List<Vector2> path, float maxDistance, float decelerationDist, bool loop=false){
		int index = 0;
		currentPoint.TryGetValue(agent, out index);

		if((agent.position - path[index]).sqrMagnitude < Mathf.Pow(maxDistance, 2) && index < path.Count - 1){
			index++;
		}

		if(!loop && index == path.Count - 1){
			currentPoint[agent] = index;
			return Steering.Arrive(agent, Vehicle.CreateVirtualVehicle(path[index]), decelerationDist);			
		}

		if(loop && index == path.Count)
			index = 0;

		currentPoint[agent] = index;

		return Steering.Seek(agent, Vehicle.CreateVirtualVehicle(path[index]));
	}

	/// <summary>
	/// Offset Pursuit
	/// </summary>
	public static Vector2 OffsetPursuit(Vehicle agent, Vehicle leader, Vector2 offset, float predictTweaker=1f){
		Vector2 worldOffsetPosition = leader.TransformPoint(offset);
		Vector2 relativePos = worldOffsetPosition - agent.position;

		float lookAheadTime = relativePos.magnitude / (agent.maxSpeed + leader.speed);
		float turnAroundTime = Vector2.Dot(relativePos.normalized, agent.heading) * predictTweaker;
		lookAheadTime += turnAroundTime;

		Vector2 targetPosition = worldOffsetPosition + lookAheadTime * leader.velocity;

		return Arrive(agent, Vehicle.CreateVirtualVehicle(targetPosition), 1f);
	}

	/// <summary>
	/// Separation Steering
	/// </summary>
	public static Vector2 Separation(Vehicle agent, List<Vehicle> neighbors){
		Vector2 steering = Vector2.zero;

		foreach(Vehicle v in neighbors){
			Vector2 toNeighbor = agent.position - v.position;
			if(toNeighbor == Vector2.zero)
				toNeighbor = RandomUnitVector2();

			steering += toNeighbor.normalized / toNeighbor.magnitude;
		}

		return steering;
	}

	/// <summary>
	/// Alignment Steering
	/// </summary>
	public static Vector2 Alignment(Vehicle agent, List<Vehicle> neighbors){
		Vector2 steering = Vector2.zero;

		foreach(Vehicle v in neighbors){
			steering += v.heading;
		}

		return steering / neighbors.Count;
	}

	/// <summary>
	/// Cohesion Steering
	/// </summary>
	public static Vector2 Cohesion(Vehicle agent, List<Vehicle> neighbors){
		Vector2 massCenter = Vector2.zero;

		foreach(Vehicle v in neighbors){
			massCenter += v.position;
		}

		return Seek(agent, Vehicle.CreateVirtualVehicle(massCenter / neighbors.Count));
	}

	// ------	Private Function	------

	private static bool GetLinesIntersection(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out Vector2 point){
		float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
	
		if (tmp == 0)
		{
			// No solution!
			point = Vector2.zero;
			return false;
		}
	
		float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;
	
		point =  new Vector2(
			B1.x + (B2.x - B1.x) * mu,
			B1.y + (B2.y - B1.y) * mu
		);

		return true;
	}

	private static bool GetFLinesIntersection(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out Vector2 point){
		if(GetLinesIntersection(A1, A2, B1, B2, out point))
			if((point.x - A1.x) * (point.x - A2.x) <= 0 && (point.x - B1.x) * (point.x - B2.x) <= 0
			&& (point.y - A1.y) * (point.y - A2.y) <= 0 && (point.y - B1.y) * (point.y - B2.y) <= 0)
				return true;

		point = Vector2.zero;
		return false;
	}

	private static Vector2 GetHidingPosition(Vector2 pos, Obstacle ob, float hideDistance){
		Vector2 dir = (ob.pos - pos).normalized;

		return ob.pos + dir * (hideDistance + ob.radius);
	}

	private static Vector2 RandomUnitVector2(){
		return Random.insideUnitCircle.normalized;
	}
}