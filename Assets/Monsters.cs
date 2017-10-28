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
		print ("destroyed:" + tileX + tileY);
		foreach (GameObject monster in neighbours) {
			print ("going to detach " + tileX + tileY + " and " + monster.GetComponent<Monsters> ().tileX + monster.GetComponent<Monsters> ().tileY);
			monster.GetComponent<Monsters>().detach (gameObject);
		}

		Destroy (gameObject);
		print ("destroyed!!!:" + tileX + tileY);

	}

	public void detach(GameObject mon) {
		print ("Is neighbor removed?" + neighbours.Remove (mon));
	}
		
		
		
}
