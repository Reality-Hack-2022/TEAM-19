using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;


public class NewBehaviourScript : MonoBehaviour


{

    public GameObject[] Avatars;
    private int activeAvatarIndex = 0;


    private MLInput.Controller _controller;

    // Start is called before the first frame update
    void Start()
    {
        activeAvatarIndex = Avatars.Length-1;
        MLInput.OnControllerButtonDown += OnButtonDown;
        //MLInput.OnControllerButtonUp += OnButtonUp;
       // _controller = MLInput.GetController(MLInput.Hand.Left);
        check();
    }

    // Update is called once per frame

    void OnButtonDown(byte controllerId, MLInput.Controller.Button button)
    {
        Debug.Log("CHECK");
        if (button == MLInput.Controller.Button.Bumper)
        {
            check();
        }
    }

    void Update()
    {
        //controller button click
        //Get
        //click++, if i = 1, show
        //ifpressed
        //show and off setactive 

        //if (button == MLInput.Controller.Button.Bumper)
        //{
        //    // console.log("yes");
        //    check();
        //}
    }

   

    void check()
    {
        activeAvatarIndex = (activeAvatarIndex + 1) % Avatars.Length;
        for (int i = 0; i < Avatars.Length; i++)
        {
            if (i == activeAvatarIndex)
            {
                Avatars[i].SetActive(true);
               
            }
            else
            {
                Avatars[i].SetActive(false);
            }
          
        }
    }
 
}
