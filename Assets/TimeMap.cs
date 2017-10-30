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
	int highestLevel = 1;

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

	//Monster spawning weights
	static int maxMonsterLevelPossible = 8;
	float[] weights = new float[maxMonsterLevelPossible];


	void Start ()
	{
		weights [0] = 1;
		for (int i = 1; i < maxMonsterLevelPossible; i++) {
			weights [i] = weights [i - 1] * 2 / 3;
			print (weights [i]);
		}

		// Putting the position of units
		selectedUnit.GetComponent<Unit>().carrying = false;
		selectedUnit.GetComponent<Unit>().tileX = 4;
		selectedUnit.GetComponent<Unit>().tileY = 4;
		selectedMonster.GetComponent<Monsters>().tileX = Random.Range(1,7);
		selectedMonster.GetComponent<Monsters>().tileY = Random.Range(1,7);

		for (int i = 0; i < mapSizeX; i++) {
			for (int j = 0; j < mapSizeY; j++) {
				occupationArray [i, j] = false;
			}
		}


		Debug.Log(counter + " mon x " + 		selectedMonster.GetComponent<Monsters>().tileX + " y " +selectedMonster.GetComponent<Monsters>().tileY);

		selectedMonster.transform.position = TileCoordToWorldCoord(selectedMonster.GetComponent<Monsters>().tileX, selectedMonster.GetComponent<Monsters>().tileY);

		SpawnMon();
		counter++;

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
		print ("taking away from " + mon.GetComponent<Monsters>().tileX + mon.GetComponent<Monsters>().tileY);
		occupationArray [mon.GetComponent<Monsters>().tileX, mon.GetComponent<Monsters>().tileY] = false;
		selectedUnit.GetComponent<Unit> ().carryingLevel = mon.GetComponent<Monsters> ().level;
		print ("carrying level is " + selectedUnit.GetComponent<Unit> ().carryingLevel);
		monsterList.Remove(mon);
		mon.GetComponent<Monsters>().destroy ();
		selectedUnit.GetComponent<Unit>().carrying = true;
		occupiedCount--;
		//clickableTile.GetComponent<ClickableTile>().carrying = true;
	}

	//TODO stop monsters from spawning on top of existing monsters
	public void SpawnMon() {

		if(counter % 3 == 0) {
			var randomX = Random.Range(1,7);
			var randomY = Random.Range(1,7);

			print (selectedUnit.GetComponent<Unit> ().tileX);
			print (selectedUnit.GetComponent<Unit> ().tileY);
			while ((occupiedCount < 36 && occupationArray [randomX, randomY] == true) || (randomX == selectedUnit.GetComponent<Unit>().tileX && randomY == selectedUnit.GetComponent<Unit>().tileY)) {
				print ("uh oh occupied");
				print ("blocking at " + randomX + randomY);
				randomX = Random.Range(1,7);
				randomY = Random.Range(1,7);
			}
			print ("isOccupied " + occupationArray [randomX, randomY]);
			print ("spawning at" + randomX + randomY);

			/*
			if(randomX == selectedUnit.GetComponent<Unit>().tileX && randomY == selectedUnit.GetComponent<Unit>().tileY) {
				SpawnMon();
			}
			*/

			newSpawn = (GameObject)Instantiate(selectedMonster.GetComponent<Monsters>().Monster, new Vector3(randomX, randomY, -1), Quaternion.identity);
			newSpawn.gameObject.tag = "Monster1";
			newSpawn.GetComponent<Monsters>().tileX = randomX;
			newSpawn.GetComponent<Monsters>().tileY = randomY;
			newSpawn.GetComponent<Monsters> ().level = newSpawnLevel (highestLevel);
			newSpawn.GetComponent<Monsters> ().UpdateSprite ();

			occupiedCount++;
			occupationArray [randomX, randomY] = true;
			print (occupiedCount);

			monsterList.Add(newSpawn);
			connect(newSpawn);
		}


	}

	//Randomly pick a level to spawn based on the highest level so far
	int newSpawnLevel(int highestLevel)
	{
		print("Spawning a monster, highest lvl: " + highestLevel);
		if(highestLevel == 1)
			return 1;
		float sum = 0;
		for (int i = 0; i < (highestLevel - 1); i++) {
			sum += weights[i];
		}
		float randomvalue = Random.Range(0, sum);
		print("from 0 to " + sum + " chose " + randomvalue);
		for (int i = 0; i < (highestLevel - 1); i++) {
			if(randomvalue < weights[i]) {
				return i + 1;
			} else {
				randomvalue -= weights[i];
			}
		}
		print("should not have gotten here!");
		return 1;
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
			if (m.level == mon.level &&
				((m.tileX == (mon.tileX - 1)&&m.tileY == mon.tileY) || 
				(m.tileX == (mon.tileX + 1)&&m.tileY == mon.tileY) ||
				(m.tileY == (mon.tileY - 1)&&m.tileX == mon.tileX) || 
					(m.tileY == (mon.tileY + 1))&&m.tileX == mon.tileX)) {
				if (m.neighbours == null) {
					m.neighbours = new HashSet<GameObject> ();
				}
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
						Monsters neighbourMonster = neighbour.GetComponent<Monsters> ();
						if (!neighbourMonster.markDestroy) {
							print ("hey this is neighbors:" + neighbourMonster.tileX + neighbourMonster.tileY);
							print ("neighbor count:" + cur.neighbours.Count);
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
					print ("clear count" + toremove.Count);
					foreach (GameObject g in toremove) {
						Monsters gmonster = g.GetComponent<Monsters> ();
						occupiedCount--;
						occupationArray [gmonster.tileX, gmonster.tileY] = false;
						monsterList.Remove (g);
						gmonster.destroy ();
					}


					// dropping position to be CHANGED.
					print("spawnpoint: " + selectedUnit.GetComponent<Unit>().tileX + " y: " + selectedUnit.GetComponent<Unit>().tileY);
					GameObject newSpawn	= (GameObject)Instantiate(selectedMonster.GetComponent<Monsters>().Monster, new Vector3(selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileY, -1), Quaternion.identity);
					//newSpawn.gameObject.tag = "Monster1";
					newSpawn.GetComponent<Monsters>().tileX = selectedUnit.GetComponent<Unit>().tileX;
					newSpawn.GetComponent<Monsters>().tileY = selectedUnit.GetComponent<Unit>().tileY;

					// fuse to a higher level monster
					newSpawn.GetComponent<Monsters>().level = mon.level + 1;
					newSpawn.GetComponent<Monsters> ().UpdateSprite ();
					highestLevel = Mathf.Max(highestLevel, mon.level + 1);
					monsterList.Add(newSpawn);
					occupiedCount++;
					occupationArray [selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileY] = true;

					connect(newSpawn);

					fuse (newSpawn);

				} else {
					foreach (GameObject g in toremove) {
						Monsters gmonster = g.GetComponent<Monsters> ();
						gmonster.markDestroy = false;
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
	/*
	void OnTriggerEnter2D(Collider2D other) {
	        Destroy(other.gameObject);
	    }
	    */


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

					
					selectedUnit.GetComponent<Unit>().tileX = x;
					selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
					counter++;
				SpawnMon();
			} else if (selectedUnit.GetComponent<Unit>().tileX - x == 0 && (selectedUnit.GetComponent<Unit>().tileY - y == -1 || selectedUnit.GetComponent<Unit>().tileY - y == 1)) {
				
					selectedUnit.GetComponent<Unit>().tileY = y;
					selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
					counter++;
				SpawnMon();
			}

		} else { // carrying false, moving left right, up down
			if ((selectedUnit.GetComponent<Unit>().tileX - x == -1 || selectedUnit.GetComponent<Unit>().tileX - x == 1) && selectedUnit.GetComponent<Unit>().tileY - y == 0) {
				selectedUnit.GetComponent<Unit>().tileX = x;
				selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
				counter++;
				SpawnMon();
		} else if (selectedUnit.GetComponent<Unit>().tileX - x == 0 && (selectedUnit.GetComponent<Unit>().tileY - y == -1 || selectedUnit.GetComponent<Unit>().tileY - y == 1)) {
				selectedUnit.GetComponent<Unit>().tileY = y;
				selectedUnit.transform.position = TileCoordToWorldCoord(x, y);
				counter++;
				SpawnMon();
			}

			//pick up
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
