using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;


public class MenuController : MonoBehaviour {

    public Sprite StartGame;
    public Sprite EndGame;
    public Sprite Credits;
    public Sprite Controls;

    public float waitTime = 0.5f;
    private float currentWaitTime = 0;

    private int position = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentWaitTime -= Time.fixedDeltaTime;

        userInput();


        updatePosition();
        if (currentWaitTime < 0.0f)
        {
            setImage();        }
        
    }

    private void updatePosition()
    {
        var isLeft = CrossPlatformInputManager.GetAxis("Horizontal") > 0;
        var isRight = CrossPlatformInputManager.GetAxis("Horizontal") < 0;
        if (isLeft)
        {
            position++;

        }
        if (isRight)
        {
            position--;
        }
        if (position < 0)
        {
            position = 3;
        }
        if (position > 3)
        {
            position = 0;
        }
    }

    private void userInput()
    {

        if (Input.GetButton("Jump") || CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            switch (position)
            {
                case 0:
                    LoadScene();
                    break;
                case 1:
                    Application.Quit();
                    break;
                case 2:
                    LoadCredits();
                    break;
                case 3:

                    break;
                default:
                    LoadScene();
                    break;
            }
        }
    }

    private void setImage()
    {
        var images = gameObject.GetComponentsInChildren<Image>();

        foreach (Image image in images)
        {

            Sprite currentImage;

            switch (position)
            {
                case 0:
                    currentImage = StartGame;
                    break;
                case 1:
                    currentImage = EndGame;
                    break;
                case 2:
                    currentImage = Credits;
                    break;
                case 3:
                    currentImage = Controls;
                    break;
                default:
                    currentImage = StartGame;
                    break;
            }
            // var currentImage = isAlarm ? alarm : undetected;
            image.sprite = currentImage;
            currentWaitTime = waitTime;
        }
    }

    public void LoadScene()
    {
        Debug.Log("Load Scene 1");
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }
}
