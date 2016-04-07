using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour {

    [HideInInspector]
    public Power[] powers;

	public Material playerMat;
    public Power activePower;
    public GameObject gun;
    public AudioSource jumpSource;
    public int hp = 10;
    public int mp = 10;
    public bool useMobileInput = false;
    MobileInput mobileInput;

    internal GameObject powerHolder1;
    internal GameObject powerHolder2;
    internal GameObject powerHolder3;
    internal int score = 0;
    internal int playerNumber = 0;
    internal Animator anim;

    internal bool transition = false;

    //item variables
    internal bool usingItem = false;

    //movement variables
    internal Rigidbody controller;
    public float speed = 5.5f;
    internal int move = 0;

    //jump variables
    public float jumpHeight = 3f;
    internal float jumpCounter = 0f;
    internal float jumpCooldown = 1f;
    internal bool canJump = false;
    internal bool jump = false;
    internal bool jumping = false;
    float rayCastCounter = 0;
    float rayCastCooldown = 1f;


    //damage variables
    internal bool attack1 = false;
    internal bool attack2 = false;
    bool canTakeDmg = true;

    Color baseColor;
    int powerCounter = 0;


	public virtual void Start () 
    {
        playerMat.color = Color.white;

        if (GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }
        else
        {
            anim = transform.FindChild("Sphere").GetComponent<Animator>();
        }

	    jumpCounter = jumpCooldown;
	    rayCastCounter = rayCastCooldown;

        anim.SetBool("Move", true);

        controller = GetComponent<Rigidbody>();

        activePower = powers[powerCounter];

	    if (useMobileInput)
	    {
	        try
	        {
	            mobileInput = GetComponent<MobileInput>();
	        }
	        catch (Exception e)
	        {
	            mobileInput = gameObject.AddComponent<MobileInput>();
	            throw;
	        }
	    }
	}

    public virtual void Update()
    {
        if (Physics.Raycast(new Ray(transform.position, -transform.up), 0.15f, ~LayerMask.NameToLayer("EnemyProjectile")) && !canJump && rayCastCounter>rayCastCooldown)
        {
            canJump = true;
            jumping = false;

            anim.SetBool("Jump", false);

            rayCastCounter = 0;
        }

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (mobileInput.CheckTouch(touch))
                {
                    case ActionType.MoveL:

                        anim.SetFloat("Speed", 1);

                        move = -1;
                        break;

                    case ActionType.MoveR:

                        anim.SetFloat("Speed", 1);

                        move = 1;
                        break;

                    case ActionType.Jump:

                        if (!jumping)
                        {
                            anim.SetFloat("Speed", 0);
                            jump = true;
                            move = 0;
                        }
                        break;

                    case ActionType.Attack1:
                        attack1 = true;
                        break;
                    case ActionType.Attack2:
                        attack2 = true;
                        break;
                }
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);
            move = 0;
        }

        rayCastCounter += Time.deltaTime;

    }

	public virtual void FixedUpdate () 
    {

        jumpCounter += Time.fixedDeltaTime;

        Vector3 InvVelocity = new Vector3(0, 0, -controller.velocity.z *20f);
        controller.AddForce(InvVelocity, ForceMode.Force);

	    if (move == 1)
	    {
            transform.LookAt(transform.position + Camera.main.transform.right);
            Move(speed, 1f);
        }

        if (move == -1)
	    {
            transform.LookAt(transform.position - Camera.main.transform.right);
            Move(speed, -1f);
        }

	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        jump = true;
	    }

	    if (jump)
	    {
            if (jumpCounter >= jumpCooldown && canJump && !jumping)
            {
                Jump(jumpHeight);

                jumpSource.Play();
                canJump = false;
                jumpCounter = 0;
            }
	        jump = false;
	    }


        if (useMobileInput) { return; }

        if (Input.GetAxis("Horizontal") > 0.1f  || Input.GetAxis("Horizontal") < -0.1f )
        {
            anim.SetFloat("Speed", 1);
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                transform.LookAt(transform.position + Camera.main.transform.right);

            }
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                transform.LookAt(transform.position - Camera.main.transform.right);
            }
            Move(speed, Input.GetAxisRaw("Horizontal"));
        }
        if(controller.velocity.magnitude <0.4f)
        {
            anim.SetFloat("Speed", 0);
        }

    }
    public void Move(float speed, float direction)
    {
        float newSpeed;

        newSpeed = speed - controller.velocity.z;
        newSpeed = Mathf.Abs(newSpeed);

        Vector3 velocity = new Vector3(0, controller.velocity.y, newSpeed * direction);
        controller.AddForce(velocity, ForceMode.Force);

    }

    public void Jump(float jumpHeight)
    {
        anim.SetBool("Jump", true);

        controller.velocity = new Vector3(0, 0, controller.velocity.z);
        controller.AddForce(new Vector3(0, jumpHeight, controller.velocity.z), ForceMode.Impulse);
        jumping = true;
    }

    public IEnumerator DamageCounter()
    {
        if (canTakeDmg)
        {
            canTakeDmg = false;
			yield return new WaitForSeconds(1f);
            canTakeDmg = true;
        }
    }
    
    public IEnumerator Death()
    {
        //spawn particle system
        //emit 10 or so ominidirectional with huge particles
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0;

        gameObject.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool TakeDamage(int dmg)
    {
        if (!canTakeDmg) { return false; }
        hp -= dmg;
        anim.SetTrigger("Damaged");
        
        if (hp < 1) { StartCoroutine("Death"); }
        else { Camera.main.GetComponentInParent<LevelCamera>().RemoveHP(); StartCoroutine("ChangeColor"); }
        StartCoroutine("DamageCounter");

        return true;

    }

    public IEnumerator ChangeColor()
    {
        Camera.main.GetComponentInParent<LevelCamera>().hpBar.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        Camera.main.GetComponentInParent<LevelCamera>().hpBar.color = Color.white;
        yield return null;
    }

    public bool UseMP(int amountUsed)
    {
        if (amountUsed == 0) { return true;}
        if (mp < 0) { Debug.Log("NoMana"); mp = 0; return false; }

        mp -= amountUsed;
        if (mp < 0) { Debug.Log("NoMana"); mp = 0; return false; }
        Camera.main.GetComponentInParent<LevelCamera>().RemoveMP();
        return true;
    }
    
    public void UseItem(ItemInfo itemToUse)
    {
        if (usingItem) { return; }

        switch (itemToUse.itemType)
        {
            case ItemType.hp:
                usingItem = true;
                GainHP(itemToUse.gainAmount);
                break;
            case ItemType.mp:
                usingItem = true;
                GainMP(itemToUse.gainAmount);
                break;
            case ItemType.score:
                usingItem = true;
                GainScore(itemToUse.gainAmount);
                break;
        }
    }

    private void GainScore(int p)
    {
        if (!usingItem) { return; }
        Camera.main.GetComponentInParent<LevelCamera>().score += p;
        //Camera.main.GetComponentInParent<LevelCamera>().scoreText.text = "" + score;

        usingItem = false;

    }

    private void GainMP(int p)
    {
        if (!usingItem) { return; }
        mp += p;
        if (mp > 10) { mp = 10; }
        Camera.main.GetComponentInParent<LevelCamera>().AddMP();
        usingItem = false;

    }

    private void GainHP(int p)
    {
        hp += p;
        if (hp > 10) { hp = 10; }
        Camera.main.GetComponentInParent<LevelCamera>().AddHP();
        usingItem = false;

    }
}