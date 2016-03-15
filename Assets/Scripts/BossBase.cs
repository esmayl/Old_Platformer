using UnityEngine;
using System.Collections;

public enum BossStates { Moving,MovingToPlayer,Attack1,Attack2,Attack3,Jump}

public class BossBase : MonoBehaviour {


    internal RaycastHit hit;
    internal GameObject player;
    internal BossStates currentState;
    internal Vector3 walkDirection;
    internal Vector3 velocity;
    internal CharacterController controller;

    public GameObject landParticle;
    public GameObject gun;
    public GameObject bullet;
    public float speed = 5;
    public int meleeDamage = 5;
    public int range = 10;
    public float rangeToStop = 10f;
    public int height = 3;
    public float hp = 100;

    internal Vector3[] pos;
    internal int attack1Counter = 0;
    internal int attack2Counter = 0;
    internal int attack3Counter = 0;

    private float beginX;
    private bool landed = false;

	public virtual void Start () {
        landParticle.SetActive(false);

        beginX = transform.position.x;
        controller = GetComponent<CharacterController>();
        StartCoroutine("DetectPlayer", range);
	}
	
	public void Update () {
        if (player)
        {
            gun.transform.LookAt(player.transform.position + transform.up);
        }
	}

    public IEnumerator Land()
    {
        if (landed) { yield return null; }
        Debug.Log("");

        landed = true;
        landParticle.SetActive(true);
        yield return new WaitForSeconds(landParticle.GetComponent<ParticleSystem>().duration);
        landParticle.GetComponent<ParticleSystem>().Stop();
        landParticle.SetActive(false);
    }

    public virtual IEnumerator DetectPlayer(float Radius)
    {
        while (true)
        {
                if (controller.isGrounded)
                {
                    if (!landed)
                    {
                        StartCoroutine("Land");
                    }
                }

            if (transform.position.x != beginX) { transform.position = new Vector3(beginX, transform.position.y, transform.position.z); }
            //==============================UPDATE========================================//

            Collider[] hits;
            hits = Physics.OverlapSphere(transform.position, range);
            if (hits.Length > 0)
            {
                foreach (Collider h in hits)
                {
                    if (h.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        player = h.gameObject;
                        Vector3 playerPos = player.transform.position;
                        playerPos.y = transform.position.y;

                        if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < range && Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) > range / 1.5f)
                        {
                            currentState = BossStates.MovingToPlayer;
                            transform.LookAt(playerPos);
                        }

                        if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < range / 2) { currentState = BossStates.Jump; }
                        
                        if (player)
                        {
                            //using sphere cast to fake damage on collision
                            //if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < rangeToStop)
                            //{
                            //    player.gameObject.GetComponent<PlayerMovement>().TakeDamage(meleeDamage);
                            //}
                        }
                        hits = null;
                       
                    }
                }
            }
            switch (currentState)
            {
                case BossStates.Moving:
                    break;
                case BossStates.Jump:
                    MoveUp();
                    break;
                case BossStates.Attack1:
                    Attack1();
                    break;
                case BossStates.Attack2:
                    Attack2();
                    MoveRandomDirection();
                    break;
                case BossStates.Attack3:
                    break;
                case BossStates.MovingToPlayer:
                    MoveToPlayer(player.transform);
                    break;

                default:
                    break;
            }

            


            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Attack1()
    {
        attack1Counter++;

        
        //using transform.up to make sure the bullet instances above the ground
        GameObject tempObj = Instantiate(bullet, gun.transform.position+transform.forward, Quaternion.identity) as GameObject;
        tempObj.transform.LookAt(gun.transform.position+gun.transform.forward*2);
        tempObj.GetComponent<Bullet>().bulletSound = player.transform.FindChild("AttackSource 3").GetComponent<AudioSource>();
        //tempObj.GetComponent<SphereCollider>().isTrigger = true;
    }
    public virtual void Attack2(){}
    

    public void MoveRandomDirection()
    {
        Vector3[] walkDirs = new Vector3[] {gun.transform.forward,-gun.transform.forward};
        controller.Move(((walkDirs[Random.Range(0, walkDirs.Length - 1)].normalized * speed)));
    }


    public void MoveToPlayer(Transform target)
    {
        walkDirection = target.position - transform.position;
        //floating enemy?
        //walkDirection.y = 0;
        velocity = controller.velocity;

        if (walkDirection.magnitude > rangeToStop)
        {
            velocity = walkDirection.normalized * speed;
        }
        velocity.x = 0;
        controller.SimpleMove(velocity);
        transform.LookAt(new Vector3(target.position.x,transform.position.y,target.position.z));
    }

    public void MoveUp()
    {
        velocity = GetComponent<CharacterController>().velocity;

        velocity.y += height*3;
        controller.Move(velocity);
        velocity.y = 0; 
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
}
