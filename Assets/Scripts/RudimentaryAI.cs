using UnityEngine;
using System.Collections;

public class RudimentaryAI : MonoBehaviour {

    KeyCode[] myKeyCodes = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3 };
    char[] myChars = new char[] { '1','2','3','4','5','6','7' };

    comboListener combos;

    public float AIStartDelayTime = 3.0f;


	// Use this for initialization
	void Start () {
        combos = gameObject.GetComponent<comboListener>();
        StartCoroutine(InitAI());
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public IEnumerator InitAI()
    {

        while (GameObject.Find("MainMenu") != null)
        {
            yield return null;
        }
        yield return new WaitForSeconds(AIStartDelayTime);
        StartCoroutine(DetermineMoveSet());
    }

    public IEnumerator DetermineMoveSet()
    {
        float AISpeed = 0.2f;

        combos.punchHighBool2 = Random.value > 0.5f;
        combos.punchMedBool2 = Random.value > 0.5f;
        combos.punchLowBool2 = Random.value > 0.5f;
        combos.punchHighComboBool2 = Random.value > 0.5f;
        combos.punchMedComboBool2 = Random.value > 0.5f;
        combos.punchLowComboBool2 = Random.value > 0.5f;
        combos.dashLeftBool2 = GameObject.Find("Player1").transform.position.x < gameObject.transform.position.x;
        combos.dashRightBool2 = GameObject.Find("Player1").transform.position.x > gameObject.transform.position.x;
        yield return new WaitForSeconds(AISpeed);
        StartCoroutine(DetermineMoveSet());
    }
}
