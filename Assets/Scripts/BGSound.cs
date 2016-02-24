using UnityEngine;
using System.Collections;

public class BGSound : MonoBehaviour {


    GameObject[] musicObject;
    AudioSource audio;
    
    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        musicObject = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObject.Length == 1)
        {
            audio.Play();
        }
        else {
            for (int i = 1; i < musicObject.Length; i++)
            {
                Destroy(musicObject[i]);
            }

        }


    }

    // Update is called once per frame
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}