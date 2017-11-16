using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour {

	public int tileX;
	public int tileY;

	public bool carrying;
	public int carryingLevel;

	public AudioClip moveSound;
	public AudioSource source;

	public Sprite unitSpriteFront;
	public Sprite unitSpriteBack;
	public Sprite unitSpriteLeft;
	public Sprite unitSpriteRight;

	public GameObject leftCloud;
	public GameObject rightCloud;
	public GameObject midCloud;
	public GameObject backCloud;

	public TimeMap map;

	void Awake() {
		source = GetComponent<AudioSource> ();
	}

	public void PlaySound() {
		source.PlayOneShot (moveSound, 1);
	}

	IEnumerator ShowLeftCloud() {
		leftCloud.SetActive(true);
		yield return new WaitForSeconds (0.5f);
		leftCloud.SetActive(false);
	}

	IEnumerator ShowRightCloud() {
		rightCloud.SetActive(true);
		yield return new WaitForSeconds (0.5f);
		rightCloud.SetActive(false);
	}

	IEnumerator ShowMidCloud() {
		midCloud.SetActive(true);
		yield return new WaitForSeconds (0.5f);
		midCloud.SetActive(false);
	}

	IEnumerator ShowBackCloud() {
		backCloud.SetActive(true);
		yield return new WaitForSeconds (0.5f);
		backCloud.SetActive(false);
	}

	void Update() {
		var child = this.gameObject.transform.GetChild (0);
		if(map.hasStarted && !map.gameOver) {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteLeft;
                map.MoveSelectedUnitTo(Convert.ToInt32(Math.Max(0, transform.position.x - 1)), Convert.ToInt32(transform.position.y));
				source.PlayOneShot (moveSound, 1);
				StartCoroutine ("ShowLeftCloud");
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteRight;
                map.MoveSelectedUnitTo(Convert.ToInt32(Mathf.Min(5, transform.position.x + 1)), Convert.ToInt32(transform.position.y));
				source.PlayOneShot (moveSound, 1);
			StartCoroutine ("ShowRightCloud");
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteBack;
                map.MoveSelectedUnitTo(Convert.ToInt32(transform.position.x), Convert.ToInt32(Mathf.Min(5,transform.position.y + 1)));
				source.PlayOneShot (moveSound, 1);
				StartCoroutine ("ShowBackCloud");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteFront;
                map.MoveSelectedUnitTo(Convert.ToInt32(transform.position.x), Convert.ToInt32(Mathf.Max(0, transform.position.y - 1)));
				source.PlayOneShot (moveSound, 1);
				StartCoroutine ("ShowMidCloud");
            }
        }
    }
}
