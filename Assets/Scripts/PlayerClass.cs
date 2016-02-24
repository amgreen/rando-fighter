using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class PlayerClass : MonoBehaviour {

    public float health = 100;
    public Image myHealthBar;
    private float initialHealth;

    private enum AttackType {High, Middle, Low, HighCombo, MiddleCombo, LowCombo};
    private bool active = false;
    public float comboDamageMultiplier = 3.0f;
    public float comboTimeMultiplier = 2.0f;
    public float attackTimeHigh = .5f;
    public float attackTimeMiddle = .5f;
    public float attackTimeLow = .5f;
    public float attackTimeProjectile = .5f;
    
    public float attackDamageHigh = 3.0f;
    public float attackDamageMiddle = 7.0f;
    public float attackDamageLow = 5.0f;
    

    public bool currentlyAttacking = false;
    public bool attackedAlready = false;

    public GameObject myFist;
    public GameObject middleFist;
    public GameObject myFoot;
    public GameObject myFistCombo;
    public GameObject middleFistCombo;
    public GameObject myFootCombo;

    public AudioClip[] audioClips;

    AudioSource audio;


    // Use this for initialization
    void Start () {
        initialHealth = health;
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!gameObject.GetComponent<PlayerController>().player2)
        {
            if (gameObject.GetComponent<comboListener>().punchHighComboBool1)
            {
                StartCoroutine(Attack(AttackType.HighCombo));
                gameObject.GetComponent<comboListener>().punchHighComboBool1 = false;
            }
            if (gameObject.GetComponent<comboListener>().punchMedComboBool1)
            {
                StartCoroutine(Attack(AttackType.MiddleCombo));
                gameObject.GetComponent<comboListener>().punchMedComboBool1 = false;
            }
            if (gameObject.GetComponent<comboListener>().punchLowComboBool1)
            {
                StartCoroutine(Attack(AttackType.LowCombo));
                gameObject.GetComponent<comboListener>().punchLowComboBool1 = false;
            }

            //normal attacks
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
            if (gameObject.GetComponent<comboListener>().punchHighComboBool2)
            {
                StartCoroutine(Attack(AttackType.HighCombo));
                gameObject.GetComponent<comboListener>().punchHighComboBool2 = false;
            }
            if (gameObject.GetComponent<comboListener>().punchMedComboBool2)
            {
                StartCoroutine(Attack(AttackType.MiddleCombo));
                gameObject.GetComponent<comboListener>().punchMedComboBool2 = false;
            }
            if (gameObject.GetComponent<comboListener>().punchLowComboBool2)
            {
                StartCoroutine(Attack(AttackType.LowCombo));
                gameObject.GetComponent<comboListener>().punchLowComboBool2 = false;
            }

            //normal attacks

            if (gameObject.GetComponent<comboListener>().punchHighBool2)
            {
                StartCoroutine(Attack(AttackType.High));
                //gameObject.GetComponent<comboListener>().punchHighBool2 = false;
            }
            if (gameObject.GetComponent<comboListener>().punchMedBool2)
            {
                StartCoroutine(Attack(AttackType.Middle));
                //gameObject.GetComponent<comboListener>().punchMedBool2 = false;
            }
            if (gameObject.GetComponent<comboListener>().punchLowBool2)
            {
                StartCoroutine(Attack(AttackType.Low));
                //gameObject.GetComponent<comboListener>().punchLowBool2 = false;
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

        // Turn off all other hi

        /*
        myFist.SetActive(false);
        middleFist.SetActive(false);
        myFoot.SetActive(false);
        myFistCombo.SetActive(false);
        middleFistCombo.SetActive(false);
        myFootCombo.SetActive(false);*/

        currentlyAttacking = true;
        Debug.Log(executeAttack);
        if (!active)
        {
            if (executeAttack == AttackType.High)
            {
                active = true;
                myFist.SetActive(true);
                gameObject.GetComponent<Animator>().SetTrigger("highAtk");
                yield return new WaitForSeconds(attackTimeHigh);
                myFist.SetActive(false);
                active = false;
            }
            if (executeAttack == AttackType.Middle)
            {
                active = true;
                middleFist.SetActive(true);
                gameObject.GetComponent<Animator>().SetTrigger("midAtk");
                yield return new WaitForSeconds(attackTimeMiddle);
                middleFist.SetActive(false);
                active = false;
            }
            if (executeAttack == AttackType.Low)
            {
                active = true;
                myFoot.SetActive(true);
                gameObject.GetComponent<Animator>().SetTrigger("lowAtk");
                yield return new WaitForSeconds(attackTimeLow);
                myFoot.SetActive(false);
                active = false;
            }
        }
        //combos
        else {
            
            if (executeAttack == AttackType.HighCombo)
            {
                turnOffAttacks();
                active = true;
                myFistCombo.SetActive(true);
                gameObject.GetComponent<Animator>().SetTrigger("highAtk");
                yield return new WaitForSeconds(attackTimeHigh * comboTimeMultiplier);
                myFistCombo.SetActive(false);
                active = false;
            }
            if (executeAttack == AttackType.MiddleCombo)
            {
                turnOffAttacks();
                active = true;
                middleFistCombo.SetActive(true);
                gameObject.GetComponent<Animator>().SetTrigger("midAtk");
                yield return new WaitForSeconds(attackTimeMiddle * comboTimeMultiplier);
                middleFistCombo.SetActive(false);
                active = false;
            }
            if (executeAttack == AttackType.LowCombo)
            {
                turnOffAttacks();
                active = true;
                myFootCombo.SetActive(true);
                gameObject.GetComponent<Animator>().SetTrigger("lowAtk");
                yield return new WaitForSeconds(attackTimeLow * comboTimeMultiplier);
                myFootCombo.SetActive(false);
                active = false;
            }
        }
        currentlyAttacking = false;
        attackedAlready = false;
    }

    void playSound(int clip)
    {
        audio.PlayOneShot(audioClips[clip]);
    }

    void turnOffAttacks()
    {
        myFist.SetActive(false);
        middleFist.SetActive(false);
        myFoot.SetActive(false);
        myFistCombo.SetActive(false);
        middleFistCombo.SetActive(false);
        myFootCombo.SetActive(false);

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherObject = collision.collider.gameObject;
        if (otherObject.tag == "high")
        {
                attackedAlready = true;
                health -= attackDamageHigh;
                Debug.Log("Health: " + health);
            playSound(0);
            
            gameObject.GetComponent<Animator>().SetTrigger("playerHit");
        }
        if (otherObject.tag == "middle")
        {
            
                attackedAlready = true;
                health -= attackDamageMiddle;
                Debug.Log("Health: " + health);
                gameObject.GetComponent<Animator>().SetTrigger("playerHit");
            playSound(1);
        }
        if (otherObject.tag == "low")
        {
            attackedAlready = true;
            health -= attackDamageLow;
            Debug.Log("Health: " + health);
            gameObject.GetComponent<Animator>().SetTrigger("playerHit");
            playSound(2);
        }


        //Combos


        if (otherObject.tag == "highCombo")
        {
            attackedAlready = true;
            health -= attackDamageHigh * comboDamageMultiplier;
            Debug.Log("Health: " + health);
            gameObject.GetComponent<Animator>().SetTrigger("playerHit");
            playSound(3);
        }
        if (otherObject.tag == "middleCombo")
        {
            attackedAlready = true;
            health -= attackDamageMiddle * comboDamageMultiplier;
            Debug.Log("Health: " + health);
            gameObject.GetComponent<Animator>().SetTrigger("playerHit");
            playSound(4);

        }
        if (otherObject.tag == "lowCombo")
        {
            
                attackedAlready = true;
                health -= attackDamageLow * comboDamageMultiplier;
                Debug.Log("Health: " + health);
                gameObject.GetComponent<Animator>().SetTrigger("playerHit");
            playSound(5);
        }

    }
}
