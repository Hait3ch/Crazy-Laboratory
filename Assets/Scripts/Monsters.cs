using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour {

	private Sprite[] sprites;
	public Sprite sprite1;
	public Sprite sprite2;
	public Sprite sprite3;
	public Sprite sprite4;
	public Sprite sprite5;
	public Sprite sprite6;
	public Sprite sprite7;
	public Sprite sprite8;

	public GameObject clearEffect;

	public int tileX;
	public int tileY;
	public int level;
	public HashSet<GameObject> neighbours; //The neighbouring monsters
	public bool markDestroy;

	public GameObject Monster;

	public TileType[] tileTypes;
	int[,] tiles;

	void Start() {
		

	}

	public void UpdateSprite()
	{
		if (sprites == null) {
			sprites = new Sprite[] {sprite1, sprite2, sprite3, sprite4, sprite5, sprite6, sprite7, sprite8};
		}
		// Load the sprites from a sprite sheet file (png). 
		// Note: The file specified must exist in a folder named Resources
		if (level > 0) {
		    if (level >= 9)
		        this.GetComponent<SpriteRenderer> ().sprite = sprites [0];
		    else
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


	//destroy this monster, and remove references to it on its neighbours
	public void destroy(bool isCarry) {
		foreach (GameObject monster in neighbours) {
			monster.GetComponent<Monsters>().detach (gameObject);
		}
		//Monster.transform.parent.gameObject.AddComponent (clearEffect);
		if (!isCarry) {
			GameObject copyClearEffect = Instantiate (clearEffect, gameObject.transform.position, new Quaternion (), Monster.transform.parent);
			Destroy (copyClearEffect, 2);
		}
		Destroy (gameObject);
	}

	public void detach(GameObject mon) {
	    neighbours.Remove (mon);
	}
		
		
		
}
