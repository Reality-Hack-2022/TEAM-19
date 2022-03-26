using UnityEngine;
using UnityEngine.XR.MagicLeap;


public class NewBehaviourScript : MonoBehaviour


{

    public GameObject[] Avatars;
    public GameObject OtherUser;
    public GameObject OtherUser2;
    public GameObject BubbleGift;
    private int activeAvatarIndex = 0;


    private MLInput.Controller _controller;

    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        activeAvatarIndex = Avatars.Length-1;
        MLInput.OnControllerButtonDown += OnButtonDown;
        // MLInput.OnTriggerUp += OnButtonDown;
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
                if (counter == 4) return;
                Avatars[i].SetActive(true);
            }
            else
            {
                if (counter == 5) return;
                if (counter == 4) {
                    OtherUser.SetActive(true);
                    OtherUser2.SetActive(true);
                }
                Avatars[i].SetActive(false);
            }
        }
        counter++;
    }

    public void showFinal()
    {
        Avatars[4].SetActive(true);
        OtherUser.SetActive(false);
        OtherUser2.SetActive(false);
        BubbleGift.SetActive(false);
    }
}
