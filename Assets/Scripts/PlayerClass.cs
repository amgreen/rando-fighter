using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerClass : MonoBehaviour {

    public float health = 100;
    public Image myHealthBar;
    private float initialHealth;

    private enum AttackType {High, Middle, Low, Projectile};

    public float attackTimeHigh = 1.0f;
    public float attackTimeMiddle = 1.0f;
    public float attackTimeLow = 1.0f;
    public float attackTimeProjectile = 1.0f;
    
    public float attackDamageHigh = 10.0f;
    public float attackDamageMiddle = 10.0f;
    public float attackDamageLow = 10.0f;
    public float attackDamageProjectile = 10.0f;

    public bool currentlyAttacking = false;
    public bool attackedAlready = false;

    public GameObject myFist;
    public GameObject middleFist;
    public GameObject myFoot;

  

    // Use this for initialization
    void Start () {
        initialHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameObject.GetComponent<PlayerController>().player2)
        {
            if (gameObject.GetComponent<comboListener>().punchHighBool1)
            {
                StartCoroutine(Attack(AttackType.High));
                gameObject.GetComponent<comboListener>().punchHighBool1 = false;
            }
            if (gameObject.GetComponent<comboListener>().punchMedBool1)
            {
                StartCoroutine(Attack(AttackType.Middle));
                gameObject.GetComponent<comboListener>().punchMedBool1 = false;
            }
            if (gameObject.GetComponent<comboListener>().punchLowBool1)
            {
                StartCoroutine(Attack(AttackType.Low));
                gameObject.GetComponent<comboListener>().punchLowBool1 = false;
            }
        }
        else
        {
            if (gameObject.GetComponent<comboListener>().punchHighBool2)
            {
                StartCoroutine(Attack(AttackType.High));
                gameObject.GetComponent<comboListener>().punchHighBool2 = false;
            }
            if (gameObject.GetComponent<comboListener>().punchMedBool2)
            {
                StartCoroutine(Attack(AttackType.Middle));
                gameObject.GetComponent<comboListener>().punchMedBool2 = false;
            }
            if (gameObject.GetComponent<comboListener>().punchLowBool2)
            {
                StartCoroutine(Attack(AttackType.Low));
                gameObject.GetComponent<comboListener>().punchLowBool2 = false;
            }
        }

        myHealthBar.fillAmount = health / initialHealth;
		if (health <= 0) {
			Destroy (gameObject);
		}

        myHealthBar.transform.FindChild("Pic").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
        myHealthBar.transform.FindChild("Pic").gameObject.GetComponent<Image>().color = gameObject.GetComponent<Image>().color;
    }

    IEnumerator Attack(AttackType executeAttack)
    {
        currentlyAttacking = true;
        Debug.Log(executeAttack);
        if (executeAttack == AttackType.High)
        {

            myFist.SetActive(true);
			gameObject.GetComponent<Animator> ().SetTrigger ("highAtk");
            yield return new WaitForSeconds(attackTimeHigh);
            myFist.SetActive(false);
        }
        if (executeAttack == AttackType.Middle)
        {
            middleFist.SetActive(true);
			gameObject.GetComponent<Animator> ().SetTrigger ("midAtk");
            yield return new WaitForSeconds(attackTimeMiddle);
            middleFist.SetActive(false);
        }
        if (executeAttack == AttackType.Low)
        {
            myFoot.SetActive(true);
			gameObject.GetComponent<Animator> ().SetTrigger ("lowAtk");
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
            if (otherObject.transform.parent.GetComponent<PlayerClass>().currentlyAttacking)
            {
                attackedAlready = true;
                health -= attackDamageHigh;
                Debug.Log("Health: " + health);
            }
            gameObject.GetComponent<Animator>().SetTrigger("playerHit");
        }
        if (otherObject.tag == "middle")
        {
            if (otherObject.transform.parent.GetComponent<PlayerClass>().currentlyAttacking)
            {
                attackedAlready = true;
                health -= attackDamageMiddle;
                Debug.Log("Health: " + health);
                gameObject.GetComponent<Animator>().SetTrigger("playerHit");
            }
        }
        if (otherObject.tag == "low")
        {
            if (otherObject.transform.parent.GetComponent<PlayerClass>().currentlyAttacking)
            {
                attackedAlready = true;
                health -= attackDamageLow;
                Debug.Log("Health: " + health);
                gameObject.GetComponent<Animator>().SetTrigger("playerHit");
            }
        }

    }
}
