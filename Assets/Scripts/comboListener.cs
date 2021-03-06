﻿using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class comboListener : MonoBehaviour
{
    static System.Random random = new System.Random();      //Important for randomizing

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

    public class ComboTimes
    {
        //public float Hadouken { get; set; }        //Time value since last Hadouken
        //public float Tatsumaki { get; set; }       //Time value since last Tatsumaki
        //public float Whirlwind { get; set; }       //Time value since last Whirlwind
        public float Punch { get; set; }
        public float PunchHighCombo { get; set; }
        public float PunchMedCombo { get; set; }
        public float PunchLowCombo { get; set; }
        //public float LastCombo { get; set; }            //Amount of time since last combo
        public float Dash { get; set; }
        public float LeftRightPressed { get; set; }
        public ComboTimes()
        {
            //Hadouken = -5f;
            //Tatsumaki = -5f;
            //Whirlwind = -5f;
            Punch = -5f;
            PunchHighCombo = -5f;
            PunchMedCombo = -5f;
            PunchLowCombo = -5f;
            //LastCombo = 0f;
            Dash = 0f;
            LeftRightPressed = 0f;
        }
    }
    ComboTimes player1Times = new ComboTimes();
    ComboTimes player2Times = new ComboTimes();
    
    public int layout = 3;
    /*
        Stores which kind of layout 
        1: TOTALLY RANDOM
        2: ARROWS SET
        3: EVERYTHING SET 
    */
    public int keyBufferSize = 8;   //Determines how many key inputs to store in the queue. A larger number will slow performance.

    /*
    public AudioClip hadoukenSound;      //Pointer to hadouken2.wav
    public AudioClip dodgeSound;         //Pointer to dodge.wav
    public AudioClip whirlwindSound;     //Pointer to whirlwind.wav
    */
    public AudioClip punchHighSound;
    public AudioClip punchMedSound;
    public AudioClip punchLowSound;
    public AudioClip dash;

    AudioSource audio;              //Something that helps play audio

    //float hadoukenThreshold = 1f;        //Amount of buffer time between hadoukens
    //float tatsumakiThreshold = 1f;       //Amount of buffer time between tatsumakis
    //float whirlwindThreshold = 2f;       //Amount of buffer time between whirlwinds
    public float punchThreshold;
    public float punchLowComboThreshold;
    public float punchMedComboThreshold;
    public float punchHighComboThreshold;
    public float dashThreshold;

    //char[] comboHadouken = new char[3] { '8', '8', '8' };           //char array that stores input to combo
    //char[] comboTatsumaki = new char[3] { '8', '8', '8' };          //8 isn't a char being used. 
    //char[] comboWhirlwind = new char[3] { '8', '8', '8' };          //all 8 values are depreciated
    char[] comboPunchHigh = new char[3] { '5', '6', '5' };
    char[] comboPunchMed = new char[3] { '6', '7', '5' };
    char[] comboPunchLow = new char[2] { '7', '6' };
    //char[] comboDashLeft = new char[2] { '3', '3' };
    //char[] comboDashRight = new char[2] { '4', '4' };

    public float comboTimeout = 1.0f;

    public Text p1Combo1;
    public Text p1Combo2;
    public Text p1Combo3;
    public Text p2Combo1;
    public Text p2Combo2;
    public Text p2Combo3;

    int[] randomIntArray;           //Random int array that determines which layout

    ButtonPressed bpInput;          //Unimportant placeholder for ButtonPressed Object

    public Queue<ButtonPressed> buttonPressedQueue = new Queue<ButtonPressed>();   //The queue used to store the input buffer
    public Queue<ButtonPressed> buttonPressedQueue2 = new Queue<ButtonPressed>();

    Dictionary<int, KeyCode> inputMappings1 = new Dictionary<int, KeyCode>();    //Dictionary used for randomizing input
    Dictionary<int, KeyCode> inputMappings2 = new Dictionary<int, KeyCode>();

    public bool punchHighBool1 = false;
    public bool punchMedBool1 = false;
    public bool punchLowBool1 = false;
    public bool punchHighComboBool1 = false;
    public bool punchMedComboBool1 = false;
    public bool punchLowComboBool1 = false;
    public bool dashLeftBool1 = false;
    public bool dashRightBool1 = false;

    public bool punchHighBool2 = false;
    public bool punchMedBool2 = false;
    public bool punchLowBool2 = false;
    public bool punchHighComboBool2 = false;
    public bool punchMedComboBool2 = false;
    public bool punchLowComboBool2 = false;
    public bool dashLeftBool2 = false;
    public bool dashRightBool2 = false;

    public bool p1c1Bool = false;
    public bool p1c2Bool = false;
    public bool p1c3Bool = false;
    public bool p1DBool = false;
    public bool p2c1Bool = false;
    public bool p2c2Bool = false;
    public bool p2c3Bool = false;
    public bool p2DBool = false;

    bool lastLeftPressed = false;
    bool lastRightPressed = false;

    void Start()
    {
        // '1' up
        // '2' down
        // '3' left
        // '4' right
        // '5' high
        // '6' medium
        // '7' low

        audio = GetComponent<AudioSource>();

        punchHighBool1 = false;
        punchMedBool1 = false;
        punchLowBool1 = false;
        punchHighComboBool1 = false;
        punchMedComboBool1 = false;
        punchLowComboBool1 = false;
        dashLeftBool1 = false;
        dashRightBool1 = false;

        punchHighBool2 = false;
        punchMedBool2 = false;
        punchLowBool2 = false;
        punchHighComboBool2 = false;
        punchMedComboBool2 = false;
        punchLowComboBool2 = false;
        dashLeftBool2 = false;
        dashRightBool2 = false;

        //layout = 3;
        if (layout == 2) {
            print("The layout is random.");
            initializeMappings(layout);
        }
        else if (layout == 3)
            print("The layout is fixed.");
        else {
            print("The layout is: " + layout);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.time - timeLastCombo > comboTextLinger) comboOutput.text = "(Nothing!)";
        //if (Time.time - timeLastCombo2 > comboTextLinger) comboOutput2.text = "(Nothing!)";
        if (!gameObject.GetComponent<PlayerController>().player2)
        {
            if (Time.time - player1Times.LeftRightPressed > 0.4)
            {
                lastLeftPressed = false;
                lastRightPressed = false;
            }
        }
        else {
            if (Time.time - player2Times.LeftRightPressed > 0.5)
            {
                lastLeftPressed = false;
                lastRightPressed = false;
            }
        }

        if (layout == 2)
        {
            if (!gameObject.GetComponent<PlayerController>().player2)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    /*
                    ButtonPressed bp = new ButtonPressed('1', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    */
                    lastLeftPressed = false;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    /*
                    ButtonPressed bp = new ButtonPressed('2', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    */
                    lastLeftPressed = false;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    /*
                    ButtonPressed bp = new ButtonPressed('3', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    */
                    player1Times.LeftRightPressed = Time.time;
                    if (lastLeftPressed && Time.time - player1Times.Dash > dashThreshold)
                    {
                        player1Times.Dash = Time.time;
                        audio.PlayOneShot(dash);
                        dashLeftBool1 = true;
                        p1DBool = true;
                    }
                    lastLeftPressed = true;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    /*
                    ButtonPressed bp = new ButtonPressed('4', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    */
                    player1Times.LeftRightPressed = Time.time;
                    if (lastRightPressed && Time.time - player1Times.Dash > dashThreshold)
                    {
                        player1Times.Dash = Time.time;
                        audio.PlayOneShot(dash);
                        dashRightBool1 = true;
                        p1DBool = true;
                    }
                    lastLeftPressed = false;
                    lastRightPressed = true;
                }
                if (Input.GetKeyDown(inputMappings1[0]))
                {
                    ButtonPressed bp = new ButtonPressed('5', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    if (Time.time - player1Times.Punch > punchThreshold)
                    {
                        player1Times.Punch = Time.time;
                        punchHighBool1 = true;
                    }
                }
                if (Input.GetKeyDown(inputMappings1[1]))
                {
                    ButtonPressed bp = new ButtonPressed('6', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    if (Time.time - player1Times.Punch > punchThreshold)
                    {
                        player1Times.Punch = Time.time;
                        punchMedBool1 = true;
                    }
                }
                if (Input.GetKeyDown(inputMappings1[2]))
                {
                    ButtonPressed bp = new ButtonPressed('7', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    if (Time.time - player1Times.Punch > punchThreshold)
                    {
                        player1Times.Punch = Time.time;
                        punchLowBool1 = true;
                    }
                }
            }

            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    lastLeftPressed = false;
                    lastRightPressed = false;
                    /*
                    ButtonPressed bp = new ButtonPressed('1', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    */
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    lastLeftPressed = false;
                    lastRightPressed = false;
                    /*
                    ButtonPressed bp = new ButtonPressed('2', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    */
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    player2Times.LeftRightPressed = Time.time;
                    /*
                    ButtonPressed bp = new ButtonPressed('3', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    */
                    if (lastLeftPressed && Time.time - player2Times.Dash > dashThreshold)
                    {
                        player2Times.Dash = Time.time;
                        audio.PlayOneShot(dash);
                        dashLeftBool2 = true;
                        p2DBool = true;
                    }
                    lastLeftPressed = true;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    player2Times.LeftRightPressed = Time.time;
                    /*
                    ButtonPressed bp = new ButtonPressed('4', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    */
                    if (lastRightPressed && Time.time - player1Times.Dash > dashThreshold)
                    {
                        player2Times.Dash = Time.time;
                        audio.PlayOneShot(dash);
                        dashRightBool2 = true;
                        p2DBool = true;
                    }
                    lastLeftPressed = false;
                    lastRightPressed = true;
                }
                if (Input.GetKeyDown(inputMappings2[0]))
                {
                    ButtonPressed bp = new ButtonPressed('5', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    if (Time.time - player2Times.Punch > punchThreshold)
                    {
                        player2Times.Punch = Time.time;
                        punchHighBool2 = true;
                    }
                }
                if (Input.GetKeyDown(inputMappings2[1]))
                {
                    ButtonPressed bp = new ButtonPressed('6', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    if (Time.time - player2Times.Punch > punchThreshold)
                    {
                        player2Times.Punch = Time.time;
                        punchMedBool2 = true;
                    }
                }
                if (Input.GetKeyDown(inputMappings2[2]))
                {
                    ButtonPressed bp = new ButtonPressed('7', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    if (Time.time - player2Times.Punch > punchThreshold)
                    {
                        player2Times.Punch = Time.time;
                        punchLowBool2 = true;
                    }
                }
            }
        }
        else if (layout == 3)
        {
            if (!gameObject.GetComponent<PlayerController>().player2)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    /*
                    ButtonPressed bp = new ButtonPressed('1', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    */
                    lastLeftPressed = false;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    /*
                    ButtonPressed bp = new ButtonPressed('2', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    */
                    lastLeftPressed = false;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    player1Times.LeftRightPressed = Time.time;
                    /*
                    ButtonPressed bp = new ButtonPressed('3', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    */
                    if (lastLeftPressed && Time.time - player1Times.Dash > dashThreshold)
                    {
                        player1Times.Dash = Time.time;
                        audio.PlayOneShot(dash);
                        dashLeftBool1 = true;
                        p1DBool = true;
                    }
                    lastLeftPressed = true;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    player1Times.LeftRightPressed = Time.time;
                    /*
                    ButtonPressed bp = new ButtonPressed('4', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    */
                    if (lastRightPressed && Time.time - player1Times.Dash > dashThreshold)
                    {
                        player1Times.Dash = Time.time;
                        audio.PlayOneShot(dash);
                        dashRightBool1 = true;
                        p1DBool = true;
                    }
                    lastLeftPressed = false;
                    lastRightPressed = true;
                }
                if (Input.GetKeyDown(KeyCode.T))
                {
                    ButtonPressed bp = new ButtonPressed('5', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    if (Time.time - player1Times.Punch > punchThreshold)
                    {
                        player1Times.Punch = Time.time;
                        punchHighBool1 = true;
                    }
                    lastLeftPressed = false;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    ButtonPressed bp = new ButtonPressed('6', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    if (Time.time - player1Times.Punch > punchThreshold)
                    {
                        player1Times.Punch = Time.time;
                        punchMedBool1 = true;
                    }
                    lastLeftPressed = false;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.U))
                {
                    ButtonPressed bp = new ButtonPressed('7', Time.time);
                    buttonPressedEnqueue(buttonPressedQueue, bp);
                    if (Time.time - player1Times.Punch > punchThreshold)
                    {
                        player1Times.Punch = Time.time;
                        punchLowBool1 = true;
                    }
                    lastLeftPressed = false;
                    lastRightPressed = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    lastLeftPressed = false;
                    lastRightPressed = false;
                    /*
                    ButtonPressed bp = new ButtonPressed('1', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    */
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    lastLeftPressed = false;
                    lastRightPressed = false;
                    /*
                    ButtonPressed bp = new ButtonPressed('2', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    */
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    player2Times.LeftRightPressed = Time.time;
                    /*
                    ButtonPressed bp = new ButtonPressed('3', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    */
                    if (lastLeftPressed && Time.time - player2Times.Dash > dashThreshold)
                    {
                        player2Times.Dash = Time.time;
                        audio.PlayOneShot(dash);
                        dashLeftBool2 = true;
                        p2DBool = true;
                    }
                    lastLeftPressed = true;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    player2Times.LeftRightPressed = Time.time;
                    /*
                    ButtonPressed bp = new ButtonPressed('4', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    */
                    if (lastRightPressed && Time.time - player1Times.Dash > dashThreshold)
                    {
                        player2Times.Dash = Time.time;
                        audio.PlayOneShot(dash);
                        dashRightBool2 = true;
                        p2DBool = true;
                    }
                    lastLeftPressed = false;
                    lastRightPressed = true;
                }
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    ButtonPressed bp = new ButtonPressed('5', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    if (Time.time - player2Times.Punch > punchThreshold)
                    {
                        player2Times.Punch = Time.time;
                        punchHighBool2 = true;
                    }
                    lastLeftPressed = false;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    ButtonPressed bp = new ButtonPressed('6', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    if (Time.time - player2Times.Punch > punchThreshold)
                    {
                        player2Times.Punch = Time.time;
                        punchMedBool2 = true;
                    }
                    lastLeftPressed = false;
                    lastRightPressed = false;
                }
                if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    ButtonPressed bp = new ButtonPressed('7', Time.time);
                    buttonPressedEnqueue2(buttonPressedQueue2, bp);
                    if (Time.time - player2Times.Punch > punchThreshold)
                    {
                        player2Times.Punch = Time.time;
                        punchLowBool2 = true;
                    }
                    lastLeftPressed = false;
                    lastRightPressed = false;
                }
            }
        }
        else print("WTF");
    }

    //These next two functions create an array of 15 ints without replacement.
    public static List<int> GenerateRandom(int count, int min, int max)
    {

        //  initialize set S to empty
        //  for J := N-M + 1 to N do
        //    T := RandInt(1, J)
        //    if T is not in S then
        //      insert T in S
        //    else
        //      insert J in S
        //
        // adapted for C# which does not have an inclusive Next(..)
        // and to make it from configurable range not just 1.

        if (max <= min || count < 0 ||
                // max - min > 0 required to avoid overflow
                (count > max - min && max - min > 0))
        {
            // need to use 64-bit to support big ranges (negative min, positive max)
            throw new ArgumentOutOfRangeException("Range " + min + " to " + max +
                    " (" + ((Int64)max - (Int64)min) + " values), or count " + count + " is illegal");
        }

        // generate count random values.
        HashSet<int> candidates = new HashSet<int>();

        // start count values before max, and end at max
        for (int top = max - count; top < max; top++)
        {
            // May strike a duplicate.
            // Need to add +1 to make inclusive generator
            // +1 is safe even for MaxVal max value because top < max
            if (!candidates.Add(random.Next(min, top + 1)))
            {
                // collision, add inclusive max.
                // which could not possibly have been added before.
                candidates.Add(top);
            }
        }

        // load them in to a list, to sort
        List<int> result = candidates.ToList();

        // shuffle the results because HashSet has messed
        // with the order, and the algorithm does not produce
        // random-ordered results (e.g. max-1 will never be the first value)
        for (int i = result.Count - 1; i > 0; i--)
        {
            int k = random.Next(i + 1);
            int tmp = result[k];
            result[k] = result[i];
            result[i] = tmp;
        }
        return result;
    }

    public static List<int> GenerateRandom(int count)
    {
        return GenerateRandom(count, 0, count);
    }

    public bool containsCombo(Queue<ButtonPressed> queue, char[] combo, int cPos)
    {
        bool found = false;
        if (combo.Length == cPos)
        {
            //Found the combo!
            //print("We did it boys.");
            return true;
        }
        else if (queue.Count == 0)
        {
            //Combo not present =(
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
        //Sanity check to print what's in the queue stack
        string s = "";
        foreach (ButtonPressed obj in q)
        {
            s += " " + obj.Button;
        }
        print("The print stack is: " + s);
    }

    public char keyCodeReturn(KeyCode kc) {
        if (kc == KeyCode.T)
            return 'T';
        else if (kc == KeyCode.Y)
            return 'Y';
        else if (kc == KeyCode.U)
            return 'U';
        else if (kc == KeyCode.Keypad1)
            return '1';
        else if (kc == KeyCode.Keypad2)
            return '2';
        else if (kc == KeyCode.Keypad3)
            return '3';
        else return ' ';
    }

    public void buttonPressedEnqueue(Queue<ButtonPressed> q, ButtonPressed bp)
    {
        while (q.Count > 0 && Time.time - q.Peek().TimeCreated > comboTimeout)
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

        /*
        Queue<ButtonPressed> q1 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> q2 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> q3 = new Queue<ButtonPressed>();
        */
        Queue<ButtonPressed> qPunchHigh = new Queue<ButtonPressed>();
        Queue<ButtonPressed> qPunchMed = new Queue<ButtonPressed>();
        Queue<ButtonPressed> qPunchLow = new Queue<ButtonPressed>();
        //Queue<ButtonPressed> qDashLeft = new Queue<ButtonPressed>();
        //Queue<ButtonPressed> qDashRight = new Queue<ButtonPressed>();
        foreach (ButtonPressed obj in q)
        {
            /*
            q1.Enqueue(obj);
            q2.Enqueue(obj);
            q3.Enqueue(obj);
            */
            qPunchHigh.Enqueue(obj);
            qPunchMed.Enqueue(obj);
            qPunchLow.Enqueue(obj);
            //qDashLeft.Enqueue(obj);
            //qDashRight.Enqueue(obj);
        }
        /*if (containsCombo(q1, comboHadouken, 0) && Time.time - player1Times.Hadouken > hadoukenThreshold)
        {
            player1Times.Hadouken = Time.time;
            player1Times.LastCombo = Time.time;
            //audio.PlayOneShot(hadoukenSound);
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
        if (containsCombo(q3, comboWhirlwind, 0) && Time.time - player1Times.Whirlwind > whirlwindThreshold)
        {
            player1Times.Whirlwind = Time.time;
            player1Times.LastCombo = Time.time;
            //audio.PlayOneShot(whirlwindSound);
            q.Clear();
        }*/
        if (containsCombo(qPunchHigh, comboPunchHigh, 0) && Time.time - player1Times.PunchHighCombo > punchHighComboThreshold)
        {

            player1Times.PunchHighCombo = Time.time;
            //player1Times.LastCombo = Time.time;
            audio.PlayOneShot(punchHighSound);
            punchHighComboBool1 = true;
            p1c1Bool = true;
            q.Clear();
        }
        if (containsCombo(qPunchMed, comboPunchMed, 0) && Time.time - player1Times.PunchMedCombo > punchMedComboThreshold)
        {
            player1Times.PunchMedCombo = Time.time;
            //player1Times.LastCombo = Time.time;
            audio.PlayOneShot(punchMedSound);
            punchMedComboBool1 = true;
            p1c2Bool = true;
            q.Clear();
        }
        if (containsCombo(qPunchLow, comboPunchLow, 0) && Time.time - player1Times.PunchLowCombo > punchLowComboThreshold)
        {
            player1Times.PunchLowCombo = Time.time;
            //player1Times.LastCombo = Time.time;
            audio.PlayOneShot(punchLowSound);
            punchLowComboBool1 = true;
            p1c3Bool = true;
            q.Clear();
        }
        /*
        if (containsCombo(qDashLeft, comboDashLeft, 0) && Time.time - player1Times.Dash > dashThreshold)
        {
            player1Times.Dash = Time.time;
            //player2Times.LastCombo = Time.time;
            audio.PlayOneShot(dash);
            dashLeftBool1 = true;
            p1DBool = true;
            q.Clear();
        }
        if (containsCombo(qDashRight, comboDashRight, 0) && Time.time - player1Times.Dash > dashThreshold)
        {
            player1Times.Dash = Time.time;
            //player2Times.LastCombo = Time.time;
            audio.PlayOneShot(dash);
            dashRightBool1 = true;
            p1DBool = true;
            q.Clear();
        }
        */
        //queuePrint(q);
    }

    public void buttonPressedEnqueue2(Queue<ButtonPressed> q, ButtonPressed bp)
    {
        while (q.Count > 0 && Time.time - q.Peek().TimeCreated > comboTimeout)
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
        /*
        Queue<ButtonPressed> q1 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> q2 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> q3 = new Queue<ButtonPressed>();
        */
        Queue<ButtonPressed> qPunchHigh = new Queue<ButtonPressed>();
        Queue<ButtonPressed> qPunchMed = new Queue<ButtonPressed>();
        Queue<ButtonPressed> qPunchLow = new Queue<ButtonPressed>();
        //Queue<ButtonPressed> qDashLeft = new Queue<ButtonPressed>();
        //Queue<ButtonPressed> qDashRight = new Queue<ButtonPressed>();
        foreach (ButtonPressed obj in q)
        {
            /*
            q1.Enqueue(obj);
            q2.Enqueue(obj);
            q3.Enqueue(obj);
            */
            qPunchHigh.Enqueue(obj);
            qPunchMed.Enqueue(obj);
            qPunchLow.Enqueue(obj);
            //qDashLeft.Enqueue(obj);
            //qDashRight.Enqueue(obj);
        }
        /*
        if (containsCombo(q1, comboHadouken, 0) && Time.time - player2Times.Hadouken > hadoukenThreshold)
        {
            player2Times.Hadouken = Time.time;
            player2Times.LastCombo = Time.time;
            //audio.PlayOneShot(hadoukenSound);
            q.Clear();
        }
        if (containsCombo(q2, comboTatsumaki, 0) && Time.time - comboTimeTatsumaki2 > tatsumakiThreshold)
        {
            comboTimeTatsumaki2 = Time.time;
            timeLastCombo2 = Time.time;
            comboOutput2.text = "TATSUMAKI SEMPUKYAKU";
            audio.PlayOneShot(dodge);
            q.Clear();
        }
        if (containsCombo(q3, comboWhirlwind, 0) && Time.time - player2Times.Whirlwind > whirlwindThreshold)
        {
            player2Times.Whirlwind = Time.time;
            player2Times.LastCombo = Time.time;
            //audio.PlayOneShot(whirlwindSound);
            q.Clear();
        }
        */
        if (containsCombo(qPunchHigh, comboPunchHigh, 0) && Time.time - player2Times.PunchHighCombo > punchHighComboThreshold)
        {

            player2Times.PunchHighCombo = Time.time;
            //player2Times.LastCombo = Time.time;
            audio.PlayOneShot(punchHighSound);
            punchHighComboBool2 = true;
            p2c1Bool = true;
            q.Clear();
        }
        if (containsCombo(qPunchMed, comboPunchMed, 0) && Time.time - player2Times.PunchMedCombo > punchMedComboThreshold)
        {

            player2Times.PunchMedCombo = Time.time;
            //player2Times.LastCombo = Time.time;
            audio.PlayOneShot(punchMedSound);
            punchMedComboBool2 = true;
            p2c2Bool = true;
            q.Clear();
        }
        if (containsCombo(qPunchLow, comboPunchLow, 0) && Time.time - player2Times.PunchLowCombo > punchLowComboThreshold)
        {

            player2Times.PunchLowCombo = Time.time;
            //player2Times.LastCombo = Time.time;
            audio.PlayOneShot(punchLowSound);
            punchLowComboBool2 = true;
            p2c3Bool = true;
            q.Clear();
        }
        /*
        if (containsCombo(qDashLeft, comboDashLeft, 0) && Time.time - player2Times.Dash > dashThreshold)
        {
            player2Times.Dash = Time.time;
            //player2Times.LastCombo = Time.time;
            audio.PlayOneShot(dash);
            dashLeftBool2 = true;
            p2DBool = true;
            q.Clear();
        }
        if (containsCombo(qDashRight, comboDashRight, 0) && Time.time - player2Times.Dash > dashThreshold)
        {
            player2Times.Dash = Time.time;
            audio.PlayOneShot(dash);
            dashRightBool2 = true;
            p2DBool = true;
            q.Clear();
        }
        */
        //queuePrint(q);
    }

    private void initializeMappings(int layout)
    {
        if (layout == 2)
        {
            List<int> l = GenerateRandom(3);
            string s = "";
            randomIntArray = l.ToArray();
            
            if (!gameObject.GetComponent<PlayerController>().player2)
            {

                KeyCode[] charArray1 = { KeyCode.T, KeyCode.Y, KeyCode.U};

                for (int i = 0; i < randomIntArray.Length; i++)
                {
                    inputMappings1.Add(randomIntArray[i], charArray1[i]);
                    s += randomIntArray[i] + "" + charArray1[i] + "  ";
                }
                //print("The cipher1 is: " + s);
                char high = keyCodeReturn(inputMappings1[0]);
                char mid = keyCodeReturn(inputMappings1[1]);
                char low = keyCodeReturn(inputMappings1[2]);
                p1Combo1.text = "Uppercut - "+high+" "+mid+" "+high;
                p1Combo2.text = "Haymaker - " + mid + " " + low + " " + high;
                p1Combo3.text = "Sweep - " + low + " " + mid;
            }
            else
            {

                KeyCode[] charArray2 = {    KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3};

                for (int i = 0; i < randomIntArray.Length; i++)
                {
                    inputMappings2.Add(randomIntArray[i], charArray2[i]);
                    s += randomIntArray[i] + "" + charArray2[i] + "  ";
                }
                //print("The cipher2 is: " + s);
                char high = keyCodeReturn(inputMappings2[0]);
                char mid = keyCodeReturn(inputMappings2[1]);
                char low = keyCodeReturn(inputMappings2[2]);
                p2Combo1.text = "Uppercut - " + high + " " + mid + " " + high;
                p2Combo2.text = "Haymaker - " + mid + " " + low + " " + high;
                p2Combo3.text = "Sweep - " + low + " " + mid;
            }
        }
        else if (layout == 3)
            print("The layout is 3. But this shouldn't happen.");
        else
        {
            print("UH OH");
        }
    }
}
