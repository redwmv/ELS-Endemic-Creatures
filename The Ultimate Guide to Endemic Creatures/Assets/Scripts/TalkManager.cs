﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Text conversation;
    public float TextShowDelay;
    bool nextEvent = false;
    public delegate void EndConversation();
    public event EndConversation endConvoEvent;


    void Start()
    {


        //GoOutOfFrame();
        conversation = GetComponentInChildren<Text>();
        StartCoroutine(EnumMakeConversation(new string[] {"All your base are belong to us!", "Yahahahaha!"}));
        GetComponentInChildren<DownButtonHandler>().nextLineEvent += GoNextLine; // subscribing to event
    }

    public void CreateConversation(string[] lines) {
        ///<summary> Makes the conversation thing pop up, accepts an array of arguments </summary>
        ///<param> lines </param>
         
        StartCoroutine(EnumMakeConversation(lines));
    }

    IEnumerator EnumMakeConversation(string[] sentences) {
        Animator animator = GetComponent<Animator>();

        if (animator != null) {
            animator.SetBool("slidein", true);
        }

        // GoInFrame();
        enabled = true;
        foreach (string sentence in sentences) {
            for (int i = 0; i < sentence.Length + 1; i++) {
                conversation.text = sentence.Substring(0, i);
                yield return new WaitForSeconds(TextShowDelay);
            }
            yield return new WaitUntil(() => nextEvent);
        }
        nextEvent = false;
        yield return new WaitUntil(() => nextEvent); // wait for one final button click
        
        endConvoEvent();
        animator.SetBool("slidein", false);
        //GoOutOfFrame();
    }

    IEnumerator WaitingForNext() {
        yield return new WaitUntil(() => nextEvent);
        nextEvent = false;
    }

    void GoNextLine() {
        nextEvent = true;
    }

    void GoOutOfFrame() {
        Vector3 distance = new Vector3(0, 15, 0);
        transform.position -= distance;
        conversation.transform.position -= new Vector3(0, 200, 0);
        GetComponentInChildren<DownButtonHandler>().transform.position -= distance;
    }

    void GoInFrame() {
        Vector3 distance = new Vector3(0, 15, 0);
        transform.position += distance;
        conversation.transform.position += new Vector3(0, 200, 0);
        GetComponentInChildren<DownButtonHandler>().transform.position += distance;
    }
} 
