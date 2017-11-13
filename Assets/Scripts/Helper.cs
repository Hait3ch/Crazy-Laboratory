using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour {

	public Unit unit;
	public TimeMap map;

	public void MoveAndSpawn(int x, int y) {
        map.selectedUnit.GetComponent<Unit>().tileX = x;
        map.selectedUnit.GetComponent<Unit>().tileY = y;
        map.selectedUnit.transform.position = map.TileCoordToWorldCoord(x, y);
        map.counterIncrease();
        map.SpawnMon();
	}
}
