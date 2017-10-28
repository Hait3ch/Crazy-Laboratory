using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeMap : MonoBehaviour {


	public GameObject selectedUnit;
	public GameObject selectedMonster;
	public GameObject selectedMon1;
	public GameObject newSpawn;

	public bool[,] occupationArray = new bool[8, 8];
	public int occupiedCount = 0;

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

		for (int i = 0; i < mapSizeX; i++) {
			for (int j = 0; j < mapSizeY; j++) {
				occupationArray [i, j] = false;
			}
		}

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
		mon.GetComponent<Monsters>().destroy ();
		monsterList.Remove(mon);
		selectedUnit.GetComponent<Unit>().carrying = true;
		//clickableTile.GetComponent<ClickableTile>().carrying = true;
	}

	//TODO stop monsters from spawning on top of existing monsters
	public void SpawnMon() {

		if(counter % 3 == 0) {
			var randomX = Random.Range(1,7);
			var randomY = Random.Range(1,7);
			print (occupationArray [randomX, randomY]);
			print ("aaaaawtf");

			while (occupiedCount < 36 && occupationArray [randomX, randomY] == true) {
				print ("uh oh occupied");
				print (randomX);
				print (randomY);
				randomX = Random.Range(1,7);
				randomY = Random.Range(1,7);
			}

			/*
			if(randomX == selectedUnit.GetComponent<Unit>().tileX && randomY == selectedUnit.GetComponent<Unit>().tileY) {
				SpawnMon();
			}
			*/

			newSpawn = (GameObject)Instantiate(selectedMonster.GetComponent<Monsters>().Monster, new Vector3(randomX, randomY, -1), Quaternion.identity);
			newSpawn.gameObject.tag = "Monster1";
			newSpawn.GetComponent<Monsters>().tileX = randomX;
			newSpawn.GetComponent<Monsters>().tileY = randomY;

			occupiedCount++;
			occupationArray [randomX, randomY] = true;
			print (occupiedCount);

			monsterList.Add(newSpawn);
			connect(newSpawn);
		}


	}

	//Connect a monster to its neighbouring monsters
	public void connect(GameObject monster) {
		Monsters mon = monster.GetComponent<Monsters> ();
		if (mon.neighbours == null) {
			mon.neighbours = new HashSet<GameObject> ();
		}
		foreach (GameObject g in monsterList) {
			Monsters m = g.GetComponent<Monsters> ();
			//TODO Check if same level
			if ((m.tileX == (mon.tileX - 1)&&m.tileY == mon.tileY) || 
				(m.tileX == (mon.tileX + 1)&&m.tileY == mon.tileY) ||
				(m.tileY == (mon.tileY - 1)&&m.tileX == mon.tileX) || 
				(m.tileY == (mon.tileY + 1))&&m.tileX == mon.tileX) {
				m.neighbours.Add (monster);
				int useless = mon.neighbours.Count;
				mon.neighbours.Add (g);
				print ("Linked" + mon.tileX + " " + mon.tileY + " to " + m.tileX + " " + m.tileY);
			}
		}
	}
	//For the argument GameObject, check if it is connected to at least 3 monsters.
	//If that is the case, use BFS to explore the entire connected set of monsters and remove all of them
	public void fuse(GameObject monster) {
		Monsters mon = monster.GetComponent<Monsters> ();
		HashSet<Monsters> set = new HashSet<Monsters> ();
		set.Add (mon);
		foreach (GameObject g in mon.neighbours) {
			set.Add (g.GetComponent<Monsters>());
		}
		foreach (Monsters m in set) {
			// If >= 3 connected
			if (m.neighbours.Count >= 1) {
				List<GameObject> monsterObjectToRemove = new List<GameObject>{ };

				//Do BFS to find all the connecting monsters, and mark them all as to be destroyed
				Queue<Monsters> q = new Queue<Monsters> ();
				q.Enqueue (mon);
				while (q.Count > 0) {
					Monsters cur = q.Dequeue ();
					cur.markDestroy = true;
					monsterObjectToRemove.Add (monster);

					foreach (GameObject neighbour in cur.neighbours) {
						print (neighbour);
						Monsters neighbourMonster = neighbour.GetComponent<Monsters> ();
						if (!neighbourMonster.markDestroy) {
							q.Enqueue (neighbourMonster);
							neighbourMonster.markDestroy = true;
							//monsterObjectToRemove.Add (neighbour);
						}
					}
				}

				/*
				print ("monsterObjectToRemove.Count+ " + monsterObjectToRemove.Count);
				if (monsterObjectToRemove.Count >= 3) {
					foreach (GameObject monsterObject in monsterObjectToRemove) {
						Monsters gmonster = monsterObject.GetComponent<Monsters> ();
						monsterList.Remove (monsterObject);
						gmonster.destroy ();
						occupiedCount--;
						occupationArray [gmonster.tileX, gmonster.tileY] = false;
					}
				}

*/

				//Find the to-be-removed monsters
				List<GameObject> toremove = new List<GameObject> ();
				foreach (GameObject g in monsterList) {
					Monsters gmonster = g.GetComponent<Monsters> ();
					if (gmonster.markDestroy) {
						toremove.Add (g);
					}
				}

				if (toremove.Count >= 3) {
					foreach (GameObject g in toremove) {
						Monsters gmonster = g.GetComponent<Monsters> ();
						occupiedCount--;
						occupationArray [gmonster.tileX, gmonster.tileY] = false;
						gmonster.destroy ();
						monsterList.Remove (g);
					}

				}

				/*
				//Remove them from the monsterlist
				foreach (GameObject woad in toremove) {
					monsterList.Remove (woad);
				}
				*/


			}
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
						
						occupiedCount--;
						occupationArray [monsterList[i].GetComponent<Monsters>().tileX, monsterList[i].GetComponent<Monsters>().tileY] = false;
						CarryMonster(monsterList[i]);
						counter++;
					}
				}


			}

		}
	}
}
