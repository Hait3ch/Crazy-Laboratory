using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeMap : MonoBehaviour {

	public GameObject selectedUnit;

	public GameObject selectedMonster;
	public TileType[] tileTypes;
	int[,] tiles;

	int mapSizeX = 10;
	int mapSizeY = 10;
	int counter = 0;

	void Start() {


		// Putting the position of units
		selectedUnit.GetComponent<Unit>().tileX = 4;
		selectedUnit.GetComponent<Unit>().tileY = 4;
		selectedMonster.GetComponent<Monsters>().tileX = Random.Range(1,8);
		selectedMonster.GetComponent<Monsters>().tileY = Random.Range(1,8);
counter++;

		Debug.Log(counter + " mon x " + 		selectedMonster.GetComponent<Monsters>().tileX + " y " +selectedMonster.GetComponent<Monsters>().tileY);

selectedMonster.transform.position = TileCoordToWorldCoord(selectedMonster.GetComponent<Monsters>().tileX, selectedMonster.GetComponent<Monsters>().tileY);

		GenerateMapData();
		GenerateMapVisual();
	}
	void GenerateMapData() {
		// Allocate our map tiles
		tiles = new int[mapSizeX, mapSizeY];

		// Initialize our map tiles with floors
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeY; y++) {
				tiles[x,y] = 0;
			}
		}
		// Put Walls in correct locations
		for(int i=0; i < mapSizeX; i++) {
			tiles[i, 0] = 1;
			tiles[i, 9] = 1;
			tiles[0, i] = 1;
			tiles[9, i] = 1;

		}

	}

	void GenerateMapVisual() {
		for(int x=0; x < mapSizeX; x++) {
			for(int	 y=0; y < mapSizeY; y++) {
				TileType tt = tileTypes[ tiles[x,y] ];

				GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);

				ClickableTile ct = go.GetComponent<ClickableTile>();
				ct.tileX = x;
				ct.tileY = y;
				ct.map = this;

			}
		}
	}

	public Vector3 TileCoordToWorldCoord(int x, int y) {
		return new Vector3(x, y ,0);
	}


	public void MoveSelectedUnitTo(int x, int y) {
		// initial place values



 // Debug.Log("mon x y " + selectedMonster.GetComponent<Monsters>().tileX + selectedMonster.GetComponent<Monsters>().tileY + " player" +
  // selectedUnit.GetComponent<Unit>().tileX + selectedUnit.GetComponent<Unit>().tileY);


/*
		// if else for pick up measure
		if((selectedUnit.GetComponent<Unit>().tileX - x == -1 || selectedUnit.GetComponent<Unit>().tileX - x == 1) && selectedUnit.GetComponent<Unit>().tileY - y == 0) {
			// Debug.Log("true" + selectedUnit.GetComponent<Unit>().tileX + " " + x + " -1");
			selectedUnit.GetComponent<Unit>().tileX = x;
			selectedUnit.transform.position = TileCoordToWorldCoord(x, y);

		} else if (selectedUnit.GetComponent<Unit>().tileX - x == 0 && (selectedUnit.GetComponent<Unit>().tileY - y == -1 || selectedUnit.GetComponent<Unit>().tileY - y == 1)) {
			// Debug.Log("UP (-1)");
			selectedUnit.GetComponent<Unit>().tileY = y;
			selectedUnit.transform.position = TileCoordToWorldCoord(x, y);

		} else {
			// Debug.Log("ELSE");
		}
*/

	// Player moving on board
	selectedUnit.GetComponent<Unit>().tileX = x;
	selectedUnit.GetComponent<Unit>().tileY = y;
	selectedUnit.transform.position = TileCoordToWorldCoord(x, y);


		// if else for Destroy Monsters
    try {
			//Debug.Log("!!mon x y " + selectedMonster.GetComponent<Monsters>().tileX + selectedMonster.GetComponent<Monsters>().tileY + " player" +
		  // selectedUnit.GetComponent<Unit>().tileX + selectedUnit.GetComponent<Unit>().tileY);
			if (selectedMonster.GetComponent<Monsters>().tileX == selectedUnit.GetComponent<Unit>().tileX && selectedMonster.GetComponent<Monsters>().tileY == selectedUnit.GetComponent<Unit>().tileY) {

				Destroy(selectedMonster);

			} else if((selectedMonster.GetComponent<Monsters>().tileX - x == -1 || selectedMonster.GetComponent<Monsters>().tileX - x == 1) && selectedMonster.GetComponent<Monsters>().tileY - y == 0) {
				Debug.Log("!!!mon x: " + selectedMonster.GetComponent<Monsters>().tileX + " y: " + selectedMonster.GetComponent<Monsters>().tileY + " player x:" +
				selectedUnit.GetComponent<Unit>().tileX + " y: " + selectedUnit.GetComponent<Unit>().tileY);

				selectedMonster.GetComponent<Monsters>().carryable = true;

				Debug.Log("next to Monsters" + counter);
			} else if (selectedMonster.GetComponent<Monsters>().tileX - x == 0 && (selectedMonster.GetComponent<Monsters>().tileY - y == -1 || selectedMonster.GetComponent<Monsters>().tileY - y == 1)) {

				Debug.Log("next to Monsters"+ counter);

				selectedMonster.GetComponent<Monsters>().carryable = true;
			}
			counter++;
		}


	catch {

			Debug.Log("error"); // goes here if it's destroyed
	}
	}
}
