using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpSpeed;

    private Rigidbody2D rb2d;
    private Vector2 movement;
    public bool inAir = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
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
            /* Ray2D characterRay = new Ray2D(Vector2.down, gameObject.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(characterRay.origin, characterRay.direction);
            Debug.Log(hit); 
            inAir = hit ? false : true;*/

            inAir = Collider2D.IsTouching(ground);

            if (!inAir)
            {

                rb2d.AddForce(Vector2.up * jumpSpeed);
                
                //inAir = true;
            }
            
        }
    }

}
