using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    GameManager gameManager;
    UserCharacterController controls;
    public Transform menuPanel;
    public Transform mainPanel;

    Event keyEvent;

    Text buttonText;

    KeyCode newKey;

    bool waitingForKey;
    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        controls = GameObject.FindGameObjectWithTag("Player").GetComponent<UserCharacterController>();

        waitingForKey = false;

        for (int i = 0; i < menuPanel.childCount; i++)
        {
            if (menuPanel.GetChild(i).name == "ForwardKey")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.forward.ToString();

            else if (menuPanel.GetChild(i).name == "BackwardKey")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.backward.ToString();

            else if (menuPanel.GetChild(i).name == "LeftKey")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.left.ToString();

            else if (menuPanel.GetChild(i).name == "RightKey")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.right.ToString();

            else if (menuPanel.GetChild(i).name == "JumpKey")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.jump.ToString();

            else if (menuPanel.GetChild(i).name == "SprintKey")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.sprint.ToString();

            else if (menuPanel.GetChild(i).name == "CrouchKey")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.crouch.ToString();

            else if (menuPanel.GetChild(i).name == "Attack1Key")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.attack1.ToString();

            else if (menuPanel.GetChild(i).name == "Attack2Key")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.attack2.ToString();

            else if (menuPanel.GetChild(i).name == "Ability1Key")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.ability1.ToString();

            else if (menuPanel.GetChild(i).name == "Ability2Key")
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = gameManager.ability2.ToString();
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !mainPanel.gameObject.activeSelf)
        {
            Time.timeScale = 0;
            controls.enabled = false;
            mainPanel.gameObject.SetActive(true);
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && mainPanel.gameObject.activeSelf)
        {
            Time.timeScale = 1;
            controls.enabled = true;
            mainPanel.gameObject.SetActive(false);

        }
    }

    void OnGUI()
    {
        keyEvent = Event.current;

        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode; //Assigns newKey to the key user presses
            waitingForKey = false;
        }
    }

    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;
        yield return WaitForKey(); //Executes endlessly until user presses a key

        switch (keyName)
        {
            case "forward":
                gameManager.forward = newKey; //Set forward to new keycode
                buttonText.text = gameManager.forward.ToString(); //Set button text to new key
                PlayerPrefs.SetString("forwardKey", gameManager.forward.ToString()); //save new key to PlayerPrefs
                break;

            case "backward":
                gameManager.backward = newKey; //set backward to new keycode
                buttonText.text = gameManager.backward.ToString(); //set button text to new key
                PlayerPrefs.SetString("backwardKey", gameManager.backward.ToString()); //save new key to PlayerPrefs
                break;

            case "left":
                gameManager.left = newKey; //set left to new keycode
                buttonText.text = gameManager.left.ToString(); //set button text to new key
                PlayerPrefs.SetString("leftKey", gameManager.left.ToString()); //save new key to playerprefs
                break;

            case "right":
                gameManager.right = newKey; //set right to new keycode
                buttonText.text = gameManager.right.ToString(); //set button text to new key
                PlayerPrefs.SetString("rightKey", gameManager.right.ToString()); //save new key to playerprefs
                break;

            case "jump":
                gameManager.jump = newKey; //set jump to new keycode
                buttonText.text = gameManager.jump.ToString(); //set button text to new key
                PlayerPrefs.SetString("jumpKey", gameManager.jump.ToString()); //save new key to playerprefs
                break;

            case "sprint":
                gameManager.jump = newKey; //set jump to new keycode
                buttonText.text = gameManager.jump.ToString(); //set button text to new key
                PlayerPrefs.SetString("sprint", gameManager.jump.ToString()); //save new key to playerprefs
                break;

            case "crouch":
                gameManager.jump = newKey; //set jump to new keycode
                buttonText.text = gameManager.jump.ToString(); //set button text to new key
                PlayerPrefs.SetString("crouch", gameManager.jump.ToString()); //save new key to playerprefs
                break;

            case "attack1":
                gameManager.jump = newKey; //set jump to new keycode
                buttonText.text = gameManager.jump.ToString(); //set button text to new key
                PlayerPrefs.SetString("attack1", gameManager.jump.ToString()); //save new key to playerprefs
                break;

            case "attack2":
                gameManager.jump = newKey; //set jump to new keycode
                buttonText.text = gameManager.jump.ToString(); //set button text to new key
                PlayerPrefs.SetString("attack2", gameManager.jump.ToString()); //save new key to playerprefs
                break;

            case "ability1":
                gameManager.jump = newKey; //set jump to new keycode
                buttonText.text = gameManager.jump.ToString(); //set button text to new key
                PlayerPrefs.SetString("ability1", gameManager.jump.ToString()); //save new key to playerprefs
                break;

            case "ability2":
                gameManager.jump = newKey; //set jump to new keycode
                buttonText.text = gameManager.jump.ToString(); //set button text to new key
                PlayerPrefs.SetString("ability2", gameManager.jump.ToString()); //save new key to playerprefs
                break;
        }
        yield return null;
    }

    public void SendText(Text text)
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }
}
