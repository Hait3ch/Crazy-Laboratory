using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TimeMap map;
	public Unit unit;
    float timeLeftToPressAgain = 0.25f;

    // playing the game with mouse
    // move by clicking and drop by right-click
	/*
	void OnMouseUp() {
		map.MoveSelectedUnitTo(tileX, tileY);
	}
	*/

	void Update() {
        // there is 0.25 second between that player can PickUp again or Drop again

        if (!map.selectedUnit.GetComponent<Unit>().carrying) {
            timeLeftToPressAgain -= Time.deltaTime;
            timeLeftToPressAgain = Mathf.Max(timeLeftToPressAgain, -0.25f);
        }
        else {
            timeLeftToPressAgain += Time.deltaTime;
            timeLeftToPressAgain = Mathf.Min(timeLeftToPressAgain, 0.25f);
        }

        if (Input.GetKeyDown("space") /*( || Input.GetMouseButtonDown(1))*/) {
            // while carrying the time builds up for player to Drop and vice versa in PickUp
            if (map.selectedUnit.GetComponent<Unit>().carrying && timeLeftToPressAgain > 0) {

                map.selectedUnit.GetComponent<Unit>().carrying = false;
				map.selectedUnit.GetComponent<Unit> ().UpdateIdleSprite ();

                GameObject newSpawn	= (GameObject)Instantiate(map.selectedMonster.GetComponent<Monsters>().Monster, new Vector3(map.selectedUnit.GetComponent<Unit>().tileX, map.selectedUnit.GetComponent<Unit>().tileY, -1), Quaternion.identity);
                newSpawn.GetComponent<Monsters>().tileX = map.selectedUnit.GetComponent<Unit>().tileX;
                newSpawn.GetComponent<Monsters>().tileY = map.selectedUnit.GetComponent<Unit>().tileY;
                newSpawn.GetComponent<Monsters>().level = map.selectedUnit.GetComponent<Unit>().carryingLevel;
                newSpawn.GetComponent<Monsters> ().UpdateSprite ();

                map.monsterList.Add(newSpawn);
                map.occupiedCount++;
                map.occupationArray [map.selectedUnit.GetComponent<Unit>().tileX, map.selectedUnit.GetComponent<Unit>().tileY] = true;

                map.connect(newSpawn);
                map.fuse (newSpawn);

                map.selectedUnit.GetComponent<Unit>().carryingLevel = 0;
                timeLeftToPressAgain = 0.25f;
            }

            // pick up monster while on top of it
            else if (!map.selectedUnit.GetComponent<Unit>().carrying && timeLeftToPressAgain < 0) {
                for(int i = 0; i < map.monsterList.Count; i++) {
                    if (map.monsterList[i].GetComponent<Monsters>().tileX == map.selectedUnit.GetComponent<Unit>().tileX &&
                        map.monsterList[i].GetComponent<Monsters>().tileY == map.selectedUnit.GetComponent<Unit>().tileY &&
                        map.occupationArray [map.monsterList[i].GetComponent<Monsters>().tileX, map.monsterList[i].GetComponent<Monsters>().tileY] == true) {

                            map.selectedUnit.GetComponent<Unit>().carrying = true;
                            map.occupiedCount--;
                            map.occupationArray [map.monsterList[i].GetComponent<Monsters>().tileX, map.monsterList[i].GetComponent<Monsters>().tileY] = false;
                            map.CarryMonster(map.monsterList[i]);
                            timeLeftToPressAgain = -0.25f;
                    }
                }
            }
        }
    }
}
