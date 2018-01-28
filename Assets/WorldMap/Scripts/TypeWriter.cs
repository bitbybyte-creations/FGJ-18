using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour {

    public Text self_;
    public float textSpeed_;
    public Button skipButton_;
    public bool isWriting_;

    private string textToWrite_;
    private bool skipWrite_;

    private void Start() {
        if (skipButton_ != null)
        {
            skipButton_.onClick.AddListener(() => SkipWrite());
        };
    }

    public void Write(string Text, bool clearText=true, bool showEncounterButton = true) {
        // Remove own text if set
        string theText = Text;
        if (clearText) {
            self_.text = "";
        }
        else {
           theText = self_.text + " " + Text;
        }
        textToWrite_ = theText;

        Debug.Log("Writing text: " + theText);
        StartCoroutine(Writer(theText, showEncounterButton));
    }
    public void SkipWrite() {
        skipWrite_ = true;
    }

    IEnumerator Writer(string theText, bool showEncounterButton = true) {

        isWriting_ = true;
        // Write the text out one letter at a time
        int stringLength = theText.Length;
        string writtenText = "";
        skipWrite_ = false;
        for (int i = 0; i < stringLength; i++) {
            writtenText += theText[i];
            self_.text = writtenText;
            yield return new WaitForSeconds(1f / textSpeed_);
            if (skipWrite_) {
                break;
            }
        }
        self_.text = textToWrite_;
        if (showEncounterButton)
        {
            WorldMapController.instance_.ShowEncounterButton();
        };
    }
    public void ClearWriter() {
        self_.text = "";
        WorldMapController.instance_.ShowEncounterButton(false);
    }

}
