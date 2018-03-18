using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSortingLayer : MonoBehaviour {

	public TrailRenderer tr;

	// Use this for initialization
	void Start () {
		Debug.Log(tr.sortingLayerName);
		Debug.Log(tr.sortingOrder);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
