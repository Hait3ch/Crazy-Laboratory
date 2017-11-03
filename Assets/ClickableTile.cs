using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TimeMap map;
	public Unit unit;

    // playing the game with mouse
    // move by clicking and drop by right-click
	void OnMouseUp() {
		map.MoveSelectedUnitTo(tileX, tileY);
	}

	void Update() {

        if ((Input.GetKeyDown("space") || Input.GetMouseButtonDown(1)) && map.selectedUnit.GetComponent<Unit>().carrying) {
            map.selectedUnit.GetComponent<Unit>().carrying = false;

            GameObject newSpawn	= (GameObject)Instantiate(map.selectedMonster.GetComponent<Monsters>().Monster, new Vector3(map.selectedUnit.GetComponent<Unit>().tileX, map.selectedUnit.GetComponent<Unit>().tileY, -1), Quaternion.identity);
            newSpawn.gameObject.tag = "Monster1";
            newSpawn.GetComponent<Monsters>().tileX = map.selectedUnit.GetComponent<Unit>().tileX;
            newSpawn.GetComponent<Monsters>().tileY = map.selectedUnit.GetComponent<Unit>().tileY;
            newSpawn.GetComponent<Monsters>().level = map.selectedUnit.GetComponent<Unit>().carryingLevel;
            newSpawn.GetComponent<Monsters> ().UpdateSprite ();

            map.monsterList.Add(newSpawn);
            map.occupiedCount++;
            map.occupationArray [map.selectedUnit.GetComponent<Unit>().tileX, map.selectedUnit.GetComponent<Unit>().tileY] = true;

            map.connect(newSpawn);

            map.fuse (newSpawn);

            // Check if game ends after dropping monster
            map.isGameEnd();
            print("right and carried and dropped");

        }
    }
}
