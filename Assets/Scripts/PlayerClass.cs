using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerClass : MonoBehaviour {

    public float health = 100;
    public Image myHealthBar;
    private float initialHealth;

    private enum AttackType {Punch, Kick, Counter};

    public float attackTime = 2.0f;
    public float attackDamage = 10.0f;

    public bool currentlyAttacking = false;
    public bool attackedAlready = false;

    public GameObject myFist;

    // Use this for initialization
    void Start () {
        initialHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Attack(AttackType.Punch));
        }
        myHealthBar.fillAmount = health / initialHealth;
	}

    IEnumerator Attack(AttackType executeAttack)
    {
        currentlyAttacking = true;
        myFist.SetActive(true);
        Debug.Log(executeAttack);

        if (executeAttack == AttackType.High)
        {
            myFist.SetActive(true);
            yield return new WaitForSeconds(attackTimeHigh);
            myFist.SetActive(false);
        }
        if (executeAttack == AttackType.Middle)
        {
            middleFist.SetActive(true);
            yield return new WaitForSeconds(attackTimeMiddle);
            middleFist.SetActive(false);
        }
        if (executeAttack == AttackType.Low)
        {
            myFoot.SetActive(true);
            yield return new WaitForSeconds(attackTimeLow);
            myFoot.SetActive(false);
        }      
        currentlyAttacking = false;
        attackedAlready = false;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherObject = collision.collider.gameObject;
        if (otherObject.tag == "high")
        {
            if (otherObject.transform.parent.GetComponent<PlayerClass>().currentlyAttacking && !attackedAlready)
            {
                attackedAlready = true;

                health -= attackDamage;
                Debug.Log("Health: " + health);
            }

                health -= attackDamageHigh;
                Debug.Log("Health: " + health);
            }
        }
        if (otherObject.tag == "middle")
        {
            attackedAlready = true;
            health -= attackDamageMiddle;
            Debug.Log("Health: " + health);
        }
        if (otherObject.tag == "low")
        {
            attackedAlready = true;
            health -= attackDamageLow;
            Debug.Log("Health: " + health);

        }

    }
}
