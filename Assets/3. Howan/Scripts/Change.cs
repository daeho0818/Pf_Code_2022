/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change : MonoBehaviour
{
    float Timer = 0;
    float WaitingTime;
    bool showing = true;
    public GameObject Back;

    // Start is called before the first frame update
    void Start()
    {
        //WaitingTime = Random.Range(1.0f, 8.0f);
        WaitingTime = 4;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > WaitingTime*//* && showing == true*//*)
        {
            this.gameObject.layer = 0;
            Timer = 0;
            //WaitingTime = Random.Range(2.0f, 4.0f);
            WaitingTime = 1;
        }
        if (Timer > WaitingTime*//* && showing == false*//*)
        {

            Timer = 0;
            //WaitingTime = Random.Range(1.0f, 8.0f);
            WaitingTime = 1;
        }
    }

    *//*void hide()
    {
        BackObject.SetActive(false);
        //gameObject.renderer.enabled = false;
        showing = false;
        return;
    }

    void show()
    {
        gameObject.SetActive(true);
        //gameObject.renderer.enabled = true;
        showing = true;
        return;
    }*//*
}*/