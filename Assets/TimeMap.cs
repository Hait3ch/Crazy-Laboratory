using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeMap : MonoBehaviour {


	public GameObject selectedUnit;
	public GameObject selectedMonster;
	public GameObject selectedMon1;
	public GameObject newSpawn;

	public Unit unit;
	public Monsters monsters;
	public TileType[] tileTypes;
	public ClickableTile clickableTile;

	public List<GameObject> monsterList = new List<GameObject>();



	int[,] tiles;
	public bool carryingMove = false; // if true then cant move on monster
	//public bool carrying = false;
	int mapSizeX = 8;
	int mapSizeY = 8;
	int counter = 0;

	void Start() {

		// Putting the position of units
		selectedUnit.GetComponent<Unit>().carrying = false;
		selectedUnit.GetComponent<Unit>().tileX = 4;
		selectedUnit.GetComponent<Unit>().tileY = 4;
		selectedMonster.GetComponent<Monsters>().tileX = Random.Range(1,7);
		selectedMonster.GetComponent<Monsters>().tileY = Random.Range(1,7);
		SpawnMon();
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
			tiles[i, 7] = 1;
			tiles[0, i] = 1;
			tiles[7, i] = 1;
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


	public void CarryMonster(GameObject mon) {
		Destroy(mon);
		monsterList.Remove(mon);
		selectedUnit.GetComponent<Unit>().carrying = true;
		//clickableTile.GetComponent<ClickableTile>().carrying = true;
	}

	public void SpawnMon() {

		if(counter % 5 == 0) {
			var randomX = Random.Range(1,7);
			var randomY = Random.Range(1,7);

			if(randomX == selectedUnit.GetComponent<Unit>().tileX && randomY == selectedUnit.GetComponent<Unit>().tileY) {
				SpawnMon();
			}

			newSpawn = (GameObject)Instantiate(selectedMonster.GetComponent<Monsters>().Monster, new Vector3(randomX, randomY, -1), Quaternion.identity);
			newSpawn.gameObject.tag = "Monster1";
			newSpawn.GetComponent<Monsters>().tileX = randomX;
			newSpawn.GetComponent<Monsters>().tileY = randomY;

			monsterList.Add(newSpawn);
		}


	}
	//TODO ANOTHER WAY TO IMPLEMENT PICKUPS
	void OnTriggerEnter2D(Collider2D other) {
	        Destroy(other.gameObject);
	    }


	public void MoveSelectedUnitTo(int x, int y) {

		print("moving");
		if(selectedUnit.GetComponent<Unit>().carrying == true) {
			print("tru");
			for(int i = 0; i < monsterList.Count; i++) {
				print("for " + counter);
				if(monsterList[i].GetComponent<Monsters>().tileX == x && monsterList[i].GetComponent<Monsters>().tileY == y) {
					print("You Cant Move Here!");
					return;
				}
			}
			if((selectedUnit.GetComponent<Unit>().tileX - x == -1 || selectedUnit.GetComponent<Unit>().tileX - x == 1) && selectedUnit.GetComponent<Unit>().tileY - y == 0) {

					SpawnMon();
					selectedUnit.GetComponent<Unit>().tileX = x;
					selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
					counter++;
			} else if (selectedUnit.GetComponent<Unit>().tileX - x == 0 && (selectedUnit.GetComponent<Unit>().tileY - y == -1 || selectedUnit.GetComponent<Unit>().tileY - y == 1)) {

					SpawnMon();
					selectedUnit.GetComponent<Unit>().tileY = y;
					selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
					counter++;
			}

		} else { // carrying false, moving left right, up down
			if ((selectedUnit.GetComponent<Unit>().tileX - x == -1 || selectedUnit.GetComponent<Unit>().tileX - x == 1) && selectedUnit.GetComponent<Unit>().tileY - y == 0) {
				SpawnMon();
				selectedUnit.GetComponent<Unit>().tileX = x;
				selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
				counter++;
		} else if (selectedUnit.GetComponent<Unit>().tileX - x == 0 && (selectedUnit.GetComponent<Unit>().tileY - y == -1 || selectedUnit.GetComponent<Unit>().tileY - y == 1)) {
				SpawnMon();
				selectedUnit.GetComponent<Unit>().tileY = y;
				selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
				counter++;
			}
			if(monsterList.Count != 0) {
				print(monsterList);
				for(int i = 0; i < monsterList.Count; i++) {
					if(monsterList[i].GetComponent<Monsters>().tileX == selectedUnit.GetComponent<Unit>().tileX && monsterList[i].GetComponent<Monsters>().tileY == selectedUnit.GetComponent<Unit>().tileY) {
						CarryMonster(monsterList[i]);
						counter++;
					}
				}


			}

		}
	}
}
