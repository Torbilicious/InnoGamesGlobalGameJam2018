//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using UnityStandardAssets.CrossPlatformInput;

//public class MainMenuController : MonoBehaviour {

//    Button btnStart;
//    Button btnEnd;
//    Button btnCredits;

//    // Use this for initialization
//    void Start () {
//        btnStart = GameObject.Find("Start Game").GetComponent<UnityEngine.UI.Button>();
//        btnEnd = GameObject.Find("End Game").GetComponent<UnityEngine.UI.Button>();
//        btnCredits = GameObject.Find("Credits").GetComponent<UnityEngine.UI.Button>();

//        btnStart.onClick.AddListener(LoadScene);
//    }
	
//	// Update is called once per frame
//	void Update () {

//        //Debug.Log(btnStart);
//        if(btnStart.gameObject.activeSelf)
//        {
//            btnEnd.gameObject.SetActive(false);
//            btnCredits.gameObject.SetActive(false);
//        }
       

//        if (Input.GetButton("Jump") || CrossPlatformInputManager.GetButtonDown("Jump"))
//        {
//            //Debug.Log("jump");
//            LoadScene();
//        }

//        if(Math.Abs(CrossPlatformInputManager.GetAxis("Horizontal")) > 0)
//        {
//            if(CrossPlatformInputManager.GetAxis("Horizontal") < 0 && btnStart.gameObject.activeSelf)
//            {
//                // Left
//                btnStart.gameObject.SetActive(false);
//                btnCredits.gameObject.SetActive(false);
//                btnEnd.gameObject.SetActive(true);              

//            }
//            if (CrossPlatformInputManager.GetAxis("Horizontal") > 0 && btnStart.gameObject.activeSelf)
//            {
//                btnStart.gameObject.SetActive(false);
//                btnCredits.gameObject.SetActive(true);
//                btnEnd.gameObject.SetActive(false);
//                // Right
//            }

//            if (CrossPlatformInputManager.GetAxis("Horizontal") > 0 && btnEnd.gameObject.activeSelf)
//            {
//                btnEnd.gameObject.SetActive(false);
//                btnStart.gameObject.SetActive(true);
//                btnCredits.gameObject.SetActive(false);
//                // Right
//            }

//            if (CrossPlatformInputManager.GetAxis("Horizontal") < 0 && btnCredits.gameObject.activeSelf)
//            {
//                btnCredits.gameObject.SetActive(false);
//                btnStart.gameObject.SetActive(true);
//                btnEnd.gameObject.SetActive(false);
//                // Right
//            }
//        }
//    }

//    public void LoadScene()
//    {
//        Debug.Log("Load Scene 1");
//        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
//    }

//}
