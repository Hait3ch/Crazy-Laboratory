using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TimeMap map;
	public Unit unit;
	public Helper helper;

	void OnMouseUp() {
		//Debug.Log("Click");
		map.MoveSelectedUnitTo(tileX, tileY); // can move anywhere
/*
		if(unit.carrying == false) {
		} else { // can only move on empty spots
			helper.isCarrying();
		}
		*/
	}
	void Update() {
		if (Input.GetMouseButtonDown(0))
				Debug.Log("Pressed left click.");

		if (Input.GetMouseButtonDown(1))
				Debug.Log("Pressed right click.");
			}
}
