using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour {

	public int tileX;
	public int tileY;
	public int level;
	public HashSet<GameObject> neighbours; //The neighbouring monsters
	public bool markDestroy;


	public GameObject Monster;
	//public GameObject selectedUnit;

	public TileType[] tileTypes;
	int[,] tiles;

	void Start() {
		neighbours = new HashSet<GameObject>();

	}

	void OnMouseUp() {
		if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(0)) {
	 		Debug.Log("Click something");
      // do something
   }

	}

	//destroy this monster, and remove references to it on its neighbours
	public void destroy() {
		foreach (GameObject monster in neighbours) {
			monster.GetComponent<Monsters>().detach (gameObject);
		}
		Destroy (gameObject);
	}

	public void detach(GameObject mon) {
		neighbours.Remove (mon);
	}
		
		
		
}
