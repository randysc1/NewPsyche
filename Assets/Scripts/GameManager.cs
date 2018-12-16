using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager GM;

    //Movement
    public KeyCode jump { get; set; }
    public KeyCode forward { get; set; }
    public KeyCode backward { get; set; }
    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode crouch { get; set; }
    public KeyCode sprint { get; set; }

    //Attacks and abilities
    public KeyCode attack1 { get; set; }
    public KeyCode attack2 { get; set; }
    public KeyCode ability1 { get; set; }
    public KeyCode ability2 { get; set; }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        if(GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else if(GM!=this)
        {
            Destroy(gameObject);
        }

        //Movement
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space"));
        forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forwardKey", "W"));
        backward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backwardKey", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("crouch", "C"));
        sprint = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("sprint", "LeftShift"));
        //Abilities
        attack1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("attack1", "Z"));
        attack2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("attack2", "X"));
        ability1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ability1", "1"));
        ability2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ability1", "2"));
    }
}
