using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TimeMap map;
	public Unit unit;


	void OnMouseUp() {
		//Debug.Log("Click");
		//print("carr"  + carrying + " "+ selectedUnit.GetComponent<Unit>().carrying);


		print("player: " + tileX + " y: " + tileY);

		map.MoveSelectedUnitTo(tileX, tileY); // can move anywhere


	}

	void fuse() {
		
	}

	void Update() {
		//print("Carrying: " + map.selectedUnit.GetComponent<Unit>().carrying);
		if (Input.GetMouseButtonDown(1));
				//Debug.Log("Pressed right click.");

		/*
		if (map.occupationArray [tileX, tileY] == true) {
			//gameObject.GetComponent<Renderer>().material.color = new Color (0,1,0,1);
			Renderer rend = GetComponent<Renderer> ();
			//Set the initial color (0f,0f,0f,0f)
			rend.material.color = Color.black;
		} else {
			Renderer rend = GetComponent<Renderer> ();
			//Set the initial color (0f,0f,0f,0f)
			rend.material.color = Color.cyan;
		}
		*/

    if (Input.GetMouseButtonDown(1) && map.selectedUnit.GetComponent<Unit>().carrying) {
				map.selectedUnit.GetComponent<Unit>().carrying = false;
				//map.selectedMonster.GetComponent<Monsters>().tileX = tileX;
				//map.selectedMonster.GetComponent<Monsters>().tileY = tileY;

				print("spawnpoint: " + map.selectedUnit.GetComponent<Unit>().tileX + " y: " + map.selectedUnit.GetComponent<Unit>().tileY);
			GameObject newSpawn	= (GameObject)Instantiate(map.selectedMonster.GetComponent<Monsters>().Monster, new Vector3(map.selectedUnit.GetComponent<Unit>().tileX, map.selectedUnit.GetComponent<Unit>().tileY, -1), Quaternion.identity);
				newSpawn.gameObject.tag = "Monster1";
				newSpawn.GetComponent<Monsters>().tileX = map.selectedUnit.GetComponent<Unit>().tileX;
				newSpawn.GetComponent<Monsters>().tileY = map.selectedUnit.GetComponent<Unit>().tileY;
			newSpawn.GetComponent<Monsters>().level = map.selectedUnit.GetComponent<Unit>().carryingLevel;
				newSpawn.GetComponent<Monsters> ().UpdateSprite ();
				/*
				map.newSpawn = (GameObject)Instantiate(map.selectedMonster.GetComponent<Monsters>().Monster, new Vector3(tileX, tileY, -1), Quaternion.identity);
				map.newSpawn.gameObject.tag = "Monster1";
				map.newSpawn.GetComponent<Monsters>().tileX = tileX;
				map.newSpawn.GetComponent<Monsters>().tileY = tileY;
				*/
				map.monsterList.Add(newSpawn);
			map.occupiedCount++;
			map.occupationArray [map.selectedUnit.GetComponent<Unit>().tileX, map.selectedUnit.GetComponent<Unit>().tileY] = true;

			map.connect(newSpawn);

			map.fuse (newSpawn);
				print("right and carried and dropped");

			}
		}
	}
