using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obstacle class
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class Obstacle : MonoBehaviour{

	public static List<Obstacle> AllObstacles = new List<Obstacle>();

	/// ------	Public Variables	------
	public Vector2 pos = Vector2.zero;
	public float radius = 1.0f;

	/// ------	Required Components	------
	[HideInInspector]
	public Vector2 localPosition = Vector2.zero;
	private LineRenderer lr;

	private const float PI = 3.1415f;

	void Start(){
		AllObstacles.Add(this);

		lr = GetComponent<LineRenderer>();
		lr.widthMultiplier = 0.03f;

		float theta_scale = 0.1f;             //Set lower to add more points
		int size = (int)Mathf.Floor((2.0f * PI) / theta_scale) + 1; //Total number of points in circle.
		lr.positionCount = size;
	}

	void Update(){
		pos = transform.position;

		int i = 0;
		for(float theta = 0; theta < 2 * PI; theta += 0.1f) {
			float x = radius * Mathf.Cos(theta);
			float y = radius * Mathf.Sin(theta);

			Vector3 position = pos + new Vector2(x, y);
			lr.SetPosition(i, position);
			i+=1;
		}
	}
}
