using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	[SerializeField] 
	private float Health;

 
	public void ApplyDamage(float damage) 
	{
		Health -= damage;
		if (Health <= 0) {
			Destroy (gameObject);
		}

	}
}
