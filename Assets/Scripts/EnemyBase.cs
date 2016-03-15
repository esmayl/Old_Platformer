using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEngine.UI;

public enum EnemyStates { Idle,Patrol,Attack,AttackAtPlayer}
public enum EnemyTypes { Melee,Ranged,FlyingMelee,FlyingRanged}

[RequireComponent(typeof(Rigidbody))]
public class EnemyBase : MonoBehaviour {

    public EnemyTypes enemyType = EnemyTypes.Ranged;
    public EnemyStates currentState = EnemyStates.Patrol;
    public GameObject damageDealer;
    public GameObject gun;
    public Material enemyMaterial;
    [HideInInspector]
    public GameObject player;

    public float hp = 100;
    public float speed = 3;
    public int meleeDamage = 5;
    public float meleeRange = 1;
    public float range = 15;

    internal float rangeToStop = 0.5f;
    internal Vector3 walkDirection;
    internal bool canGoForward = false;                                                                                                    

    //Pathfinding variables
    internal Vector3 velocity;
    internal RaycastHit hit;

    Vector3 target;
    Rigidbody controller;
    float height;

    public virtual void Start()
    {
        if(Physics.Raycast(new Ray(transform.position,-transform.up),out hit,10f))
        {
            if (hit.transform.tag == "Ground")
            {
                transform.position = hit.point+transform.up/3.5f;
            }
        }
        controller = GetComponent<Rigidbody>();
        StartCoroutine("DetectPlayer", range);
    }

	public virtual void Update ()
    {
        if (player)
        {
            if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < meleeRange)
            {
                player.GetComponent<PlayerMovement>().TakeDamage(meleeDamage);
                velocity = Vector3.zero;

            }
        }

        if (canGoForward)
        {
            controller.velocity = velocity;
        }

        if (hp < 0)
        {
            ItemDatabase.DropItem(transform.position, transform.name);
            Destroy(gameObject);
            Debug.LogError("");
        }

    }

    public void TakeDamage(float damage)
    {
        if (hp < 0)
        {
            ItemDatabase.DropItem(transform.position, transform.name);
            Destroy(gameObject);
        }
        hp -= damage;
        StartCoroutine("DamageFlash");  
    }

    public IEnumerator DamageFlash()
    {
        enemyMaterial = transform.Find(gameObject.name).GetComponent<Renderer>().material;
        Color tempColor = enemyMaterial.color;
        enemyMaterial.color = Color.black;

        while (enemyMaterial.color.b < 255f)
        {
            enemyMaterial.color += new Color(8, 8, 8, 8);
            yield return new WaitForEndOfFrame();
        }
        enemyMaterial.color = tempColor;

        if (hp <= 0)
        {
            ItemDatabase.DropItem(transform.position, transform.name);
            Destroy(gameObject);
        }
        StopCoroutine("DamageFlash");
    }
    public virtual void Jump() { }


    //doesnt move in y direction, detects path on its own by raycasting forward and at 1.5 times forward in the down direction
    public virtual void Patrol()
    {
        velocity.y =0;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + transform.forward * 1.1f,-transform.up),out hit,10))
        {
            if (hit.transform.tag == "Ground")
            {
                if (hit.distance < 0.4f)
                {
                    if (!Physics.Raycast(transform.position, transform.forward, 1f))
                    {
                        canGoForward = true;
                        target = transform.position + transform.forward;
                        walkDirection = target - transform.position;
                        if (Mathf.Abs(walkDirection.magnitude) > rangeToStop)
                        {
                            velocity = walkDirection.normalized * speed;
                            transform.LookAt(transform.position + transform.forward);
                        }
                    }
                    else
                    {
                        canGoForward = false;
                        transform.LookAt(transform.position - transform.forward);
                    }

                }
                if (hit.distance > 0.41f)
                {
                    canGoForward = false;
                    transform.LookAt(transform.position-transform.forward);
                }
            }
        }
        else
        {
            canGoForward = false;
        }
    }

    public void MoveToPlayer(Transform target) 
    {

        Renderer test = transform.GetChild(0).GetComponent<Renderer>();
        Material testMat = test.material;
        testMat.SetColor("_EmissionColor", Color.red);

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + transform.forward * 1.1f, -transform.up), out hit, 10f))
        {
            if (hit.transform.tag == "Ground")
            {
                if (hit.distance < 0.4f)
                {
                    if (!Physics.Raycast(transform.position, transform.forward,0.5f))
                    {
                        canGoForward = true;
                        walkDirection = target.position - transform.position;
                        walkDirection.y = 0;
                        velocity = controller.velocity;

                        if (Mathf.Abs(walkDirection.normalized.magnitude) > rangeToStop)
                        {
                            velocity = walkDirection.normalized * speed*1.5f;

                        }
                        velocity.y = 0;

                        if (player)
                        {
                            //using sphere cast to fake damage on collision
                            if (Mathf.Abs(Vector3.Distance(target.transform.position, transform.position)) < meleeRange)
                            {
                                player.gameObject.GetComponent<PlayerMovement>().TakeDamage(meleeDamage);
                            }
                        }
                    }
                    else
                    {
                        canGoForward = false;
                        transform.LookAt(transform.position - transform.forward);
                    }

                }
                if (hit.distance > 0.41f)
                {
                    canGoForward = false;
                    transform.LookAt(transform.position - transform.forward);
                }
                else { return; }
            }
        }
        else
        {
            canGoForward = false;
        }

    }

    public IEnumerator Idle()
    {
        yield return new WaitForSeconds(0.5f);
        currentState = EnemyStates.Patrol;
    }

    public virtual void Attack(Vector3 Direction) 
    {

        if (!player)
        {
            currentState = EnemyStates.Idle;
            return;
        }

        if (Mathf.Abs(player.transform.position.y - transform.position.y) < 1.5f)
        {
            velocity = Vector3.zero;

            Vector3 flattendPos = player.transform.position;
            flattendPos.y = transform.position.y;
            transform.LookAt(flattendPos);
            
            //using transform.up to make sure the bullet instances above the ground
            if (gun) 
            {
                GameObject tempObj = Instantiate(damageDealer, gun.transform.position + transform.forward*0.5f, Quaternion.identity) as GameObject;
                tempObj.transform.LookAt(gun.transform.position +transform.forward);
            }
            else
            {
                GameObject tempObj = Instantiate(damageDealer, transform.position + transform.forward * 2 + transform.up / 3.5f, Quaternion.identity) as GameObject;
                tempObj.transform.LookAt(transform.position + transform.forward * 2.5f + transform.up / 3.5f);
            }
        }
    }

    public virtual IEnumerator DetectPlayer(float Radius) 
    {
        while (true)
        {

            //==============================UPDATE========================================//
            Collider[] hits;
            hits = Physics.OverlapSphere(transform.position, Radius);
            if (hits.Length > 0)
            {
                foreach (Collider h in hits)
                {
                    if (h.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                            if (h.transform.tag == "Player")
                            {
                                player = h.gameObject;
                                Vector3 playerPos = player.transform.position;
                                playerPos.y = transform.position.y;
                                transform.LookAt(playerPos);
                                currentState = EnemyStates.AttackAtPlayer;

                                hits = null;
                            }
                            if (h.transform.tag == "Ground")
                            {
                                currentState = EnemyStates.Patrol;
                                hits = null;
                            }
                  
                    }
                }
            }
           
            if (player)
            {
                if (Vector3.Distance(player.transform.position, transform.position) > range)
                {
                    player = null;
                    transform.LookAt(transform.position - transform.forward);
                    currentState = EnemyStates.Patrol;
                }
            }

            switch (currentState)
            {
                case EnemyStates.Patrol:
                    Patrol();
                    break;
                case EnemyStates.Idle:
                    velocity = Vector3.zero;
                    StartCoroutine("Idle");
                    break;
                case EnemyStates.Attack:
                    Attack(transform.forward);
                    break;
                case EnemyStates.AttackAtPlayer:
                    MoveToPlayer(player.transform);
                    break;

                default:
                    Idle();
                    break;
            }


            yield return new WaitForSeconds(0.4f);
        }
    }


    /// <summary>
    /// only use in air AKA flying enemy
    /// </summary>
    public virtual void MoveUp() { }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 255, 255, 0.5f);

        Gizmos.DrawSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.forward * 1.1f, -transform.up);
    }
}
