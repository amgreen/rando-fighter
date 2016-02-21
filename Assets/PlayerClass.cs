using UnityEngine;
using System.Collections;

public class PlayerClass : MonoBehaviour {

    public float health = 100;

    private enum AttackType {Punch, Kick, Counter};

    public float attackTime = 2.0f;
    public float attackDamage = 10.0f;

    public bool currentlyAttacking = false;
    public bool attackedAlready = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Attack(AttackType.Punch));
        }
	}

    IEnumerator Attack(AttackType executeAttack)
    {
        currentlyAttacking = true;
        Debug.Log(executeAttack);

        yield return new WaitForSeconds(attackTime);
        currentlyAttacking = false;
        attackedAlready = false;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherObject = collision.collider.gameObject;
        if (otherObject.tag == "Player")
        {
            if (otherObject.GetComponent<PlayerClass>().currentlyAttacking && !attackedAlready)
            {
                attackedAlready = true;
                health -= attackDamage;
                Debug.Log("Health: " + health);
            }
        }

    }
}
