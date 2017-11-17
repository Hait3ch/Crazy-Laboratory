using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour {
	public AudioSource hoverSound;

	public void LoadSceneOnClick (int index) {
    		SceneManager.LoadScene (index);
    	}

    public void DoQuit (int index) {
    		Application.Quit();
    	}

	public void PlayHoverSound () {
		hoverSound.Play();
	}
}
