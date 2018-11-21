using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPickUpHandler : MonoBehaviour {

	public bool OnPickedUp = false;
	public Rigidbody rb;  
	public Collider col;
	public ItemPickup itemPickUp;

	public void CheckOnPickup () {
		if (OnPickedUp) {
			rb.isKinematic = true;
			col.enabled = false;
			itemPickUp.enabled = false;
		} else {
			rb.isKinematic = false;
			col.enabled = true;
			itemPickUp.enabled = true;
		}	
	}
}
