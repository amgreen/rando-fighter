using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpSpeed;

    private Rigidbody2D rb2d;
    private Vector2 movement;
    private bool jumped = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {

        
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetAxis("Horizontal")>0)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            if (jumped == false)
            {

                rb2d.AddForce(Vector2.up * jumpSpeed);
                
                jumped = true;
            }
            
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "ground")
        {
            jumped = false;
        }
    }
}
