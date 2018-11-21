using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {
	public static PauseManager pm; 

	public GameObject PauseCanvas;

	public KeyCode pausekey = KeyCode.Escape;

	public bool isPaused = false;

	Inventory inventory;
    
	void Start () {  
		if (pm != null)
			Destroy (this.gameObject);

		pm = this;
		inventory = Inventory.instance;
		isPaused = false;  
	}

	void Update () 
	{
		if (Input.GetKeyDown (pausekey)) {
			isPaused = !isPaused;
		 
			if (isPaused == true) {     
				if (inventory.isShown) {
					inventory.HideInventory ();
					inventory.isShown = true;
				}
				CursorManager.ShowCursorLook (); 
 
			} else if (isPaused == false) {  
				CursorManager.HideCursorLook ();
				  
			}  
		}
			if (isPaused == true) {    
				Time.timeScale = 0.0f;
				PauseCanvas.SetActive (true);

			} 
			else if (isPaused == false) 
			{   
				Time.timeScale = 1.0f;
				PauseCanvas.SetActive (false);
			}  
		
 
	}

	public void Resume () {
		isPaused = false;
		CursorManager.HideCursorLook ();
		if (inventory.isShown)
			inventory.ShowInventory ();
	}

	public void Exit() {
		Application.Quit ();
	}
 

}
