using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class voiceRecoColor : MonoBehaviour
{

    public Renderer rend1;

    public KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> actions = new Dictionary<string, Action>();

    void Start()
    {
        actions.Add("happy", Happy);
        actions.Add("sad", Sad);
        //actions.Add("okay", Okay);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        print(keywordRecognizer.Keywords);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        rend1 = GetComponent<MeshRenderer>();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Happy()
    {
        rend1.material.color = Color.yellow;
    }

    private void Sad()
    {
        rend1.material.color = Color.blue;
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