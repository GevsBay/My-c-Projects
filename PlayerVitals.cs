using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerVitals : MonoBehaviour 
{
	public static PlayerVitals intsance;

	[Header("UI")]
	public Gradient gradient;

	[Header("Health")]
	public Slider HealthSlider;

	public Image HealthSliderFill;
	public int maxHealth;
	public int maxHealthfallRate;
	public float currentHealth;
	public float HealthRegenRate;
	public Text healthText;

	[Header("Thirst")]
	public Slider ThirstSlider;

	public Image ThirstSliderFill;
	public int maxThirst;
	public int maxThirstfallRate;
	public float currentThirst;
	public Text thirstText;

	[Header("Hunger")]
	public Slider HungerSlider;

	public Image HungerSliderFill;
	public int maxHunger;
	public int maxHungerfallRate;
	public float currentHunger;
	public Text hungerText;
	public Text CaloriesText;

	[Header("Stamina")]
	public Slider StaminaSlider;

	public CanvasGroup StaminasCanvas; 

	public int normmaxStamina;
	private int StaminafallRate;
	public int StaminafallrateMult;
	private int staminaRegenRate;
	public int staminregenRateMult;

	[Header("Fatigue")]
	public Slider FatigueSlider;
	public Image fatigueSliderFill;
	public float maxFatigue;
	public float fatigueFallRate;
	public bool fatStage0 = true;
	public bool fatStage1 = true;
	public bool fatStage2 = true;
	public bool fatStage3 = true;
	public bool fatStageNormal = false;

	public bool tired = false;
	public bool rested = true;
	bool done = true;

	private RigidbodyFirstPersonController PlayerController;
	Inventory inventory;
	FoodManager foodManager;

	void Awake(){ 
		intsance = this;
	}

	void Start() 
	{
		inventory = Inventory.instance;
		FatigueSlider.maxValue = maxFatigue;
		FatigueSlider.value = maxFatigue;
		foodManager = FoodManager.instance;
		HealthSlider.maxValue = maxHealth;
		currentHealth = maxHealth;
		ThirstSlider.maxValue = maxThirst;
		currentThirst = maxThirst;
		HungerSlider.maxValue = maxHunger;
		currentHunger = maxHunger;
		StaminaSlider.maxValue = normmaxStamina; 

		StaminafallRate = 1;
		staminaRegenRate = 1; 

		PlayerController = GetComponent<RigidbodyFirstPersonController> (); 
	} 

	void Update () 
	{
		HealthSlider.value = currentHealth;
		ThirstSlider.value = currentThirst;
		HungerSlider.value = currentHunger; 

		float healthGradientValue = HealthSlider.value / HealthSlider.maxValue;
		float hungerGradientValue = HungerSlider.value / HungerSlider.maxValue;
		float ThirstGradientValue = ThirstSlider.value / ThirstSlider.maxValue;
		float FatigueGradientValue = FatigueSlider.value / FatigueSlider.maxValue;

		HealthSliderFill.color = gradient.Evaluate (healthGradientValue);
		HungerSliderFill.color = gradient.Evaluate (hungerGradientValue);
		ThirstSliderFill.color = gradient.Evaluate (ThirstGradientValue);
		fatigueSliderFill.color = gradient.Evaluate (FatigueGradientValue);


			//Health Sysyem
		if (currentHunger <= 0 && currentThirst <= 0) {
			currentHealth -= Time.deltaTime / maxHealthfallRate * 2;
		} 
		else if(currentHunger <= 0 || currentThirst <= 0)
		{
			currentHealth -= Time.deltaTime / maxHealthfallRate;
		}
		 if (currentHealth <= 0) {
			PLayerDies ();
		}
			
		if (currentHunger > 0 && currentThirst > 0) {
			HealthRegenRate = 0.2f;
			if (currentHealth < 100)
				currentHealth += Time.deltaTime * HealthRegenRate;
		}
		if(currentHealth < 100)
		healthText.text = Mathf.Round (currentHealth).ToString () + "%";

		//Hunger System
		if (currentHunger > 0) {
			if(!foodManager.eating)
			currentHunger -= Time.deltaTime * maxHungerfallRate;
		} 
		else if (currentHunger <= 0) {
			currentHunger = 0;
		}

		if (currentHunger >=  maxHunger) { 
			currentHunger = maxHunger;
		} 

	 
		if (currentHunger < 2200 && currentHunger > 1800)
			hungerText.text = "little hungry";
		else if (currentHunger < 1800 && currentHunger > 1200)
			hungerText.text = "hungry";
		else if (currentHunger < 1200 && currentHunger > 600)
			hungerText.text = "very hungry";
		else if (currentHunger <= 600)
			hungerText.text = "starving";
		if (currentHunger == 0 || currentHunger >= 2200)  
			hungerText.text = null;
	 
		
		if(currentHunger != 0)
		CaloriesText.text = "Calories: " + Mathf.Round(currentHunger).ToString ();
		else
			CaloriesText.text = null;
 
		 //Thirst System
	 
	    if (currentThirst >  0) {
			if(!foodManager.eating)
			currentThirst -= Time.deltaTime * maxThirstfallRate;
		} 
		else if (currentThirst <= 0) {
			currentThirst = 0;
		}
	    if (currentThirst >=  maxThirst) { 
			currentThirst = maxThirst;
		}

		if (currentThirst < 800 && currentThirst > 600)
			thirstText.text = "thirsty";
		else if (currentThirst < 600 && currentThirst > 350)
			thirstText.text = "very thirsty";
		else if (currentThirst <= 350)
			thirstText.text = "dehydrated";
	    if (currentThirst == 0 || currentThirst >= 800)
			thirstText.text = null;

		//Stamina System
		bool Running = false; 
		bool hidedCanvas = false;

		if (FatigueSlider.value > 0) {
			if (PlayerController.Velocity.magnitude > 0 && Input.GetKey (KeyCode.LeftShift)) {
				StaminaSlider.value -= Time.deltaTime * StaminafallRate * StaminafallrateMult; 
				StaminasCanvas.alpha = Mathf.MoveTowards (StaminasCanvas.alpha, 1, 1.5f * Time.deltaTime);
				Running = true;  
				done = false;
			} else {  
				StaminaSlider.value += Time.deltaTime * staminaRegenRate * staminregenRateMult; 
				StaminasCanvas.alpha = Mathf.MoveTowards (StaminasCanvas.alpha, 0, 1.5f * Time.deltaTime);
				Running = false;
			}
			hidedCanvas = false;
		} else if (FatigueSlider.value <= 0 && !hidedCanvas) {
			StaminasCanvas.alpha = Mathf.MoveTowards (StaminasCanvas.alpha, 0, 1.5f * Time.deltaTime);
			hidedCanvas = true;
		}


		if (Running && !done) {
			fatigueFallRate = 1;
		}else  if(!Running && !done){
			fatigueFallRate = 0.5f;
			done = true;
		}

		if (StaminaSlider.value >= normmaxStamina) 
		{ 
			StaminaSlider.value = normmaxStamina;

		} else if (StaminaSlider.value <= 0) 
		{
			StaminaSlider.value = 0; 
			PlayerController.movementSettings.RunMultiplier = 1;   
			 
		}   
 
		if (StaminaSlider.value <= 16) { 
			maxHungerfallRate = 2;
			maxThirstfallRate = 4;
		} else {
			maxHungerfallRate = 1;
			maxThirstfallRate = 2;
		}

		//Fatigue System
		if(FatigueSlider.value > 80 && !fatStage0){
			fatStage0 = true;
		}
		else if (FatigueSlider.value > 60 && !fatStage1) {
	
			fatStage1 = true;
			StaminafallrateMult = 4;
		}else if (FatigueSlider.value > 40 && !fatStage2) {
			fatStage2 = true;
			staminaRegenRate = 2;
		}else if (FatigueSlider.value > 20 && !fatStage3) {
	
			fatStage3 = true;
		}
		 

		if (FatigueSlider.value < 79 && FatigueSlider.value > 78.9) {
			inventory.OnLevelChanged = true;
		}else if (FatigueSlider.value < 59 && FatigueSlider.value > 58.9) {
			inventory.OnLevelChanged = true;
		}else if (FatigueSlider.value < 39 && FatigueSlider.value > 38.9) {
			inventory.OnLevelChanged = true;
		}else if (FatigueSlider.value < 19 && FatigueSlider.value > 18.9) {
			inventory.OnLevelChanged = true;
		}

		if (inventory.invloadLevelNormal) {

			if (FatigueSlider.value > 80 && fatStage0 && !fatStageNormal) {
			 
				PlayerController.movementSettings.ForwardSpeed = 6f;

				PlayerController.movementSettings.BackwardSpeed = 4f;

				PlayerController.movementSettings.StrafeSpeed = 4f;
			 
				fatStageNormal = true;

			} else if (FatigueSlider.value <= 80 && fatStage0) {
				PlayerController.movementSettings.ForwardSpeed = 5.5f;

				PlayerController.movementSettings.BackwardSpeed = 3.5f;

				PlayerController.movementSettings.StrafeSpeed = 3.5f;
				fatStage0 = false;
				fatStageNormal = false;

			} else if (FatigueSlider.value <= 60 && fatStage1) { 
				PlayerController.movementSettings.ForwardSpeed = 5f;

				PlayerController.movementSettings.BackwardSpeed = 3f;

				PlayerController.movementSettings.StrafeSpeed = 3f;

				fatStage1 = false;
			} else if (FatigueSlider.value <= 40 && fatStage2) {
				StaminafallrateMult = 5; 
				PlayerController.movementSettings.ForwardSpeed = 4f;

				PlayerController.movementSettings.BackwardSpeed = 2.5f;

				PlayerController.movementSettings.StrafeSpeed = 2.5f;
				fatStage2 = false;
			} else if (FatigueSlider.value <= 20 && fatStage3) {
				StaminafallrateMult = 5;
				staminaRegenRate = 1; 
				PlayerController.movementSettings.ForwardSpeed = 3f;

				PlayerController.movementSettings.BackwardSpeed = 2f;

				PlayerController.movementSettings.StrafeSpeed = 2f;
				fatStage3 = false;
			}
		} else {
 
			if (FatigueSlider.value > 80 && inventory.OnLevelChanged && fatStage0) {
				if (inventory.invloadLevel1) {
					PlayerController.movementSettings.ForwardSpeed = 6f / PlayerController.movementSettings.InvloadLevel1Devider;

					PlayerController.movementSettings.BackwardSpeed = 4f / PlayerController.movementSettings.InvloadLevel1Devider;

					PlayerController.movementSettings.StrafeSpeed = 4f / PlayerController.movementSettings.InvloadLevel1Devider;

				} else if (inventory.invloadLevel2) {
					PlayerController.movementSettings.ForwardSpeed = 6f / PlayerController.movementSettings.InvloadLevel2Devider;

					PlayerController.movementSettings.BackwardSpeed = 4f / PlayerController.movementSettings.InvloadLevel2Devider;

					PlayerController.movementSettings.StrafeSpeed = 4f / PlayerController.movementSettings.InvloadLevel2Devider;

				} else if (inventory.invloadLevel3) {
					PlayerController.movementSettings.ForwardSpeed = 6f / PlayerController.movementSettings.InvloadLevel3Devider;

					PlayerController.movementSettings.BackwardSpeed = 4f / PlayerController.movementSettings.InvloadLevel3Devider;

					PlayerController.movementSettings.StrafeSpeed = 4f / PlayerController.movementSettings.InvloadLevel3Devider;

				}

				inventory.OnLevelChanged = false; 
			} else if (FatigueSlider.value <= 80 && inventory.OnLevelChanged && FatigueSlider.value > 60) {
				if (inventory.invloadLevel1) {
					PlayerController.movementSettings.ForwardSpeed = 5.5f / PlayerController.movementSettings.InvloadLevel1Devider;

					PlayerController.movementSettings.BackwardSpeed = 3.5f / PlayerController.movementSettings.InvloadLevel1Devider;

					PlayerController.movementSettings.StrafeSpeed = 3.5f / PlayerController.movementSettings.InvloadLevel1Devider;

				} else if (inventory.invloadLevel2) {
					PlayerController.movementSettings.ForwardSpeed = 5.5f / PlayerController.movementSettings.InvloadLevel2Devider;

					PlayerController.movementSettings.BackwardSpeed = 3.5f / PlayerController.movementSettings.InvloadLevel2Devider;

					PlayerController.movementSettings.StrafeSpeed = 3.5f / PlayerController.movementSettings.InvloadLevel2Devider;

				} else if (inventory.invloadLevel3) {
					PlayerController.movementSettings.ForwardSpeed = 5.5f / PlayerController.movementSettings.InvloadLevel3Devider;

					PlayerController.movementSettings.BackwardSpeed = 3.5f / PlayerController.movementSettings.InvloadLevel3Devider;

					PlayerController.movementSettings.StrafeSpeed = 3.5f / PlayerController.movementSettings.InvloadLevel3Devider;

				}

				inventory.OnLevelChanged = false; 
				fatStageNormal = false;
			 
			} else if (FatigueSlider.value <= 60 && inventory.OnLevelChanged  && FatigueSlider.value > 40) {
				if (inventory.invloadLevel1) {
					PlayerController.movementSettings.ForwardSpeed = 5f / PlayerController.movementSettings.InvloadLevel1Devider;

					PlayerController.movementSettings.BackwardSpeed = 3f / PlayerController.movementSettings.InvloadLevel1Devider;

					PlayerController.movementSettings.StrafeSpeed = 3f / PlayerController.movementSettings.InvloadLevel1Devider;

				} else if (inventory.invloadLevel2) {
					PlayerController.movementSettings.ForwardSpeed = 5f / PlayerController.movementSettings.InvloadLevel2Devider;

					PlayerController.movementSettings.BackwardSpeed = 3f / PlayerController.movementSettings.InvloadLevel2Devider;

					PlayerController.movementSettings.StrafeSpeed = 3f / PlayerController.movementSettings.InvloadLevel2Devider;

				} else if (inventory.invloadLevel3) {
					PlayerController.movementSettings.ForwardSpeed = 5f / PlayerController.movementSettings.InvloadLevel3Devider;

					PlayerController.movementSettings.BackwardSpeed = 3f / PlayerController.movementSettings.InvloadLevel3Devider;

					PlayerController.movementSettings.StrafeSpeed = 3f / PlayerController.movementSettings.InvloadLevel3Devider;

				} 
				inventory.OnLevelChanged = false; 
				fatStage0 = false;
				fatStage1 = true;
			} else if (FatigueSlider.value <= 40 && inventory.OnLevelChanged && FatigueSlider.value > 20) {
				if (inventory.invloadLevel1) {
					PlayerController.movementSettings.ForwardSpeed = 4f / PlayerController.movementSettings.InvloadLevel1Devider;

					PlayerController.movementSettings.BackwardSpeed = 2.5f / PlayerController.movementSettings.InvloadLevel1Devider;

					PlayerController.movementSettings.StrafeSpeed = 2.5f / PlayerController.movementSettings.InvloadLevel1Devider;

				} else if (inventory.invloadLevel2) {
					PlayerController.movementSettings.ForwardSpeed = 4f / PlayerController.movementSettings.InvloadLevel2Devider;

					PlayerController.movementSettings.BackwardSpeed = 2.5f / PlayerController.movementSettings.InvloadLevel2Devider;

					PlayerController.movementSettings.StrafeSpeed = 2.5f / PlayerController.movementSettings.InvloadLevel2Devider;

				} else if (inventory.invloadLevel3) {
					PlayerController.movementSettings.ForwardSpeed = 4f / PlayerController.movementSettings.InvloadLevel3Devider;

					PlayerController.movementSettings.BackwardSpeed = 2.5f / PlayerController.movementSettings.InvloadLevel3Devider;

					PlayerController.movementSettings.StrafeSpeed = 2.5f / PlayerController.movementSettings.InvloadLevel3Devider;

				}

				inventory.OnLevelChanged = false; 
				fatStage1 = false;
				fatStage2 = true; 
			} else if (FatigueSlider.value <= 20 && inventory.OnLevelChanged) {
				if (inventory.invloadLevel1) {
					PlayerController.movementSettings.ForwardSpeed = 3f / PlayerController.movementSettings.InvloadLevel1Devider;

					PlayerController.movementSettings.BackwardSpeed = 2f / PlayerController.movementSettings.InvloadLevel1Devider;

					PlayerController.movementSettings.StrafeSpeed = 2f / PlayerController.movementSettings.InvloadLevel1Devider;

				} else if (inventory.invloadLevel2) {
					PlayerController.movementSettings.ForwardSpeed = 3.5f / PlayerController.movementSettings.InvloadLevel2Devider;

					PlayerController.movementSettings.BackwardSpeed = 2f / PlayerController.movementSettings.InvloadLevel2Devider;

					PlayerController.movementSettings.StrafeSpeed = 2f / PlayerController.movementSettings.InvloadLevel2Devider;

				} else if (inventory.invloadLevel3) {
					PlayerController.movementSettings.ForwardSpeed = 4f / PlayerController.movementSettings.InvloadLevel3Devider;

					PlayerController.movementSettings.BackwardSpeed = 2f / PlayerController.movementSettings.InvloadLevel3Devider;

					PlayerController.movementSettings.StrafeSpeed = 2f / PlayerController.movementSettings.InvloadLevel3Devider;

				}

				inventory.OnLevelChanged = false; 
				fatStage2 = false;
				fatStage3 = true; 
			}
		}

		if (rested && FatigueSlider.value > 0)   {  
			tired = false;
		} 
		if (FatigueSlider.value > 0) {
			if (!SleepManager.instance.sleeping) {
				FatigueSlider.value -= Time.deltaTime * fatigueFallRate;
				if (!rested)
					rested = true;
			}
		} else if (FatigueSlider.value <= 0) {
			FatigueSlider.value = 0;
			currentHealth -= Time.deltaTime / maxHealthfallRate;

			if (!tired) {
				PlayerController.movementSettings.ForwardSpeed = PlayerController.movementSettings.forwardspeedTired;

				PlayerController.movementSettings.BackwardSpeed = PlayerController.movementSettings.backwardspeedTired;

				PlayerController.movementSettings.StrafeSpeed = PlayerController.movementSettings.starfespeedTired;
				tired = true;
				rested = false;
			} 
		}
		else if (FatigueSlider.value >= maxFatigue) 
		{
			FatigueSlider.value = maxFatigue;
		}
	}

	void PLayerDies() {
		Destroy (this.gameObject);
	}







 
















}
