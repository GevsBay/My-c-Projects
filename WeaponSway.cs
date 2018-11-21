using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

	public float amount;
	public float maxamount;
	public float smoothamount;

	private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		initialPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		float movementX = -Input.GetAxis ("Mouse X") * amount;
		float movementY = -Input.GetAxis ("Mouse Y") * amount;
		movementX = Mathf.Clamp (movementX, -maxamount, maxamount);
		movementY = Mathf.Clamp (movementY, -maxamount, maxamount);

		Vector3 finalpos = new Vector3 (movementX, movementY, 0);
		transform.localPosition = Vector3.Lerp (transform.localPosition, finalpos + initialPosition, Time.deltaTime * smoothamount);

	}







































}
