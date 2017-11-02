using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour {

	public int tileX;
	public int tileY;

	public bool carrying;
	public int carryingLevel;

	public TimeMap map;




	void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            map.MoveSelectedUnitTo(Convert.ToInt32(Math.Max(0, transform.position.x - 1)), Convert.ToInt32(transform.position.y)); // can move anywhere
            }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            map.MoveSelectedUnitTo(Convert.ToInt32(Mathf.Min(5, transform.position.x + 1)), Convert.ToInt32(transform.position.y));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            map.MoveSelectedUnitTo(Convert.ToInt32(transform.position.x), Convert.ToInt32(Mathf.Min(5,transform.position.y + 1)));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            map.MoveSelectedUnitTo(Convert.ToInt32(transform.position.x), Convert.ToInt32(Mathf.Max(0, transform.position.y - 1)));
        }
    }
}
