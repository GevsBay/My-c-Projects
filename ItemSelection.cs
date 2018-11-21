using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour {
 
	public List<GameObject> ItemsToCook = new List<GameObject>();
	public int itemSpot = 0;
	public Button rightbutton;
	public Button leftButton;
	CookManager cm;

	// Use this for initialization
	void Start () {
		cm = GetComponent<CookManager> ();
	}


	public void RightSelection () {
		if (itemSpot < ItemsToCook.Count - 1) { 
			itemSpot++;
			Select ();
		} 
	}
	public void LeftSelection () {
		if (itemSpot > 0) { 
			itemSpot--;
			Select ();
		} 
	}

	public void Select(){
		if (ItemsToCook.Count == 0)
			return;
		
		for (int i = 0; i < ItemsToCook.Count; i++) {
			ItemsToCook [i].SetActive (false);
		}
		ItemsToCook [itemSpot].SetActive (true);
		cm.cookobj = null;  
		cm.CheckeverycookButton ();
	}
 
	public void Enabled(){
		if (ItemsToCook.Count == 0)
			return;
		
		for (int i = 0; i < ItemsToCook.Count; i++) {
			ItemsToCook [i].SetActive (false);
		}
		if(ItemsToCook [itemSpot] != null)
		ItemsToCook [itemSpot].SetActive (true); 
	}

	public void Selectfirst(){ 
		if (ItemsToCook.Count == 0)
			return;
		  
			ItemsToCook [0].SetActive (true); 
	}
	// Update is called once per frame
	void Update () {
		if (ItemsToCook.Count == 0) {

			leftButton.interactable = false;

			rightbutton.interactable = false;

			return;
		}
		
		if (cm.uiisShown) {
			if (itemSpot == 0)
				leftButton.interactable = false;
			else
				leftButton.interactable = true;

			if(itemSpot == ItemsToCook.Count-1)
				rightbutton.interactable = false;
			else
				rightbutton.interactable = true;
			
		}
	}





























}
