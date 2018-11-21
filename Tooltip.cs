using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

	private Item item;
	private string data; 
	public GameObject tooltip;
 
	void Update(){ 
		if (Inventory.instance.isShown) {
			tooltip.transform.position = Input.mousePosition;
		}
	}

	public void Activate () {
		tooltip.SetActive (true);
	}


	public void DeActivate () {
		tooltip.SetActive (false);
	}

	public void ConstructItemDescription(string name){
		data = name;
		tooltip.transform.GetChild (0).GetComponent<Text> ().text = data;
		Activate ();
	}



























}
