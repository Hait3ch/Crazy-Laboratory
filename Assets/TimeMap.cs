using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeMap : MonoBehaviour {

	public GameObject selectedUnit;
	public GameObject selectedMonster;
	public GameObject selectedMon1;
	GameObject newSpawn;

	public TileType[] tileTypes;

	List<GameObject> monsterList = new List<GameObject>();

	int[,] tiles;

	int mapSizeX = 8;
	int mapSizeY = 8;
	int counter = 0;

	void Start() {


		// Putting the position of units
		selectedUnit.GetComponent<Unit>().tileX = 4;
		selectedUnit.GetComponent<Unit>().tileY = 4;
		selectedMonster.GetComponent<Monsters>().tileX = Random.Range(1,7);
		selectedMonster.GetComponent<Monsters>().tileY = Random.Range(1,7);


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





	public void DestroyMonster() {

		try {
			foreach(GameObject thisMonster in monsterList) {
				if(thisMonster.GetComponent<Monsters>().tileX == selectedUnit.GetComponent<Unit>().tileX && thisMonster.GetComponent<Monsters>().tileY == selectedUnit.GetComponent<Unit>().tileY) {
					print("POS of MONSTER: (x,y) (" + thisMonster.GetComponent<Monsters>().tileX + ", " + thisMonster.GetComponent<Monsters>().tileY + ")");
					print(monsterList.Count);

					Destroy(thisMonster);
					monsterList.Remove(thisMonster);
				}
				print("co " + counter);
			}

		} catch {
			Debug.Log("DestroyMonster Error");
		}
	}

	public void SpawnMon() {






		//Debug.Log("newMon " + objList.Count + "    " + selectedUnit.GetComponent<Unit>().tileX);
		if(counter % 5 == 0) {
			var randomX = Random.Range(1,7);
			var randomY = Random.Range(1,7);

			newSpawn = (GameObject)Instantiate(selectedMonster.GetComponent<Monsters>().Monster, new Vector3(randomX, randomY, -1), Quaternion.identity);
			newSpawn.gameObject.tag = "Monster1";
			newSpawn.GetComponent<Monsters>().tileX = randomX;
			newSpawn.GetComponent<Monsters>().tileY = randomY;

			monsterList.Add(newSpawn);



			//Debug.Log(" counter % 5 == 0" );

		}


	}
	//TODO ANOTHER WAY TO IMPLEMENT PICKUPS
	void OnTriggerEnter2D(Collider2D other) {
	        Destroy(other.gameObject);
	    }

	public void MoveSelectedUnitTo(int x, int y) {


		SpawnMon();
		DestroyMonster();

		// Player moving on board
		// Moving Unit
		if ((selectedUnit.GetComponent<Unit>().tileX - x == -1 || selectedUnit.GetComponent<Unit>().tileX - x == 1) && selectedUnit.GetComponent<Unit>().tileY - y == 0) {
			// Debug.Log("true" + selectedUnit.GetComponent<Unit>().tileX + " " + x + " -1");
			selectedUnit.GetComponent<Unit>().tileX = x;
			selectedUnit.transform.position = TileCoordToWorldCoord(x, y);

		} else if (selectedUnit.GetComponent<Unit>().tileX - x == 0 && (selectedUnit.GetComponent<Unit>().tileY - y == -1 || selectedUnit.GetComponent<Unit>().tileY - y == 1)) {
			// Debug.Log("UP (-1)");
			selectedUnit.GetComponent<Unit>().tileY = y;
			selectedUnit.transform.position = TileCoordToWorldCoord(x, y);

		}

		// if else for Destroy Monsters
		try {


			if (selectedMonster.GetComponent<Monsters>().tileX == selectedUnit.GetComponent<Unit>().tileX && selectedMonster.GetComponent<Monsters>().tileY == selectedUnit.GetComponent<Unit>().tileY) {

				Destroy(newSpawn);

				Destroy(selectedMonster);

			} else if((selectedMonster.GetComponent<Monsters>().tileX - x == -1 || selectedMonster.GetComponent<Monsters>().tileX - x == 1) && selectedMonster.GetComponent<Monsters>().tileY - y == 0) {
				Debug.Log("!!!mon x: " + selectedMonster.GetComponent<Monsters>().tileX + " y: " + selectedMonster.GetComponent<Monsters>().tileY + " player x:" +
				selectedUnit.GetComponent<Unit>().tileX + " y: " + selectedUnit.GetComponent<Unit>().tileY);

				selectedMonster.GetComponent<Monsters>().carryable = true;

				Debug.Log("next to Monsters" + counter +  " " + selectedMonster.GetComponent<Monsters>().carryable);
			} else if (selectedMonster.GetComponent<Monsters>().tileX - x == 0 && (selectedMonster.GetComponent<Monsters>().tileY - y == -1 || selectedMonster.GetComponent<Monsters>().tileY - y == 1)) {

				Debug.Log("next to Monsters"+ counter);

				selectedMonster.GetComponent<Monsters>().carryable = true;
			}

		}


	catch {

			Debug.Log("error"); // goes here if it's destroyed
	}
	//Debug.Log(counter + " counter");
	counter++;
	}
	/*
	void Update() {

		Debug.Log(counter + " UPDATE");
		selectedMonster.GetComponent<Monsters>().tileX = Random.Range(1,8);
		selectedMonster.GetComponent<Monsters>().tileY = Random.Range(1,8);

				GenerateMapData();
				GenerateMapVisual();
	}
*/
}
