using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCanvas : MonoBehaviour {

	public GameObject canvas;
	public GameObject anotherCanvas;
	public GameObject help1;
	public GameObject help2;
	public GameObject help3;

	public GameObject quit;
	public GameObject next1;
	public GameObject next2;
	public GameObject back2;
	public GameObject back3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ClickOpen() {
		anotherCanvas.SetActive (false);
		canvas.SetActive (true);
	}

	public void ClickQuit() {
		anotherCanvas.SetActive (true);
		canvas.SetActive (false);
	}

	public void ClickNext1() {
		help2.SetActive (true);
	}

	public void ClickNext2() {
		help3.SetActive (true);
	}

	public void ClickBack2() {
		help2.SetActive (false);
	}

	public void ClickBack3() {
		help3.SetActive (false);
	}
}
