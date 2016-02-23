using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public GameObject ground;
    public float speed;
    public float jumpSpeed;
    public float dashSpeed;

    private Rigidbody2D rb2d;
    private Vector2 movement;
    public bool inAir = false;
    public bool player2 = true;
    public bool facingleft = false; 
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
        gameObject.GetComponent<Animator>().SetBool("inAir",inAir);
        if (player2)
        {

            //quick dash
            if (gameObject.GetComponent<comboListener>().dashLeftBool2)
            {
                facingleft = true;
                if (inAir)
                {
                    gameObject.GetComponent<comboListener>().dashLeftBool2 = false;
                }
                else
                {
                    transform.position += Vector3.left * dashSpeed * Time.deltaTime;
                    gameObject.GetComponent<comboListener>().dashLeftBool2 = false;
                    gameObject.GetComponent<Animator>().SetTrigger("dashAtk");
                    
                }
            }
            if (gameObject.GetComponent<comboListener>().dashRightBool2)
            {
                facingleft = false;
                if (inAir)
                {
                    gameObject.GetComponent<comboListener>().dashRightBool2 = false;
                }
                else
                {
                    transform.position += Vector3.right * dashSpeed * Time.deltaTime;
                    gameObject.GetComponent<comboListener>().dashRightBool2 = false;
                    gameObject.GetComponent<Animator>().SetTrigger("dashAtk");
                    
                }
            }


            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (facingleft == false)
                {
                    gameObject.transform.Rotate(0, 180, 0);
                }
                facingleft = true;
                gameObject.GetComponent<Animator>().SetTrigger("walking");
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                if (facingleft == true)
                {
                    gameObject.transform.Rotate(0, 180, 0);
                }
                facingleft = false;
                gameObject.GetComponent<Animator>().SetTrigger("walking");
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
            //quick dash
            if (gameObject.GetComponent<comboListener>().dashLeftBool1)
            {
                if (facingleft == false)
                {
                    gameObject.transform.Rotate(0, 180, 0);
                }
                facingleft = true;
                if (inAir)
                {
                    gameObject.GetComponent<comboListener>().dashLeftBool1 = false;
                }
                else
                {
                    transform.position += Vector3.left * dashSpeed * Time.deltaTime;
                    gameObject.GetComponent<comboListener>().dashLeftBool1 = false;
                    gameObject.GetComponent<Animator>().SetTrigger("dashAtk");
                    
                }
            }
            if (gameObject.GetComponent<comboListener>().dashRightBool1)
            {
                if (facingleft == true)
                {
                    gameObject.transform.Rotate(0, 180, 0);
                }
                facingleft = false;
                if (inAir)
                {
                    gameObject.GetComponent<comboListener>().dashRightBool1 = false;
                }
                else
                {
                    transform.position += Vector3.right * dashSpeed * Time.deltaTime;
                    gameObject.GetComponent<comboListener>().dashRightBool1 = false;
                    gameObject.GetComponent<Animator>().SetTrigger("dashAtk");
                    
                }
                
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (facingleft == false)
                {
                    gameObject.transform.Rotate(0, 180, 0);
                }
                facingleft = true;
                gameObject.GetComponent<Animator>().SetTrigger("walking");

            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                if (facingleft == true)
                {
                    gameObject.transform.Rotate(0, 180, 0);
                }
                facingleft = false;
                gameObject.GetComponent<Animator>().SetTrigger("walking");

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
