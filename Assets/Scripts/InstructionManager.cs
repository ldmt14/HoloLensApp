using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class InstructionManager : MonoBehaviour {

    private InstructionObject lastFocusedObject;

    public InstructionObject LastFocusedObject { get; private set; }

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {
        keywords.Add("Next", () =>
        {
            NextStep();
        });

        keywords.Add("Back", () =>
        {
            PreviousStep();
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update () {
        var mainCamera = Camera.main.transform;
        RaycastHit hitInfo;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hitInfo, 30.0f))
        {
            var instructionObject = hitInfo.collider.gameObject.GetComponent<InstructionObject>();
            if (instructionObject != null) {
                lastFocusedObject = instructionObject;
            }
        }
	}

    void NextStep()
    {
        if (lastFocusedObject != null)
        {
            lastFocusedObject.NextStep();
        }
    }

    void PreviousStep()
    {
        if (lastFocusedObject != null)
        {
            lastFocusedObject.PreviousStep();
        }
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
