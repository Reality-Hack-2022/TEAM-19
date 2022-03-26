using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class voiceTranslate : MonoBehaviour
{

    public Renderer rend2;

    public KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> actions = new Dictionary<string, Action>();
    public GameObject BubbleGift;
    public GameObject Manager;

    void Start()
    {
        actions.Add("i like your outfit", Gift);
        actions.Add("i like your style", Gift);
        actions.Add("cool clothes", Gift);
        actions.Add("goodbye", Close);
        actions.Add("good bye", Close);
        actions.Add("bye now", Close);
        actions.Add("see you", Close);
        actions.Add("have a good one", Close);
        actions.Add("happy hacking", Close);
        //actions.Add("okay", Okay);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        print(keywordRecognizer.Keywords);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        rend2 = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            Gift();
        }
        if (Input.GetKeyUp("enter"))
        {
            Close();
        }
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Gift()
    {
        // transform.Translate(0, 0, -1);
        BubbleGift.GetComponent<Gift>().RotateNewTarget(this.gameObject);
    }

    private void Close()
    {
        // transform.Translate(0, 0, -1);
        Manager.GetComponent<NewBehaviourScript>().showFinal();
    }
    //private void changeAlpha(var color, var newAlpha)
    //{
    //    color.a = 0.0f;
    //    return color;
    //}

    //private void Okay()
    //{
    //    rend1.material.color = changeAlpha(staticRenderer.material.color, 0.0f);
    //}
}