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

	public Sprite unitStopFront;
	public Sprite unitStopBack;
	public Sprite unitStopLeft;
	public Sprite unitStopRight;

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
		bool isMoved = false;
		if(map.hasStarted && !map.gameOver) {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteLeft;
				isMoved = (transform.position.x - 1 >= 0) && map.MoveSelectedUnitTo(Convert.ToInt32(Math.Max(0, transform.position.x - 1)), Convert.ToInt32(transform.position.y));
				if (isMoved) {
					source.PlayOneShot (moveSound, 1);
					StartCoroutine ("ShowLeftCloud");
				} else {
					child.GetComponent<SpriteRenderer> ().sprite = unitStopLeft;
				}
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteRight;
				isMoved = (transform.position.x + 1 <= 5) && map.MoveSelectedUnitTo(Convert.ToInt32(Mathf.Min(5, transform.position.x + 1)), Convert.ToInt32(transform.position.y));
				if (isMoved) {
					source.PlayOneShot (moveSound, 1);
					StartCoroutine ("ShowRightCloud");
				} else {
					child.GetComponent<SpriteRenderer> ().sprite = unitStopRight;
				}
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteBack;
				isMoved = (transform.position.y + 1 <= 5) && map.MoveSelectedUnitTo(Convert.ToInt32(transform.position.x), Convert.ToInt32(Mathf.Min(5,transform.position.y + 1)));
				if (isMoved) {
					source.PlayOneShot (moveSound, 1);
					StartCoroutine ("ShowBackCloud");
				} else {
					child.GetComponent<SpriteRenderer> ().sprite = unitStopBack;
				}
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                child.GetComponent<SpriteRenderer> ().sprite = unitSpriteFront;
				isMoved = (transform.position.y - 1 >= 0) && map.MoveSelectedUnitTo(Convert.ToInt32(transform.position.x), Convert.ToInt32(Mathf.Max(0, transform.position.y - 1)));
				if (isMoved) {
					source.PlayOneShot (moveSound, 1);
					StartCoroutine ("ShowMidCloud");
				} else {
					child.GetComponent<SpriteRenderer> ().sprite = unitStopFront;
				}
            }
        }
    }
}
