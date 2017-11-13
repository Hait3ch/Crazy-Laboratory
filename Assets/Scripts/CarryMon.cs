using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarryMon : MonoBehaviour {

    public TimeMap map;
    public Unit unit;

	public Sprite sprite1;
	public Sprite sprite2;
	public Sprite sprite3;
	public Sprite sprite4;
	public Sprite sprite5;
	public Sprite sprite6;
	public Sprite sprite7;
	public Sprite sprite8;
    void Start() //Lets start by getting a reference to our image component.
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null; //Our image component is the one attached to this gameObject.
    }

    public void changeCarrying() //method to set our first image
    {
       switch (map.selectedUnit.GetComponent<Unit>().carryingLevel)
             {
                case 0:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                    break;
                case 1:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
                    break;
                case 2:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
                    break;
                case 3:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite3;
                    break;
                case 4:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite4;
                    break;
                case 5:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite5;
                    break;
                case 6:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite6;
                    break;
                case 7:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite7;
                    break;
                case 8:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite8;
                    break;
                default:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                    break;
             }


    }
	// Update is called once per frame
	void Update () {
        changeCarrying();
	}
}
