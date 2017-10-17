using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour {

	public int tileX;
	public int tileY;


	public GameObject Monster;
	//public GameObject selectedUnit;

	public TileType[] tileTypes;
	int[,] tiles;

	public bool carryable = false;

	void OnMouseOver() {
		if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) {
	 		Debug.Log("Click");
      // do something
   }

	}
}
