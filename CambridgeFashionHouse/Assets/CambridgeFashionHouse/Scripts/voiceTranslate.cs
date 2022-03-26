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

    void Start()
    {
        actions.Add("i like your outfit", Positive);
        actions.Add("i like your style", Positive);
        //actions.Add("okay", Okay);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        print(keywordRecognizer.Keywords);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        rend2 = GetComponent<MeshRenderer>();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Positive()
    {
        // transform.Translate(0, 0, -1);
        BubbleGift.GetComponent<Gift>().RotateNewTarget(this.gameObject);
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