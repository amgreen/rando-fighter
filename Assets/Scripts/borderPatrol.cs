using UnityEngine;
using System.Collections;

public class borderPatrol : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherObject = collision.collider.gameObject;
        if (otherObject.tag == "Player")
        {
            otherObject.GetComponent<PlayerClass>().health=0;
        }
    }
}
