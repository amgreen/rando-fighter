using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public GameObject ground;
    public float speed;
    public float jumpSpeed;

    private Rigidbody2D rb2d;
    private Vector2 movement;
    public bool inAir = false;
    public bool player1 = true;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {


        inAir = !gameObject.GetComponent<Collider2D>().IsTouching(ground.GetComponent<Collider2D>());
        if (player1)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                /* Ray2D characterRay = new Ray2D(Vector2.down, gameObject.transform.position);
                RaycastHit2D hit = Physics2D.Raycast(characterRay.origin, characterRay.direction);
                Debug.Log(hit); 
                inAir = hit ? false : true;*/
                // Debug.Log("upped");


                if (!inAir)
                {

                    rb2d.AddForce(Vector2.up * jumpSpeed);
                    //transform.position += Vector3.up * jumpSpeed * Time.deltaTime;
                    inAir = true;
                }

            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W))
            {
                /* Ray2D characterRay = new Ray2D(Vector2.down, gameObject.transform.position);
                RaycastHit2D hit = Physics2D.Raycast(characterRay.origin, characterRay.direction);
                Debug.Log(hit); 
                inAir = hit ? false : true;*/
                // Debug.Log("upped");


                if (!inAir)
                {

                    rb2d.AddForce(Vector2.up * jumpSpeed);
                    //transform.position += Vector3.up * jumpSpeed * Time.deltaTime;
                    inAir = true;
                }
            }
        }
    }
}
