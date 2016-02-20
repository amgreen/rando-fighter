using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class keyListener2 : MonoBehaviour
{
    
    public class ButtonPressed {
        // Created a new ButtonPressed data type that stores:
        //   1) The button pressed
        //   2) The time the button was created
        public char Button { get; set; }
        public float TimeCreated { get; set; }
        public ButtonPressed(char button, float timeCreated)
        {
            Button = button;
            TimeCreated = timeCreated;
        }
    }

    public int keyBufferSize;       //Determines how many key inputs to store in the queue. A larger number will slow performance.
    public int comboTextLinger;
    public Text keyInput;
    public Text comboOutput;
    public AudioClip hadouken2;
    public AudioClip dodge;
    public AudioClip whirlwind;
    AudioSource audio;
    
    char[] comboHadouken;
    char[] comboTatsumaki;
    char[] comboWhirlwind;
    float comboTimeHadouken;         //Time value since last Hadouken
    float comboTimeTatsumaki;
    float comboTimeWhirlwind;
    float hadoukenThreshold;
    float tatsumakiThreshold;
    float whirlwindThreshold;
    float timeLastCombo;

    Queue<ButtonPressed> buttonPressedQueue = new Queue<ButtonPressed>();

    public bool containsCombo(Queue<ButtonPressed> queue, char[] combo, int cPos)
    {
        bool found = false;
        if (combo.Length == cPos)
        {
            //print("We did it boys.");
            return true;
        }
        else if (queue.Count == 0)
        {
            //print("Alas, not long enough");
            return false;
        }
        else
        {
            char qButton = queue.Dequeue().Button;    //The button in the queue
            //print("We have " + qButton);
            char cButton = combo[cPos];    //The button required from the combo
            //print("We need " + cButton);
            if (qButton == cButton)
            {
                //print("It matches");
                found = containsCombo(queue, combo, cPos+1);
            }
            else
            {
                //print("Obviously, no match");
                found = containsCombo(queue, combo, cPos);
            }
        }
        //print("This shouldn't happen, but it's irrelevant anyway.");
        return found || false;
    }

    public void queuePrint(Queue<ButtonPressed> q)
    {
        string s = "";
        foreach (ButtonPressed obj in q)
        {
            s += " " + obj.Button;
        }
        print(s);
    }

    public void buttonPressedEnqueue(Queue<ButtonPressed> q, ButtonPressed bp)
    {
        while (q.Count > 0 && Time.time - q.Peek().TimeCreated > 2)
        {
            print("delete! There are " + q.Count + " buttons left.");
            q.Dequeue();
        }

        if (q.Count <= keyBufferSize)
        {
            q.Enqueue(bp);
        }
        if (q.Count > keyBufferSize)
        {
            q.Dequeue();
            q.Enqueue(bp);
        }

        Queue<ButtonPressed> q1 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> q2 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> q3 = new Queue<ButtonPressed>();
        foreach (ButtonPressed obj in q)
        {
            q1.Enqueue(obj);
            q2.Enqueue(obj);
            q3.Enqueue(obj);
        }
        if (containsCombo(q1, comboHadouken, 0) && Time.time - comboTimeHadouken > hadoukenThreshold)
        {
            comboTimeHadouken = Time.time;
            timeLastCombo = Time.time;
            print(timeLastCombo);
            comboOutput.text = "HADOUKEN";
            audio.PlayOneShot(hadouken2);
            q.Clear();
        }
        if (containsCombo(q2, comboTatsumaki, 0) && Time.time - comboTimeTatsumaki > tatsumakiThreshold)
        {
            comboTimeTatsumaki = Time.time;
            comboOutput.text = "TATSUMAKI SEMPUKYAKU";
            audio.PlayOneShot(dodge);
            q.Clear();
        }
        if (containsCombo(q3, comboWhirlwind, 0) && Time.time - comboTimeWhirlwind> whirlwindThreshold)
        {
            comboTimeWhirlwind = Time.time;
            comboOutput.text = "WHIRLWIND";
            audio.PlayOneShot(whirlwind);
            q.Clear();
        }
        queuePrint(q);
    }

    void Start()
    {
        // '1' up
        // '2' down
        // '3' left
        // '4' right
        // '5' high
        // '6' medium
        // '7' low
        comboTextLinger = 2;

        comboHadouken = new char[3] {'2','4','5'};
        comboTatsumaki = new char[3] {'2', '3', '7'};
        comboWhirlwind = new char[3] { '5', '6', '7'};

        keyBufferSize = 8;       //Determines how many key inputs to store in the queue. A larger number will slow performance.
        timeLastCombo = 0f;
        comboTimeHadouken = 0f;
        comboTimeTatsumaki = 0f;
        comboTimeWhirlwind = 0f;
        hadoukenThreshold = 1f;
        tatsumakiThreshold = 1f;
        whirlwindThreshold = 2f;

        Queue<ButtonPressed> testQueue = new Queue<ButtonPressed>();
        testQueue.Enqueue(new ButtonPressed('2', Time.time));
        testQueue.Enqueue(new ButtonPressed('4', Time.time));
        testQueue.Enqueue(new ButtonPressed('5', Time.time));
        queuePrint(testQueue);

        if (containsCombo(testQueue, comboHadouken, 0)) print("Default input has Hadouken");
        else print("Default input does not have Hadouken");

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeLastCombo > comboTextLinger) comboOutput.text = "(Nothing!)";
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            keyInput.text = "Up arrow";
            ButtonPressed bp = new ButtonPressed('1', Time.time);
            buttonPressedEnqueue(buttonPressedQueue, bp);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            keyInput.text = "Down arrow";
            ButtonPressed bp = new ButtonPressed('2', Time.time);
            buttonPressedEnqueue(buttonPressedQueue, bp);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            keyInput.text = "Left arrow";
            ButtonPressed bp = new ButtonPressed('3', Time.time);
            buttonPressedEnqueue(buttonPressedQueue, bp);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            keyInput.text = "Right arrow";
            ButtonPressed bp = new ButtonPressed('4', Time.time);
            buttonPressedEnqueue(buttonPressedQueue, bp);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            keyInput.text = "HIGH";
            ButtonPressed bp = new ButtonPressed('5', Time.time);
            buttonPressedEnqueue(buttonPressedQueue, bp);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            keyInput.text = "MED";
            ButtonPressed bp = new ButtonPressed('6', Time.time);
            buttonPressedEnqueue(buttonPressedQueue, bp);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            keyInput.text = "LOW";
            ButtonPressed bp = new ButtonPressed('7', Time.time);
            buttonPressedEnqueue(buttonPressedQueue, bp);
        }
    }
}
