using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour {

	private Sprite[] sprites;
	public Sprite sprite1;
	public Sprite sprite2;
	public Sprite sprite3;

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
		

	}

	public void UpdateSprite()
	{
		if (sprites == null) {
			sprites = new Sprite[] {sprite1, sprite2, sprite3};
		}
		// Load the sprites from a sprite sheet file (png). 
		// Note: The file specified must exist in a folder named Resources
		if (level > 0) {
			this.GetComponent<SpriteRenderer> ().sprite = sprites [level - 1];
			float myScale = 0.4f;
			GetComponent<SpriteRenderer>().transform.localScale = new Vector3(myScale, myScale, myScale);
		}
	}
		
	/*
	private void LoadSpriteSheet()
	{
		// Load the sprites from a sprite sheet file (png). 
		// Note: The file specified must exist in a folder named Resources
		var sprites = Resources.LoadAll<Sprite>(this.SpriteSheetName);
		this.spriteSheet = sprites.ToDictionary(x => x.name, x => x);

		// Remember the name of the sprite sheet in case it is changed later
		this.LoadedSpriteSheetName = this.SpriteSheetName;
	}
	*/

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
