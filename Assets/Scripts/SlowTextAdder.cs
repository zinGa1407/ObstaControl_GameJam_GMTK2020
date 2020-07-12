using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SlowTextAdder : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueHolder = null;

    [SerializeField]
    private TMP_Text dialogueField = null;

    private string message;
    
    public void StartNewDialogueText(string text)
    {
        dialogueField.text = "";
        message = text;
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        dialogueHolder.SetActive(true);

        yield return new WaitForSeconds(2f);

        foreach (char letter in message.ToCharArray())
        {
            dialogueField.text += letter;
            //if (typeSound1 && typeSound2)
            //    SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);

            yield return new WaitForSeconds(0.03f);
            //yield return 0;
            //yield return new WaitForSeconds(letterPause);
        }
        
        
        yield return new WaitForSeconds(5f);

        dialogueHolder.SetActive(false);
    }
}
