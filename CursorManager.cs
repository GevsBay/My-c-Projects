using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CursorManager : MonoBehaviour {

 
	public static void HideCursorLook ()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		RigidbodyFirstPersonController fps = GameObject.FindGameObjectWithTag ("Player").GetComponent<RigidbodyFirstPersonController> ();
		fps.mouseLook.lockCursor = true;
	}
	


	public static void ShowCursorLook ()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		RigidbodyFirstPersonController fps = GameObject.FindGameObjectWithTag ("Player").GetComponent<RigidbodyFirstPersonController> ();
		fps.mouseLook.lockCursor = false;

	}












} 