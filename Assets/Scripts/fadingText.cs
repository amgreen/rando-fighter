using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class fadingText : MonoBehaviour
{
    public float fadeTime = 0.75f;

    float timerp1c1 = 0;
    float timerp1c2 = 0;
    float timerp1c3 = 0;
    float timerp1D = 0;
    float timerp2c1 = 0;
    float timerp2c2 = 0;
    float timerp2c3 = 0;
    float timerp2D = 0;


    public Text p1c1;
    public Text p1c2;
    public Text p1c3;
    public Text p1D;
    public Text p2c1;
    public Text p2c2;
    public Text p2c3;
    public Text p2D;

    // Use this for initialization
    void Start()
    {
        p1c1.CrossFadeAlpha(0.0f, 0.001f, false);
        p1c2.CrossFadeAlpha(0.0f, 0.001f, false);
        p1c3.CrossFadeAlpha(0.0f, 0.001f, false);
        p1D.CrossFadeAlpha(0.0f, 0.001f, false);
        p2c1.CrossFadeAlpha(0.0f, 0.001f, false);
        p2c2.CrossFadeAlpha(0.0f, 0.001f, false);
        p2c3.CrossFadeAlpha(0.0f, 0.001f, false);
        p2D.CrossFadeAlpha(0.0f, 0.001f, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player1").GetComponent<comboListener>().p1c1Bool)
        {
            GameObject.Find("Player1").GetComponent<comboListener>().p1c1Bool = false;
            timerp1c1 = Time.time;
            p1c1.CrossFadeAlpha(1.0f, 0.001f, false);
        }
        if (Time.time - timerp1c1 > 0.5)
            p1c1.CrossFadeAlpha(0.0f, fadeTime, false);



        if (GameObject.Find("Player1").GetComponent<comboListener>().p1c2Bool)
        {
            GameObject.Find("Player1").GetComponent<comboListener>().p1c2Bool = false;
            timerp1c2 = Time.time;
            p1c2.CrossFadeAlpha(1.0f, 0.001f, false);
        }
        if (Time.time - timerp1c2 > 0.5)
            p1c2.CrossFadeAlpha(0.0f, fadeTime, false);



        if (GameObject.Find("Player1").GetComponent<comboListener>().p1c3Bool)
        {
            GameObject.Find("Player1").GetComponent<comboListener>().p1c3Bool = false;
            timerp1c3 = Time.time;
            p1c3.CrossFadeAlpha(1.0f, 0.001f, false);
        }

        if (Time.time - timerp1c3 > 0.5)
            p1c3.CrossFadeAlpha(0.0f, fadeTime, false);



        if (GameObject.Find("Player1").GetComponent<comboListener>().p1DBool)
        {
            GameObject.Find("Player1").GetComponent<comboListener>().p1DBool = false;
            timerp1D = Time.time;
            p1D.CrossFadeAlpha(1.0f, 0.001f, false);
        }
        if (Time.time - timerp1D > 0.5)
            p1D.CrossFadeAlpha(0.0f, fadeTime, false);



        if (GameObject.Find("Player 2").GetComponent<comboListener>().p2c1Bool)
        {
            GameObject.Find("Player 2").GetComponent<comboListener>().p2c1Bool = false;
            timerp2c1 = Time.time;
            p2c1.CrossFadeAlpha(1.0f, 0.001f, false);
        }
        if (Time.time - timerp2c1 > 0.5)
            p2c1.CrossFadeAlpha(0.0f, fadeTime, false);



        if (GameObject.Find("Player 2").GetComponent<comboListener>().p2c2Bool)
        {
            GameObject.Find("Player 2").GetComponent<comboListener>().p2c2Bool = false;
            timerp2c2 = Time.time;
            p2c2.CrossFadeAlpha(1.0f, 0.001f, false);
        }
        if (Time.time - timerp1c2 > 0.5)
            p1c2.CrossFadeAlpha(0.0f, fadeTime, false);



        if (GameObject.Find("Player 2").GetComponent<comboListener>().p2c3Bool)
        {
            GameObject.Find("Player 2").GetComponent<comboListener>().p2c3Bool = false;
            timerp2c3 = Time.time;
            p2c3.CrossFadeAlpha(1.0f, 0.001f, false);
        }

        if (Time.time - timerp2c3 > 0.5)
            p2c3.CrossFadeAlpha(0.0f, fadeTime, false);



        if (GameObject.Find("Player 2").GetComponent<comboListener>().p2DBool)
        {
            GameObject.Find("Player 2").GetComponent<comboListener>().p2DBool = false;
            timerp2D = Time.time;
            p2D.CrossFadeAlpha(1.0f, 0.001f, false);
        }
        if (Time.time - timerp2D > 0.5)
            p2D.CrossFadeAlpha(0.0f, fadeTime, false);
    }
}
