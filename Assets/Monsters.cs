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


	void OnMouseUp() {
		if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(0)) {
	 		Debug.Log("Click something");
      // do something
   }

	}
}
