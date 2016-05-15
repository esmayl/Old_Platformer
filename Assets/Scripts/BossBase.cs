using UnityEngine;
using System.Collections;

public enum BossStates { Moving,MovingToPlayer,Attack1,Attack2,Attack3,Jump}

public class BossBase : MonoBehaviour,BossInterface {


    internal RaycastHit hit;
    internal GameObject player;
    internal BossStates currentState;
    internal Vector3 walkDirection;
    internal Vector3 velocity;
    internal Rigidbody controller;

    public int amountOfAttack1 = 5;
    public int amountOfAttack2 = 2;
    public GameObject landParticle;
    public GameObject gun;
    public GameObject bullet;
    public LayerMask mask;
    public float speed = 5;
    public int meleeDamage = 5;
    public float rangeToStop = 10f;
    public int height = 3;
    public float hp = 100;

    internal Vector3[] pos;
    internal int attack1Counter = 0;
    internal int attack2Counter = 0;
    internal int attack3Counter = 0;
    internal bool attack1Cooldown;
    internal bool attack2Cooldown;
    internal int range = 15;

    private bool canJump = false;
    private bool canMove = true;
    private float beginX;


    public virtual void Start ()
    {
        landParticle.SetActive(false);

        beginX = transform.position.x;
        controller = GetComponent<Rigidbody>();
        StartCoroutine("DetectPlayer", range);
	}

    public void FixedUpdate()
    {

        switch (currentState)
        {
            case BossStates.Moving:

                break;

            case BossStates.Jump:

                if (!canJump) { return;}

                MoveUp();
                currentState = BossStates.Attack1;

                break;

            case BossStates.MovingToPlayer:

                if (!canMove) { return;}

                MoveToPlayer(player.transform);

                Vector3 playerPos = player.transform.position;
                playerPos.y = transform.position.y;

                transform.LookAt(playerPos);

                controller.AddForce(velocity, ForceMode.Force);

                break;

            default:
                break;
        }
    }

    public void Update ()
    {

	    if (!canJump)
	    {
	        Vector3 temp = controller.velocity;
	        temp.y = 0;

	        controller.AddForce(-temp*20f,ForceMode.Force);
	    }

        if (player)
        {
            gun.transform.LookAt(player.transform.position + transform.up);


            switch (currentState)
            {
                case BossStates.Attack1:
                    if (Random.Range(-sizeof(float), sizeof (float))/20 % 2 == 0 )
                    {
                        Attack1();
                    }
                    else
                    {
                        MoveRandomDirection();
                    }
                    break;
                case BossStates.Attack2:
                    Attack2();
                    break;
                case BossStates.Attack3:
                    break;
            }
        }
    }

    public IEnumerator DetectPlayer(float Radius)
    {
        while (true)
        {
            if (transform.position.x != beginX) { transform.position = new Vector3(beginX, transform.position.y, transform.position.z); }
            //==============================UPDATE========================================//

            Collider[] hits;
            hits = Physics.OverlapSphere(transform.position, Radius);

            if (hits.Length > 0)
            {
                foreach (Collider h in hits)
                {
                    if (h.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        player = h.gameObject;

                        float distanceToPlayer = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

                        float attackCondition = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

                        if (distanceToPlayer < Radius)
                        {
                            currentState = BossStates.MovingToPlayer;
                        }

                        if (distanceToPlayer < rangeToStop)
                        {
                            currentState = BossStates.Jump;
                        }

                        if (distanceToPlayer < Radius/2 && attack1Counter < amountOfAttack1)
                        {
                            currentState = BossStates.Attack1;
                        }

                        if (distanceToPlayer < Radius/2 && attack1Counter >= amountOfAttack1)
                        {
                            StartCoroutine("Cooldown","Attack1");
                        }

                        if (distanceToPlayer < Radius / 2 && attack1Counter >= amountOfAttack1)
                        {
                            StartCoroutine("Cooldown", "Attack2");
                        }

                        hits = null;
                       
                    }    
                }
            }

            if(Physics.Raycast(new Ray(transform.position, -transform.up), 1f, mask))
            {
                canJump = true;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Attack1()
    {

        if (attack1Counter > amountOfAttack1) { return;}

        canMove = false;

        attack1Counter++;

        //using transform.up to make sure the bullet instances above the ground
        GameObject tempObj = Instantiate(bullet, gun.transform.position + transform.forward, Quaternion.identity) as GameObject;

        tempObj.transform.LookAt(gun.transform.position + gun.transform.forward*2);
    }

    public void Attack2()
    {
        
    }

    public void Attack3()
    {

    }

    public void MoveRandomDirection()
    {
        Vector3[] walkDirs = new Vector3[] {transform.forward,-transform.forward,player.transform.position-transform.position};
        controller.AddForce((walkDirs[Random.Range(0, 2)]).normalized * speed,ForceMode.Force);
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }


    public void MoveToPlayer(Transform target)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + transform.forward * 1.1f, -transform.up), out hit, 10f))
        {

            if (hit.transform.tag == "Ground")
            {
   
                    if (!Physics.Raycast(transform.position, transform.forward, 0.5f))
                    {

                        walkDirection = target.position - transform.position;
                        walkDirection.y = 0;

                        velocity = walkDirection.normalized;

                        if (Mathf.Abs(walkDirection.magnitude) > rangeToStop)
                        {

                            velocity = walkDirection.normalized * speed * 1.5f;

                        }
                        velocity.y = 0;

                        if (player)
                        {
                            //using sphere cast to fake damage on collision
                            if (Mathf.Abs(Vector3.Distance(target.transform.position, transform.position)) < rangeToStop)
                            {
                                player.gameObject.GetComponent<PlayerBase>().TakeDamage(meleeDamage);
                            }
                        }

                }

                else { canMove = false; }
            }
        }
    }

    public void MoveUp()
    {
        canJump = false;

        controller.velocity = new Vector3(0, 0, controller.velocity.z);
        controller.AddForce(new Vector3(0, height, controller.velocity.z), ForceMode.Impulse);
    }

    public void TakeDamage(float damage)
    {
        if (hp < 0)
        {
            ItemDatabase.DropItem(transform.position, transform.name);
            Destroy(gameObject);
        }

        hp -= damage;

        if (hp <= 0)
        {
            ItemDatabase.DropItem(transform.position, transform.name);
            Destroy(gameObject);
        }
    }

    public IEnumerator Cooldown(string skillName)
    {
        if(skillName == "Attack1" && attack1Cooldown) { yield break;}
        if(skillName == "Attack2" && attack2Cooldown) { yield break;}

        attack1Cooldown = true;
        attack2Cooldown = true;

        switch (skillName)
        {
            case "Attack1":
                yield return new WaitForSeconds(2f);
                attack1Counter = 0;
                attack1Cooldown = false;
                canMove = true;
                break;
            case "Attack2":
                yield return new WaitForSeconds(5f);
                attack2Counter = 0;
                attack2Cooldown = false;
                canMove = true;
                break;
        }

        yield return null;
    }
}
