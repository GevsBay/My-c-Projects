using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : MonoBehaviour {
  
	public int bulletPerMag = 30;
	public int currentBullets;
	public int bullentsleft = 200;
	public float fireRate = 0.1f;
	public float rangeFire = 100f;

	public enum ShootMode { Auto,Semi }
	public ShootMode shootingMode;
  
	public GameObject hitparticles;
	public GameObject bulletimpact;
	public float damage = 20f;
 
	private Vector3 originalPosition;
	public Vector3 aimPosition;

	float fireTimer;

	private bool isReloading;
	private bool ShootInput;
	private bool isAiming;
	public float aodspeed = 8f;

	public bool isPickedUp = false;

	EquipmentManager equip; 
	// Use this for initialization
	void Start () {
		currentBullets = bulletPerMag;   
		originalPosition = transform.localPosition; 
	}


	// Update is called once per frame
	void Update ()
	{ 
		if (this.transform.parent.parent.parent != null) {
			if (this.transform.parent.parent.parent.CompareTag ("WH"))
				isPickedUp = true;
		}else
				isPickedUp = false;
		
		
		if (!isPickedUp || Inventory.instance.isShown)
			return;
		
		switch (shootingMode) {
		case ShootMode.Auto: 
			ShootInput = Input.GetButton ("Fire1");
			break;
		case ShootMode.Semi:
			ShootInput = Input.GetButtonDown("Fire1");
			break;
		}

		if (ShootInput) {
			if (currentBullets > 0) {
				Fire ();
			} else
				if (bullentsleft > 0)
					Reload ();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			if(currentBullets < bulletPerMag && bullentsleft > 0)
				Reload ();
		} 
				if (fireTimer < fireRate)
					fireTimer += Time.deltaTime;
		AimDownSights ();
			}
		 

	public void AimDownSights() 
	{
		if (Input.GetButton ("Fire2") && !isReloading) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, aimPosition, Time.deltaTime * aodspeed);
			isAiming = true; 
		}
		else {
			transform.localPosition = Vector3.Lerp (transform.localPosition, originalPosition, Time.deltaTime * aodspeed);
			isAiming = false;
		}
	}

	public void Fire () 
	{
		

		if (fireTimer < fireRate || currentBullets <= 0 || isReloading)
			return;

		RaycastHit hit;

		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, rangeFire)) {

			GameObject hitparticleseffect = Instantiate (hitparticles, hit.point, Quaternion.FromToRotation (Vector3.up, hit.normal));
			GameObject Bullethole = Instantiate (bulletimpact, hit.point, Quaternion.FromToRotation (Vector3.forward, hit.normal));
				Bullethole.transform.SetParent (hit.transform);

			Destroy (hitparticleseffect, 1f);
			Destroy (Bullethole, 3f);

			if (hit.transform.GetComponent<HealthController> ()) 
			{
				hit.transform.GetComponent<HealthController> ().ApplyDamage (damage);
			}

		}
 

		currentBullets--;
		fireTimer = 0.0f;

	}

	public void Reload () {
		if (bullentsleft <= 0)	return;
	    int bulletsToLoad = bulletPerMag - currentBullets;
		int bulletsToDeduct = (bullentsleft >= bulletsToLoad) ? bulletsToLoad : bullentsleft;
	
	bullentsleft -= bulletsToDeduct;
	currentBullets += bulletsToDeduct;

	}

	private void DoReload () { 
		if (isReloading) return; 
	}
 
}
