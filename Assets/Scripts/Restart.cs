using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
