using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class voiceReco : MonoBehaviour
{

    public Renderer rend;

    public KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> actions = new Dictionary<string, Action>();

    void Start()
    {
        actions.Add("forward", Forward);
        actions.Add("top", Up);
        actions.Add("down", Down);
        actions.Add("back", Back);
        actions.Add("rotate", Rotate);
        actions.Add("green", Green);
        actions.Add("red", Red);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        print(keywordRecognizer.Keywords);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        rend = GetComponent<MeshRenderer>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            rend.material.color = Color.red;
        }
    }

    

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Forward()
    {
        transform.Translate(1, 0, 0);
    }

    private void Green()
    {
        rend.material.color = Color.green;
    }

    private void Red()
    {
        rend.material.color = Color.red;
    }

    private void Rotate()
    {
        transform.Rotate(0, 45, 0);
    }

    private void Up()
    {
        transform.Translate(0, 1, 0);
    }

    private void Down()
    {
        transform.Translate(0, -1, 0);
    }

    private void Back()
    {
        transform.Translate(-1, 0, 0);
    }

}