using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeMap : MonoBehaviour {
	
	public GameObject selectedUnit;
	public TileType[] tileTypes;
	int[,] tiles;
	
	int mapSizeX = 10;
	int mapSizeY = 10;
	
	void Start() {
		
		GenerateMapData();
		GenerateMapVisual();
		// Also change unit position in game
		selectedUnit.GetComponent<Unit>().tileX = 4;
		selectedUnit.GetComponent<Unit>().tileY = 4;
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
		
		//Debug.Log("status: " + selectedUnit.GetComponent<Unit>().tileX + " int x: "+ x +" "+ selectedUnit.GetComponent<Unit>().tileY + " "+ y);
		if((selectedUnit.GetComponent<Unit>().tileX - x == -1 || selectedUnit.GetComponent<Unit>().tileX - x == 1) && selectedUnit.GetComponent<Unit>().tileY - y == 0) {
			Debug.Log("true" + selectedUnit.GetComponent<Unit>().tileX + " " + x + " -1");
			selectedUnit.GetComponent<Unit>().tileX = x;
			selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
			
		} else if (selectedUnit.GetComponent<Unit>().tileX - x == 0 && (selectedUnit.GetComponent<Unit>().tileY - y == -1 || selectedUnit.GetComponent<Unit>().tileY - y == 1)) {
			Debug.Log("UP (-1)");
			selectedUnit.GetComponent<Unit>().tileY = y;
			selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
	
		} else {
			Debug.Log("ELSE");
		}
		
		

		
		
	}
}
