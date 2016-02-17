using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpSpeed;

    private Rigidbody2D rb2d;
    private Vector2 movement;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        bool moveVertical = Input.GetButtonDown("Fire1");
        if (moveVertical)
        {
            movement = new Vector2(moveHorizontal, jumpSpeed);
        }
        else
        {
            movement = new Vector2(moveHorizontal, 0);
        }
        rb2d.AddForce(movement*speed);
    }
}
