using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wall class
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class Wall : MonoBehaviour {

	public static List<Wall> AllWalls = new List<Wall>();

	/// ------	Public Variables	------
	public Vector2 from;
	public Vector2 to;

	/// ------	Required Components	------
	private LineRenderer lr;

	void Start(){
		AllWalls.Add(this);

		lr = GetComponent<LineRenderer>();
		lr.widthMultiplier = 0.1f;
		lr.positionCount = 2;
		lr.SetPosition(0, from);
		lr.SetPosition(1, to);
	}

	void Update(){
		lr.SetPosition(0, from);
		lr.SetPosition(1, to);
	}
}
