using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class keyListener : MonoBehaviour
{

    public class ButtonPressed
    {
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
    public int comboTextLinger;     //Determines how long your combo text will stay on screen before disappearing
    public Text keyInput;           //Pointer to text boxes
    public Text comboOutput;        //Pointer to text boxes
    public AudioClip hadouken2;     //Pointer to hadouken2.wav
    public AudioClip dodge;         //Pointer to dodge.wav
    public AudioClip whirlwind;     //Pointer to whirlwind.wav
    AudioSource audio;              //Something that helps play audio

    char[] comboHadouken;           //char array that stores input to combo
    char[] comboTatsumaki;          //char array that stores input to combo
    char[] comboWhirlwind;          //char array that stores input to combo
    float comboTimeHadouken;        //Time value since last Hadouken
    float comboTimeTatsumaki;       //Time value since last Tatsumaki
    float comboTimeWhirlwind;       //Time value since last Whirlwind
    float hadoukenThreshold;        //Amount of buffer time between hadoukens
    float tatsumakiThreshold;       //Amount of buffer time between tatsumakis
    float whirlwindThreshold;       //Amount of buffer time between whirlwinds
    float timeLastCombo;            //Amount of time since last combo

    System.Random rnd1 = new System.Random();
    int layout;

    ButtonPressed bpInput;
    char lastInput;
    Queue<ButtonPressed> buttonPressedQueue = new Queue<ButtonPressed>();   //The queue used to store the input buffer

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
                found = containsCombo(queue, combo, cPos + 1);
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
        print("The print stack is: " + s);
    }

    public void buttonPressedEnqueue(Queue<ButtonPressed> q, ButtonPressed bp)
    {
        while (q.Count > 0 && Time.time - q.Peek().TimeCreated > 2)
        {
            //print("delete! There are " + q.Count + " buttons left.");
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
            //print(timeLastCombo);
            comboOutput.text = "HADOUKEN";
            audio.PlayOneShot(hadouken2);
            q.Clear();
        }
        if (containsCombo(q2, comboTatsumaki, 0) && Time.time - comboTimeTatsumaki > tatsumakiThreshold)
        {
            comboTimeTatsumaki = Time.time;
            timeLastCombo = Time.time;
            comboOutput.text = "TATSUMAKI SEMPUKYAKU";
            audio.PlayOneShot(dodge);
            q.Clear();
        }
        if (containsCombo(q3, comboWhirlwind, 0) && Time.time - comboTimeWhirlwind > whirlwindThreshold)
        {
            comboTimeWhirlwind = Time.time;
            timeLastCombo = Time.time;
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

        comboHadouken = new char[3] { '2', '4', '5' };
        comboTatsumaki = new char[3] { '2', '3', '7' };
        comboWhirlwind = new char[3] { '5', '6', '7' };

        keyBufferSize = 8;       //Determines how many key inputs to store in the queue. A larger number will slow performance.
        timeLastCombo = 0f;
        comboTimeHadouken = 0f;
        comboTimeTatsumaki = 0f;
        comboTimeWhirlwind = 0f;
        hadoukenThreshold = 1f;
        tatsumakiThreshold = 1f;
        whirlwindThreshold = 2f;

        //layout = rnd1.Next(0, 3);
        layout = 2;
        print("Case: " + layout);

        /*Queue<ButtonPressed> testQueue = new Queue<ButtonPressed>();
        testQueue.Enqueue(new ButtonPressed('2', Time.time));
        testQueue.Enqueue(new ButtonPressed('4', Time.time));
        testQueue.Enqueue(new ButtonPressed('5', Time.time));
        queuePrint(testQueue);

        if (containsCombo(testQueue, comboHadouken, 0)) print("Default input has Hadouken");
        else print("Default input does not have Hadouken");*/

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Player 1 QAZ WSX EDC RFV TGB
        //Player 2 'UP''DOWN''LEFT''RIGHT' ]'/[;.PL,OK

        if (Time.time - timeLastCombo > comboTextLinger) comboOutput.text = "(Nothing!)";

        switch (layout)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    keyInput.text = "Up arrow";
                    bpInput = new ButtonPressed('1', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bpInput);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    keyInput.text = "Down arrow";
                    bpInput = new ButtonPressed('2', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bpInput);
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    keyInput.text = "Left arrow";
                    bpInput = new ButtonPressed('3', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bpInput);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    keyInput.text = "Right arrow";
                    bpInput = new ButtonPressed('4', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bpInput);
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    keyInput.text = "HIGH";
                    bpInput = new ButtonPressed('5', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bpInput);
                    //print("Random: "+rnd1.Next(0,5));
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    keyInput.text = "MED";
                    bpInput = new ButtonPressed('6', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bpInput);
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    keyInput.text = "LOW";
                    bpInput = new ButtonPressed('7', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bpInput);
                }
                break;
            case 1:
            /*
            if (Input.GetKeyDown(KeyCode.W))
            {
                keyInput.text = "Up arrow";
                ButtonPressed bp = new ButtonPressed('1', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                keyInput.text = "Down arrow";
                ButtonPressed bp = new ButtonPressed('2', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                keyInput.text = "Left arrow";
                ButtonPressed bp = new ButtonPressed('3', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.D))
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
                //print("Random: "+rnd1.Next(0,5));
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                keyInput.text = "MED";
                ButtonPressed bp = new ButtonPressed('6', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                keyInput.text = "LOW";
                ButtonPressed bp = new ButtonPressed('7', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            break;*/
            case 2:
                if (Input.GetAxis("Vertical") > 0)
                {
                    //int numberOfElements = buttonPressedQueue.Count;
                    if (lastInput == '1') { break; }
                    else
                    {
                        keyInput.text = "Up arrow";
                        bpInput = new ButtonPressed('1', Time.time);
                        buttonPressedEnqueue(buttonPressedQueue, bpInput);
                        lastInput = '1';
                        //IEnumerator cooldown = 
                    }
                }
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (lastInput == '2') { break; }
                    else
                    {
                        keyInput.text = "Down arrow";
                        bpInput = new ButtonPressed('2', Time.time);
                        buttonPressedEnqueue(buttonPressedQueue, bpInput);
                        lastInput = '2';
                    }
                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    if (lastInput == '3') { break; }
                    else
                    {
                        keyInput.text = "Left arrow";
                        bpInput = new ButtonPressed('3', Time.time);
                        buttonPressedEnqueue(buttonPressedQueue, bpInput);
                        lastInput = '3';
                    }
                }
                if (Input.GetAxis("Horizontal") > 0)
                {
                    if (lastInput == '4') { break; }
                    else
                    {
                        keyInput.text = "Right arrow";
                        bpInput = new ButtonPressed('4', Time.time);
                        buttonPressedEnqueue(buttonPressedQueue, bpInput);
                        lastInput = '4';
                    }
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    keyInput.text = "HIGH";
                    bpInput = new ButtonPressed('5', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bpInput);
                    lastInput = '5';
                }
                if (Input.GetButtonDown("Fire2"))
                {
                    keyInput.text = "MED";
                    bpInput = new ButtonPressed('6', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bpInput);
                    lastInput = '6';
                }
                if (Input.GetButtonDown("Fire3"))
                {
                    if (lastInput == '7') { break; }
                    else
                    {
                        keyInput.text = "LOW";
                        bpInput = new ButtonPressed('7', Time.time);
                        buttonPressedEnqueue(buttonPressedQueue, bpInput);
                        lastInput = '7';
                    }
                }
                break;
            default:
                print("WTF");
                break;
        }
        //queuePrint(buttonPressedQueue);
    }
}
