using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TimeMap : MonoBehaviour {

	public GameObject selectedUnit;
	public GameObject selectedMonster;
	public GameObject selectedMon1;
	public GameObject newSpawn;

	public bool[,] occupationArray = new bool[8, 8];
	int[,] tiles;
	public int occupiedCount = 0;
    public bool gameOver = false;


    // Other scripts
	public Unit unit;
	public Monsters monsters;
    public CarryMon carryMon;
	public TileType[] tileTypes;
	public ClickableTile clickableTile;
	public Helper helper;

	public List<GameObject> monsterList = new List<GameObject>();

    // Game settings
	int highestLevel = 1;
	float sinceLastSpawn = 4;
	int initialSpawn = 0;
	int mapSizeX = 6;
	int mapSizeY = 6;
    float timeLeft = 4.0f;
	int counter = 0;

	//Dynamic difficulty
	static int max = 3;
	static int min = 1;
	static float F = 1.01f;
	
	//Monster spawning weights
	static int maxMonsterLevelPossible = 8;
	float[] weights = new float[maxMonsterLevelPossible];
	static float multiplier = 2/3f;
	
	int timesCombined = 0;

	void Start ()
	{
		weights [0] = 1;
		for (int i = 1; i < maxMonsterLevelPossible; i++) {
			weights [i] = weights [i - 1] * multiplier;
		}

		// Putting the position of units
		selectedUnit.GetComponent<Unit>().carrying = false;
		selectedUnit.GetComponent<Unit>().tileX = 4;
		selectedUnit.GetComponent<Unit>().tileY = 4;
		selectedMonster.GetComponent<Monsters>().tileX = Random.Range(0,6);
		selectedMonster.GetComponent<Monsters>().tileY = Random.Range(0,6);

		for (int i = 0; i < mapSizeX; i++) {
			for (int j = 0; j < mapSizeY; j++) {
				occupationArray [i, j] = false;
			}
		}
		selectedMonster.transform.position = TileCoordToWorldCoord(selectedMonster.GetComponent<Monsters>().tileX, selectedMonster.GetComponent<Monsters>().tileY);

		SpawnMon();
		SpawnMon();
		counterIncrease();

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

	}

	void GenerateMapVisual() {
		for(int x=0; x < mapSizeX; x++) {
			for(int	 y=0; y < mapSizeY; y++) {
				TileType tt = tileTypes[ tiles[x,y] ];
				GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);
				if (tt.name == "Floor") {
					if ((x + y) % 2 == 1) {
						go.GetComponent<Renderer> ().material.color = new Color (130/255.0f, 128/255.0f, 156/255.0f, 1);
					}
				}
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
	
	public void counterIncrease() {
		counter++;
		sinceLastSpawn++;
	}

    // Defines what level monster is being carried
	public void CarryMonster(GameObject mon) {

		occupationArray [mon.GetComponent<Monsters>().tileX, mon.GetComponent<Monsters>().tileY] = false;
		selectedUnit.GetComponent<Unit> ().carryingLevel = mon.GetComponent<Monsters> ().level;
		selectedUnit.GetComponent<Unit>().carrying = true;

		monsterList.Remove(mon);
		mon.GetComponent<Monsters>().destroy (true);

        carryMon.changeCarrying();

		occupiedCount--;
	}

    // Spawning monsters
	public void SpawnMon() {
		float delay = spawnDelay(timesCombined);
		if(sinceLastSpawn > delay || initialSpawn < 2) {
		    initialSpawn++;
			sinceLastSpawn -= delay;
			var randomX = Random.Range(0,6);
			var randomY = Random.Range(0,6);

            // re-roll for reasons:
            // 1. can't be on top of player
            // 2. x and y is occupied
            while((randomX == selectedUnit.GetComponent<Unit>().tileX && randomY == selectedUnit.GetComponent<Unit>().tileY) ||
                occupationArray[randomX, randomY] == true) {

                if (monsterList.Count < 36 &&
                    ((randomX == selectedUnit.GetComponent<Unit>().tileX && // x=0 y= 1 || -1
                    Mathf.Abs(randomY - selectedUnit.GetComponent<Unit>().tileY) == 1) ||
                    (Mathf.Abs(randomX - selectedUnit.GetComponent<Unit>().tileX) == 1 && // x= 1 || -1 y=0
                    randomY == selectedUnit.GetComponent<Unit>().tileY))) {
                        randomX = Random.Range(0,6);
                        randomY = Random.Range(0,6);
                } else {
                    randomX = Random.Range(0,6);
                    randomY = Random.Range(0,6);
                }

            }

            occupiedCount++;
   			occupationArray [randomX, randomY] = true;

			newSpawn = (GameObject)Instantiate(selectedMonster.GetComponent<Monsters>().Monster, new Vector3(randomX, randomY, -1), Quaternion.identity);

			newSpawn.GetComponent<Monsters>().tileX = randomX;
			newSpawn.GetComponent<Monsters>().tileY = randomY;
			newSpawn.GetComponent<Monsters> ().level = newSpawnLevel (highestLevel);
			newSpawn.GetComponent<Monsters> ().UpdateSprite ();

			monsterList.Add(newSpawn);
			connect(newSpawn);
		}


	}
	
	float spawnDelay(int timesCombined) {
		return ((max-min)/(Mathf.Pow(F, Mathf.Max(timesCombined, 1))))+min;	
	}

	//Randomly pick a level to spawn based on the highest level so far
	int newSpawnLevel(int highestLevel)
	{
		if(highestLevel == 1)
			return 1;
		float sum = 0;
		for (int i = 0; i < (highestLevel - 1); i++) {
			sum += weights[i];
		}
		float randomvalue = Random.Range(0, sum);
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
			//Check if same level
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
							q.Enqueue (neighbourMonster);
							neighbourMonster.markDestroy = true;
						}
					}
				}

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
						monsterList.Remove (g);
						gmonster.destroy (false);
					}


					// dropping position to be CHANGED.
					GameObject newSpawn	= (GameObject)Instantiate(selectedMonster.GetComponent<Monsters>().Monster, new Vector3(selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileY, -1), Quaternion.identity);
					//newSpawn.gameObject.tag = "Monster1";
					newSpawn.GetComponent<Monsters>().tileX = selectedUnit.GetComponent<Unit>().tileX;
					newSpawn.GetComponent<Monsters>().tileY = selectedUnit.GetComponent<Unit>().tileY;
					newSpawn.GetComponent<AudioSource> ().Play ();

					// fuse to a higher level monster
					if(newSpawn.GetComponent<Monsters>().level < 8) {
                        newSpawn.GetComponent<Monsters>().level = mon.level + 1;
                        newSpawn.GetComponent<Monsters> ().UpdateSprite ();
                        highestLevel = Mathf.Max(highestLevel, mon.level + 1);
                    // When monster level goes over 8, it spawns lvl 1 monster
                    } else {
                        newSpawn.GetComponent<Monsters>().level = 1;
                        newSpawn.GetComponent<Monsters> ().UpdateSprite ();
                        highestLevel = 8;
                    }

                    timesCombined++;
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
			}
		}
	}


	public void MoveSelectedUnitTo(int x, int y) {

        // Player carrying movement
		if(selectedUnit.GetComponent<Unit>().carrying == true) {
			for(int i = 0; i < monsterList.Count; i++) {
				if(monsterList[i].GetComponent<Monsters>().tileX == x && monsterList[i].GetComponent<Monsters>().tileY == y) {
					print("You Cant Move Here!");
					return;
				}
			}
			if((selectedUnit.GetComponent<Unit>().tileX - x == -1 || selectedUnit.GetComponent<Unit>().tileX - x == 1) && selectedUnit.GetComponent<Unit>().tileY - y == 0) {
                helper.MoveAndSpawn(x, y);
			} else if (selectedUnit.GetComponent<Unit>().tileX - x == 0 && (selectedUnit.GetComponent<Unit>().tileY - y == -1 || selectedUnit.GetComponent<Unit>().tileY - y == 1)) {
                helper.MoveAndSpawn(x, y);
			}

		} else { // carrying false, moving left right, up down
			if ((selectedUnit.GetComponent<Unit>().tileX - x == -1 || selectedUnit.GetComponent<Unit>().tileX - x == 1) && selectedUnit.GetComponent<Unit>().tileY - y == 0) {
				helper.MoveAndSpawn(x, y);
		} else if (selectedUnit.GetComponent<Unit>().tileX - x == 0 && (selectedUnit.GetComponent<Unit>().tileY - y == -1 || selectedUnit.GetComponent<Unit>().tileY - y == 1)) {
				helper.MoveAndSpawn(x, y);
			}

			//pick up by moving on top of monster
			if(monsterList.Count != 0) {
				for(int i = 0; i < monsterList.Count; i++) {
					if(monsterList[i].GetComponent<Monsters>().tileX == selectedUnit.GetComponent<Unit>().tileX && monsterList[i].GetComponent<Monsters>().tileY == selectedUnit.GetComponent<Unit>().tileY) {
						
						occupiedCount--;
						occupationArray [monsterList[i].GetComponent<Monsters>().tileX, monsterList[i].GetComponent<Monsters>().tileY] = false;
						CarryMonster(monsterList[i]);
						counterIncrease();
					}
				}
			}
		}
	}

	void Update() {

	    // game ends when player drops(not carrying) the 36th monster on floor and monsterList.Count more than 36
	    if (monsterList.Count >= 36 && !selectedUnit.GetComponent<Unit>().carrying) {
	        gameOver = true;
            timeLeft -= Time.deltaTime;
            print("Time Left: " + Mathf.Round(timeLeft));
            if(timeLeft < 0)
                SceneManager.LoadScene (2);
	    }
	}
}
