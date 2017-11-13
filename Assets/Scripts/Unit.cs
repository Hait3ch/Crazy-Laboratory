using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour {

	public int tileX;
	public int tileY;

	public bool carrying;
	public int carryingLevel;

	public Sprite unitSpriteFront;
	public Sprite unitSpriteBack;
	public Sprite unitSpriteLeft;
	public Sprite unitSpriteRight;

	public TimeMap map;

	void Update() {
		var child = this.gameObject.transform.GetChild (0);
		if(!map.gameOver) {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteLeft;
                map.MoveSelectedUnitTo(Convert.ToInt32(Math.Max(0, transform.position.x - 1)), Convert.ToInt32(transform.position.y));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteRight;
                map.MoveSelectedUnitTo(Convert.ToInt32(Mathf.Min(5, transform.position.x + 1)), Convert.ToInt32(transform.position.y));
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteBack;
                map.MoveSelectedUnitTo(Convert.ToInt32(transform.position.x), Convert.ToInt32(Mathf.Min(5,transform.position.y + 1)));
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteFront;
                map.MoveSelectedUnitTo(Convert.ToInt32(transform.position.x), Convert.ToInt32(Mathf.Max(0, transform.position.y - 1)));
            }
        }
    }
}
