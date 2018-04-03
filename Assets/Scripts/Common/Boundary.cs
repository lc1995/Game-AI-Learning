using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the boundary in the world.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class Boundary : MonoBehaviour {

	/// ------	Public Variables	------
	public float width = 30f;
	public float height = 20f;

	/// ------	Private Variables	------
	private LineRenderer lr;
	#region BorderPoint
	public Vector2 lt;
	public Vector2 rt;
	public Vector2 ld;
	public Vector2 rd;
	#endregion

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer>();
		lr.positionCount = 4;
		lr.loop = true;
		lr.startColor = Color.red;
		lr.endColor = Color.red;
		lr.widthMultiplier = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		DrawBoundary();

		foreach(Agent a in Agent.AllAgents){
			if((rt.x - a.data.position.x) * (ld.x - a.data.position.x) > 0
			|| (rt.y - a.data.position.y) * (ld.y - a.data.position.y) > 0){
				a.SetPosition(-a.data.position + a.data.velocity * Time.deltaTime * 2f);
			}
		}
	}

	private void DrawBoundary(){
		float halfWidth = width * 0.5f;
		float halfHeight = height * 0.5f;
		lt = new Vector2(-halfWidth, halfHeight);
		rt = new Vector2(halfWidth, halfHeight);
		ld = new Vector2(-halfWidth, -halfHeight);
		rd = new Vector2(halfWidth, -halfHeight);

		lr.SetPosition(0, lt);
		lr.SetPosition(1, rt);
		lr.SetPosition(2, rd);
		lr.SetPosition(3, ld);
	}
}
