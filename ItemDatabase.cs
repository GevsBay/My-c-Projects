using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {

	public static ItemDatabase instance;

	void Awake() {
		instance = this;
	}

	public GameObject[] GameObjects;
}
