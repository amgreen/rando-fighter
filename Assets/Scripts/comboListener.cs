using UnityEngine;
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

    /*
        TOTALLY RANDOM
        Player 1 QAZ WSX EDC RFV TGB
        Player 2 ]'/ [;. PL, OKM IJN
        */

    /*
    ARROW KEYS SET
    Player 1 WASD TYU GHJ
    Player 2 ARROW 456 123
    */

    /*
    EVERYTHING SET
    Player 1 WASD TYU
    Player 2 ARROW 123
    */
    public int layout = 3;          //Stores which kind of layout 
    /*                              1: TOTALLY RANDOM
                                    2: ARROWS SET
                                    3: EVERYTHING SET                                
    */
    public int keyBufferSize = 8;   //Determines how many key inputs to store in the queue. A larger number will slow performance.
    int comboTextLinger = 2;        //Determines how long your combo text will stay on screen before disappearing

    public AudioClip hadouken2;     //Pointer to hadouken2.wav
    public AudioClip dodge;         //Pointer to dodge.wav
    public AudioClip whirlwind;     //Pointer to whirlwind.wav
    public AudioClip punchHigh;
    public AudioClip punchMed;
    public AudioClip punchLow;
    AudioSource audio;              //Something that helps play audio

    float hadoukenThreshold = 1f;        //Amount of buffer time between hadoukens
    float tatsumakiThreshold = 1f;       //Amount of buffer time between tatsumakis
    float whirlwindThreshold = 2f;       //Amount of buffer time between whirlwinds
    public float punchHighThreshold = .5f;
    public float punchMedThreshold = .5f;
    public float punchLowThreshold = .5f;

    char[] comboHadouken;           //char array that stores input to combo
    char[] comboTatsumaki;          //char array that stores input to combo
    char[] comboWhirlwind;          //char array that stores input to combo
    char[] comboPunchHigh;
    char[] comboPunchMed;
    char[] comboPunchLow;

    float comboTimeHadouken;        //Time value since last Hadouken
    float comboTimeTatsumaki;       //Time value since last Tatsumaki
    float comboTimeWhirlwind;       //Time value since last Whirlwind
    float comboTimePunchHigh;
    float comboTimePunchMed;
    float comboTimePunchLow;
    float timeLastCombo;            //Amount of time since last combo

    public float comboTimeout = 1.5f;

    float comboTimeHadouken2;
    float comboTimeTatsumaki2;
    float comboTimeWhirlwind2;
    float comboTimePunchHigh2;
    float comboTimePunchMed2;
    float comboTimePunchLow2;
    float timeLastCombo2;
    //char lastInput;                 //Stores what was last pressed so we don't get repeats  DEPRECIATED


    int[] randomIntArray;           //Random int array that determines which layout

    ButtonPressed bpInput;          //Unimportant placeholder for ButtonPressed Object

    Queue<ButtonPressed> buttonPressedQueue = new Queue<ButtonPressed>();   //The queue used to store the input buffer
    Queue<ButtonPressed> buttonPressedQueue2 = new Queue<ButtonPressed>();

    Dictionary<int, KeyCode> inputMappings1 = new Dictionary<int, KeyCode>();    //Dictionary used for randomizing input
    Dictionary<int, KeyCode> inputMappings2 = new Dictionary<int, KeyCode>();

    public bool punchHighBool1 = false;
    public bool punchMedBool1 = false;
    public bool punchLowBool1 = false;
    public bool hadoukenBool1 = false;
    public bool whirlwindBool1 = false;

    public bool punchHighBool2 = false;
    public bool punchMedBool2 = false;
    public bool punchLowBool2 = false;
    public bool hadoukenBool2 = false;
    public bool whirlwindBool2 = false;

    void Start()
    {
        // '1' up
        // '2' down
        // '3' left
        // '4' right
        // '5' high
        // '6' medium
        // '7' low

        comboHadouken = new char[3] { '2', '4', '5' };
        comboTatsumaki = new char[3] { '2', '3', '7' };
        comboWhirlwind = new char[3] { '5', '6', '7' };
        comboPunchHigh = new char[2] { '3', '5' };
        comboPunchMed = new char[2] { '4', '6' };
        comboPunchLow = new char[2] { '2', '7' };

        /*hadoukenThreshold = 1f;
        tatsumakiThreshold = 1f;
        whirlwindThreshold = 2f;
        punchHighThreshold = .1f;
        punchMedThreshold = .1f;
        punchLowThreshold = .1f;*/

        //keyBufferSize = 8;       
        //comboTextLinger = 2;
        timeLastCombo = 0f;
        comboTimeHadouken = 0f;
        comboTimeTatsumaki = 0f;
        comboTimeWhirlwind = 0f;
        comboTimePunchHigh = 0f;
        comboTimePunchMed = 0f;
        comboTimePunchLow = 0f;

        timeLastCombo2 = 0f;
        comboTimeHadouken2 = 0f;
        comboTimeTatsumaki2 = 0f;
        comboTimeWhirlwind2 = 0f;
        comboTimePunchHigh2 = 0f;
        comboTimePunchMed2 = 0f;
        comboTimePunchLow2 = 0f;
        audio = GetComponent<AudioSource>();

        punchHighBool1 = false;
        punchMedBool1 = false;
        punchLowBool1 = false;
        hadoukenBool1 = false;
        whirlwindBool1 = false;

        punchHighBool2 = false;
        punchMedBool2 = false;
        punchLowBool2 = false;
        hadoukenBool2 = false;
        whirlwindBool2 = false;

        //layout = 3;
        print("The layout is: " + layout);
        initializeMappings(layout);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.time - timeLastCombo > comboTextLinger) comboOutput.text = "(Nothing!)";
        //if (Time.time - timeLastCombo2 > comboTextLinger) comboOutput2.text = "(Nothing!)";

        if (layout == 1)
        {
            if (Input.GetKeyDown(inputMappings1[1]))
            {
                ButtonPressed bp = new ButtonPressed('1', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings1[2]))
            {
                ButtonPressed bp = new ButtonPressed('2', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings1[3]))
            {
                ButtonPressed bp = new ButtonPressed('3', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings1[4]))
            {
                ButtonPressed bp = new ButtonPressed('4', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings1[5]))
            {
                ButtonPressed bp = new ButtonPressed('5', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings1[6]))
            {
                ButtonPressed bp = new ButtonPressed('6', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings1[7]))
            {
                ButtonPressed bp = new ButtonPressed('7', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }

            if (Input.GetKeyDown(inputMappings2[1]))
            {
                ButtonPressed bp = new ButtonPressed('1', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings2[2]))
            {
                ButtonPressed bp = new ButtonPressed('2', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings2[3]))
            {
                ButtonPressed bp = new ButtonPressed('3', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings2[4]))
            {
                ButtonPressed bp = new ButtonPressed('4', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings2[5]))
            {
                ButtonPressed bp = new ButtonPressed('5', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings2[6]))
            {
                ButtonPressed bp = new ButtonPressed('6', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings2[7]))
            {
                ButtonPressed bp = new ButtonPressed('7', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
        }
        else if (layout == 2)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                ButtonPressed bp = new ButtonPressed('1', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ButtonPressed bp = new ButtonPressed('2', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                ButtonPressed bp = new ButtonPressed('3', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                ButtonPressed bp = new ButtonPressed('4', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings1[1]))
            {
                ButtonPressed bp = new ButtonPressed('5', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings1[2]))
            {
                ButtonPressed bp = new ButtonPressed('6', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(inputMappings1[3]))
            {
                ButtonPressed bp = new ButtonPressed('7', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ButtonPressed bp = new ButtonPressed('1', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ButtonPressed bp = new ButtonPressed('2', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ButtonPressed bp = new ButtonPressed('3', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ButtonPressed bp = new ButtonPressed('4', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(inputMappings2[1]))
            {
                ButtonPressed bp = new ButtonPressed('5', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(inputMappings2[2]))
            {
                ButtonPressed bp = new ButtonPressed('6', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(inputMappings2[3]))
            {
                ButtonPressed bp = new ButtonPressed('7', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
        }
        else if (layout == 3)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                ButtonPressed bp = new ButtonPressed('1', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ButtonPressed bp = new ButtonPressed('2', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                ButtonPressed bp = new ButtonPressed('3', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                ButtonPressed bp = new ButtonPressed('4', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                ButtonPressed bp = new ButtonPressed('5', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                ButtonPressed bp = new ButtonPressed('6', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                ButtonPressed bp = new ButtonPressed('7', Time.time);
                buttonPressedEnqueue(buttonPressedQueue, bp);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ButtonPressed bp = new ButtonPressed('1', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ButtonPressed bp = new ButtonPressed('2', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ButtonPressed bp = new ButtonPressed('3', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ButtonPressed bp = new ButtonPressed('4', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                ButtonPressed bp = new ButtonPressed('5', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                ButtonPressed bp = new ButtonPressed('6', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                ButtonPressed bp = new ButtonPressed('7', Time.time);
                buttonPressedEnqueue2(buttonPressedQueue2, bp);
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

        Queue<ButtonPressed> q1 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> q2 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> q3 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> qPunchHigh = new Queue<ButtonPressed>();
        Queue<ButtonPressed> qPunchMed = new Queue<ButtonPressed>();
        Queue<ButtonPressed> qPunchLow = new Queue<ButtonPressed>();
        foreach (ButtonPressed obj in q)
        {
            q1.Enqueue(obj);
            q2.Enqueue(obj);
            q3.Enqueue(obj);
            qPunchHigh.Enqueue(obj);
            qPunchMed.Enqueue(obj);
            qPunchLow.Enqueue(obj);
        }
        if (containsCombo(q1, comboHadouken, 0) && Time.time - comboTimeHadouken > hadoukenThreshold)
        {
            comboTimeHadouken = Time.time;
            timeLastCombo = Time.time;
            audio.PlayOneShot(hadouken2);
            hadoukenBool1 = true;
            q.Clear();
        }
        /*if (containsCombo(q2, comboTatsumaki, 0) && Time.time - comboTimeTatsumaki > tatsumakiThreshold)
        {
            comboTimeTatsumaki = Time.time;
            timeLastCombo = Time.time;
            comboOutput.text = "TATSUMAKI SEMPUKYAKU";
            audio.PlayOneShot(dodge);
            q.Clear();
        }*/
        if (containsCombo(q3, comboWhirlwind, 0) && Time.time - comboTimeWhirlwind > whirlwindThreshold)
        {
            comboTimeWhirlwind = Time.time;
            timeLastCombo = Time.time;
            audio.PlayOneShot(whirlwind);
            whirlwindBool1 = true;
            q.Clear();
        }
        if (containsCombo(qPunchHigh, comboPunchHigh, 0) && Time.time - comboTimePunchHigh > punchHighThreshold)
        {
            comboTimePunchHigh = Time.time;
            timeLastCombo = Time.time;
            audio.PlayOneShot(punchHigh);
            punchHighBool1 = true;
            q.Clear();
        }
        if (containsCombo(qPunchMed, comboPunchMed, 0) && Time.time - comboTimePunchMed > punchMedThreshold)
        {
            comboTimePunchMed = Time.time;
            timeLastCombo = Time.time;
            audio.PlayOneShot(punchMed);
            punchMedBool1 = true;
            q.Clear();
        }
        if (containsCombo(qPunchLow, comboPunchLow, 0) && Time.time - comboTimePunchLow > punchLowThreshold)
        {
            comboTimePunchLow = Time.time;
            timeLastCombo = Time.time;
            audio.PlayOneShot(punchLow);
            punchLowBool1 = true;
            q.Clear();
        }
        queuePrint(q);
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

        Queue<ButtonPressed> q1 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> q2 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> q3 = new Queue<ButtonPressed>();
        Queue<ButtonPressed> qPunchHigh = new Queue<ButtonPressed>();
        Queue<ButtonPressed> qPunchMed = new Queue<ButtonPressed>();
        Queue<ButtonPressed> qPunchLow = new Queue<ButtonPressed>();
        foreach (ButtonPressed obj in q)
        {
            q1.Enqueue(obj);
            q2.Enqueue(obj);
            q3.Enqueue(obj);
            qPunchHigh.Enqueue(obj);
            qPunchMed.Enqueue(obj);
            qPunchLow.Enqueue(obj);
        }
        if (containsCombo(q1, comboHadouken, 0) && Time.time - comboTimeHadouken2 > hadoukenThreshold)
        {
            comboTimeHadouken2 = Time.time;
            timeLastCombo2 = Time.time;
            audio.PlayOneShot(hadouken2);
            hadoukenBool2 = true;
            q.Clear();
        }
        /*if (containsCombo(q2, comboTatsumaki, 0) && Time.time - comboTimeTatsumaki2 > tatsumakiThreshold)
        {
            comboTimeTatsumaki2 = Time.time;
            timeLastCombo2 = Time.time;
            comboOutput2.text = "TATSUMAKI SEMPUKYAKU";
            audio.PlayOneShot(dodge);
            q.Clear();
        }*/
        if (containsCombo(q3, comboWhirlwind, 0) && Time.time - comboTimeWhirlwind2 > whirlwindThreshold)
        {
            comboTimeWhirlwind2 = Time.time;
            timeLastCombo2 = Time.time;
            audio.PlayOneShot(whirlwind);
            whirlwindBool2 = true;
            q.Clear();
        }
        if (containsCombo(qPunchHigh, comboPunchHigh, 0) && Time.time - comboTimePunchHigh2 > punchHighThreshold)
        {
            comboTimePunchHigh2 = Time.time;
            timeLastCombo2 = Time.time;
            audio.PlayOneShot(punchHigh);
            punchHighBool2 = true;
            q.Clear();
        }
        if (containsCombo(qPunchMed, comboPunchMed, 0) && Time.time - comboTimePunchMed2 > punchMedThreshold)
        {
            comboTimePunchMed2 = Time.time;
            timeLastCombo2 = Time.time;
            audio.PlayOneShot(punchMed);
            punchMedBool2 = true;
            q.Clear();
        }
        if (containsCombo(qPunchLow, comboPunchLow, 0) && Time.time - comboTimePunchLow2 > punchLowThreshold)
        {
            comboTimePunchLow2 = Time.time;
            timeLastCombo2 = Time.time;
            audio.PlayOneShot(punchLow);
            punchLowBool2 = true;
            q.Clear();
        }
        queuePrint(q);
    }

    private void initializeMappings(int layout)
    {
        if (layout == 1)
        {
            List<int> l = GenerateRandom(15);
            string s1 = "";
            string s2 = "";
            randomIntArray = l.ToArray();
            KeyCode[] charArray1 = { KeyCode.Q, KeyCode.A, KeyCode.Z,
                                    KeyCode.W, KeyCode.S, KeyCode.X,
                                    KeyCode.E, KeyCode.D, KeyCode.C,
                                    KeyCode.R, KeyCode.F, KeyCode.V,
                                    KeyCode.T, KeyCode.G, KeyCode.B};
            KeyCode[] charArray2 = { KeyCode.RightBracket, KeyCode.DoubleQuote, KeyCode.Question,
                                    KeyCode.LeftBracket, KeyCode.Semicolon, KeyCode.Period,
                                    KeyCode.P, KeyCode.L, KeyCode.Comma,
                                    KeyCode.O, KeyCode.K, KeyCode.M,
                                    KeyCode.I, KeyCode.J, KeyCode.N};
            /*KeyCode[] charArray2 = {    KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3,
                                        KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6,
                                        KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9};*/
            for (int i = 0; i < randomIntArray.Length; i++)
            {
                inputMappings1.Add(randomIntArray[i], charArray1[i]);
                s1 += randomIntArray[i] + "" + charArray1[i] + "  ";

                inputMappings2.Add(randomIntArray[i], charArray2[i]);
                s2 += randomIntArray[i] + "" + charArray2[i] + "  ";
            }
            print("The cipher1 is: " + s1);
            print("The cipher2 is: " + s2);
        }
        else if (layout == 2)
        {
            List<int> l = GenerateRandom(6);
            string s = "";
            randomIntArray = l.ToArray();
            KeyCode[] charArray1 = { KeyCode.T, KeyCode.Y, KeyCode.U,
                                    KeyCode.G, KeyCode.H, KeyCode.J};
            KeyCode[] charArray2 = {    KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3,
                                        KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6 };
            string s1 = "";
            string s2 = "";
            for (int i = 0; i < randomIntArray.Length; i++)
            {
                inputMappings1.Add(randomIntArray[i], charArray1[i]);
                s1 += randomIntArray[i] + "" + charArray1[i] + "  ";

                inputMappings2.Add(randomIntArray[i], charArray2[i]);
                s2 += randomIntArray[i] + "" + charArray2[i] + "  ";
            }
            print("The cipher1 is: " + s1);
            print("The cipher2 is: " + s2);
        }
        else if (layout == 3)
            print("The layout is 3");
        else
        {
            print("UH OH");
        }
    }
}
