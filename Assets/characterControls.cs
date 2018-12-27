using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class characterControls : MonoBehaviour {

	//public object HBar;
	public GameObject HBar;
	//float BarLength;
	public int phase;
	public Vector3 initialHBar;
	public float startPos;
	public int numHits;

	public GameObject meleeBox;
	public GameObject tempBox;
	public GameObject bullet;
	public GameObject tempShot;
	//public Shader heroS;
	public Material heroM;
	//public GameObject hero;
	//public Renderer heroR;

	public bool hit;
	public float hitTime = 1f;
	public float timer;

	public ParticleSystem PS;

	public controller c;

	Vector3 tempVector;

	// Use this for initialization
	void Start () {

		//HBar = GameObject.Find ("HBarGreen");
		//HBar = GameObject.FindWithTag ("HB");
		//HBar = transform.Find ("HBarGreen");
		//HBar = GameObject.Find ("/Canvas/HBarGreen");
		HBar = GameObject.FindGameObjectWithTag ("HB");
		
		//GameObject HBar = new Game
		//if (HBar != null) {		
			//BarLength = HBar.getWidth ();		//keep actual width/height 100/100 and only change scale so I don't have
		//}											//to deal with doing this bit.

		c = GameObject.Find ("Main Camera").GetComponent<controller>();
		PS = GameObject.Find("UITentacle").GetComponentInChildren<ParticleSystem>();

		if (phase == null) {
			phase = 1;
		}

		//initialHBar = new Vector3 (HBar.size ().x, HBar.size ().y);
		initialHBar = new Vector3 (HBar.GetComponent<RectTransform> ().sizeDelta.x, HBar.GetComponent<RectTransform>().sizeDelta.y);
		startPos = initialHBar.x;
		numHits = 10;

		if (PS != null) {
			PS.Play();
			PS.Pause();
		}

		//heroM = new Material ();
		//heroM.shader = Shader.Find ("EthanGrey");
		//heroS = Shader.Find ("EthanGrey");

		//Renderer heroR = hero.GetComponent<Renderer> ();
		//heroR = GetComponentInChildren<Renderer>();

		hit = false;
		timer = 0f;


	}
	
	// Update is called once per frame
	void Update () {

		//if (meleeBox.activeSelf == true)
	//		meleeBox.SetActive (false);

		if (Input.GetKeyDown (KeyCode.P))
			Damage ();

		if (Input.GetKeyDown (KeyCode.Z)) {
			//meleeBox.SetActive(true);		
			//////////////if not playing the attack animation. else play part 2?
			tempBox = Instantiate(meleeBox, meleeBox.transform.position, meleeBox.transform.rotation, this.transform);
			tempBox.SetActive(true);
			//tempBox.tag = "Weapon2";
			Destroy(tempBox, 1);
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			tempShot = Instantiate (bullet, bullet.transform.position, bullet.transform.rotation, this.transform);
			tempShot.SetActive(true);
			//tempShot.transform = tempShot.transform.forward;
			tempShot.GetComponent<Rigidbody>().velocity = tempShot.transform.forward*4;
			tempShot.transform.parent = null;
			Destroy (tempShot, 5);
		}

		if (Input.GetKeyDown (KeyCode.B)) {
			//heroM.SetColor ("EthanGrey", Color.red);
			//heroR.material.shader = Shader.Find ("EthanGrey");
			//Debug.Log (heroR.material.color);
			//heroR.material.SetColor("EthanGrey", Color.red);
			//heroR.material.color = Color.grey;
			//Debug.Log (heroR.material.color);
			//Debug.Log ("color change");
		}
		if (Input.GetKeyDown (KeyCode.N)){
			//heroR.material.color = Color.blue;
		}
		if (Input.GetKeyDown (KeyCode.M)){
			//heroR.material.color = Color.red;
		}

		if (hit == true) {
			timer += Time.deltaTime;
			if(timer >= hitTime && phase != 3){
				//Debug.Log ("change color");
				PS.Pause();
				//heroR.material.color = Color.grey;
				hit = false;
				timer = 0f;
			}
		}



	}

	void OnCollisionEnter (Collision col){

		//Debug.Log ("collision");

		if(col.collider.tag == "EBullet"){
			//HBar
			Debug.Log("hit by bullet");
			//Destroy(col.gameObject);
			col.gameObject.SetActive(false);
			Damage ();
		}

		if(col.collider.tag == "EWeapon"){
			//HBar
			Debug.Log("hit by melee");
			Damage ();
		}

		//if(col.transform.gameObject.tag == "meleeEnemy"){
			//HBar
		//	Debug.Log("hit by enemy?");
			//Damage ();
		//}

		//if (col.collider.tag == "EWeapon") {
		//	Debug.Log ("this one");
		//}

	}

	void Damage(){	//input damage type based on what hit it. Also replace HBar.getComponent<RectTransform>().sizeDelta with a variable or something.
		//Also change it to pass in a percentage of the health bar to decrement.
		//Maybe add a buffer on the health bar so it is always a little sliver before it moves to the next, instead of evenly dividing it up
		//HBar.gameObject.transform.x = HBar.gameObject.transform.x - 10;
		//HBar.transform.x = HBar.transform.x - 10;
		if (phase <= 3) {

			hit = true;
			//heroR.material.color = Color.red;

			PS.Play ();

			if (HBar.GetComponent<RectTransform> ().sizeDelta.x <= (initialHBar.x/numHits)+1) {
				//Debug.Log ("1");
				if (phase <= 3) { 
					//HBar.transform.Translate (100, 0, 0);
					HBar.GetComponent<RectTransform>().sizeDelta = new Vector3 (initialHBar.x, initialHBar.y);
					//HBar.transform.Translate (HBar.GetComponent<RectTransform>().sizeDelta.x + (initialHBar.x - ((initialHBar.x/numHits)/2)), 0, 0);
					HBar.transform.Translate ((((initialHBar.x/numHits)*(numHits-1))/2), 0, 0);		//this won't work when using different amounts of damage
					/////////////////////////
					/// screen space-camera
					//GameObject temp = GameObject.Find("HBarBackground");
					//HBar.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, temp.transform.position.z);
					/////////////////////////
					//HBar.GetComponent<RectTransform> ().sizeDelta = new Vector3 (100, 100);
					//HBar.GetComponent<RectTransform>().sizeDelta = new Vector3 (initialHBar.x, initialHBar.y);
				}
				if(phase <= 3){
					phase++;
					if (phase == 2){
						//heroR.material.color = Color.green;
						c.EnterPhase2();
					}
					else if (phase == 3){
						//heroR.material.color = Color.black;
						c.EnterPhase3();
					}
				}
			} 
			else {
				//Debug.Log ("2");
				//HBar.transform.Translate (-2.5f, 0f, 0f);
				//HBar.transform.Translate (((initialHBar.x/numHits)/-21), 0f, 0f);
				//HBar.transform.position.x += 36;
				//tempVector = HBar.transform.position;
				//tempVector.x += -2;							//1 = 25ish
				//HBar.transform.position = tempVector;
				//HBar.transform.Translate(Vector3.left * 1);
				//HBar.transform.position.Set(HBar.transform.position.x - ((initialHBar.x/numHits)*20), HBar.transform.position.y, HBar.transform.position.z);
				//Debug.Log ("shift-x:" + (initialHBar.x/numHits)/-2);
				//HBar.transform.Width () = HBar.transform.Width () - 10;
				//HBar.GetComponent<RectTransform> ().sizeDelta = new Vector3 (HBar.GetComponent<RectTransform> ().sizeDelta.x - 5, 100);
				//////////////////////////////////
				/// screen space-camera
				//HBar.transform.Translate(Vector3.left * ((initialHBar.x/numHits)/2)/25);
				//HBar.GetComponent<RectTransform>().sizeDelta = new Vector3 (HBar.GetComponent<RectTransform>().sizeDelta.x - ((initialHBar.x/numHits)), initialHBar.y);
				//////////////////////////////////
				/// screen space-overlay
				HBar.transform.Translate(((initialHBar.x/numHits)/-2), 0f, 0f);
				HBar.GetComponent<RectTransform>().sizeDelta = new Vector3 (HBar.GetComponent<RectTransform>().sizeDelta.x - ((initialHBar.x/numHits)), initialHBar.y);
				//////////////////////////////////
				//if (phase >= 3){
				//	Debug.Log ("Dead");
				//}
			}
			if (phase > 3){
				//Debug.Log ("3");
				//HBar.transform.Translate (-2.5f, 0f, 0f);
				///HBar.transform.Translate (((initialHBar.x/numHits)/-2), 0f, 0f);
				//HBar.GetComponent<RectTransform> ().sizeDelta = new Vector3 (HBar.GetComponent<RectTransform> ().sizeDelta.x - 5, 100);
				///HBar.GetComponent<RectTransform>().sizeDelta = new Vector3 (HBar.GetComponent<RectTransform>().sizeDelta.x - ((initialHBar.x/numHits)), initialHBar.y);
				Destroy (HBar, 0);
				Debug.Log ("Dead");
			}
		} 

	}

	void newPhase(int p){
		phase = p;
	}
	
}
