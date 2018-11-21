using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayMaker;

public class EquipmentManager : MonoBehaviour {

	public static EquipmentManager instance;
	Inventory inventory;

	public delegate void OnEquipmentChanged (Equipment newItem, Equipment oldItem);
	public OnEquipmentChanged onEquipmentChanged; 
	public bool ObjectEquiped = false;
	// Use this for initialization
	void Awake () {
		instance = this;
	} 
	Equipment[] currentEquipment;
	GameObject[] currentEquipedGameobjects;
	 
	public Vector3 position;
	public Quaternion rotation;

	//References
	GameObject WeaponHolder;
	[HideInInspector]
	public GameObject InventoryHolder; 
	public Equipment currentequipedobj; 

	void Start() 
	{
		inventory = Inventory.instance;
		int numslots =	System.Enum.GetNames (typeof(EquipmentSlot)).Length;
		currentEquipment = new Equipment[numslots];
		currentEquipedGameobjects = new GameObject[numslots];
		WeaponHolder = GameObject.FindGameObjectWithTag ("WH");
		InventoryHolder = GameObject.FindGameObjectWithTag ("InventoryHolder");
	}

	public void  GetItem(Equipment newitem){
		currentequipedobj = newitem;
	}

	public void  Equip (GameObject go, PlayMakerFSM fsm)
	{
		int slotIndex = (int)currentequipedobj.equipmentSlot;
		Equipment oldItem = null;
		GameObject oo = null;
		if (currentEquipment [slotIndex] != null) { 
			oldItem = currentEquipment [slotIndex];
			oo = currentEquipedGameobjects [slotIndex];
			inventory.Add (oldItem, oo);
		} 

		inventory.Remove (currentequipedobj, go);
		inventory.HideInventory ();
		go.transform.SetParent(WeaponHolder.transform);
		go.GetComponent<OnPickUpHandler> ().OnPickedUp = true;
		go.GetComponent<OnPickUpHandler> ().CheckOnPickup();  
		fsm.enabled = true;
		go.SetActive (true);

		if (onEquipmentChanged != null) {
			onEquipmentChanged (currentequipedobj, oldItem);
		} 

		ObjectEquiped = true;
		currentEquipment [slotIndex] = currentequipedobj; 
		currentEquipedGameobjects [slotIndex] = go;
	} 
 


	public void Unequip(int slotIndex)
	{
		if (currentEquipment [slotIndex] != null) {
			if (currentEquipedGameobjects [slotIndex] != null) {

			Equipment olditem = currentEquipment [slotIndex]; 
		    GameObject toadd = currentEquipedGameobjects [slotIndex]; 
				toadd.GetComponent<PlayMakerFSM> ().enabled = false;
				toadd.GetComponent<OnPickUpHandler> ().OnPickedUp = true;
				toadd.GetComponent<OnPickUpHandler> ().CheckOnPickup();  
		    toadd.GetComponent<InteractableObject> ().Unequip ();
			currentEquipment [slotIndex] = null;

			if (onEquipmentChanged != null) {
				onEquipmentChanged (null, olditem);
			}
			currentequipedobj = null;
			ObjectEquiped = false;
		}
		}
	}

	public void UnequipAll()
	{
		for (int i = 0; i < currentEquipment.Length; i++) {
			Unequip (i);
		}
	}

	public bool WeaponEquiped(Equipment equipment){
		if (currentequipedobj == equipment) {
			
			return true;
		}else
			
			return false;
	}


	void Update()
	{
		if (Input.GetKey (KeyCode.U) && !inventory.isShown) {
			UnequipAll ();
		}
	}

























}
