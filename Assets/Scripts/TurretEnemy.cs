using UnityEngine;
using System.Collections;

public class TurretEnemy : EnemyBase {

    public GameObject barrel;
    private Animation animController;

    public override void Start()
    {
        base.Start();
        currentState = EnemyStates.Attack;
        animController = GetComponent<Animation>();
        rangeToStop = range;
    }

    public override void Update()
    {
        base.Update();
    }

    public override IEnumerator DetectPlayer(float Radius)
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
                    if (h.gameObject)
                    {
                        if (h.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            if (Physics.Raycast(new Ray(transform.position, h.transform.position - transform.position), out hit))
                            {
                                Debug.Log(hit.transform.name);
                                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                                {
                                    
                                    animController.Play();
                                    yield return new WaitForSeconds(1f);
                                    animController.enabled = false;

                                    player = h.gameObject;
                                    Vector3 playerPos = player.transform.position;
                                    currentState = EnemyStates.Attack;
                                    hits = null;
                                }
                            }
                        }
                    }
                }
            }
            if (player)
            {
                if (Vector3.Distance(player.transform.position, transform.position) > Radius)
                {
                    player = null;
                }
            }

            switch (currentState)
            {
                case EnemyStates.Idle:
                    velocity = Vector3.zero;
                    StartCoroutine("Idle");
                    break;
                case EnemyStates.Attack:
                    Attack();
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Attack()
    {

        // doesnt use direction
        if (!player)
        {
            currentState = EnemyStates.Idle;
            return;
        }

        base.Attack();
            //barrel.transform.LookAt(player.transform.position+transform.up/2);
            //
            ////using transform.up to make sure the bullet instances above the ground
            //GameObject tempObj = Instantiate(damageDealer, transform.position + barrel.transform.forward*1.5f + transform.up*1.5f , Quaternion.identity) as GameObject;
            //tempObj.transform.LookAt(player.transform.position + transform.up / 2);
            //tempObj.GetComponent<Rigidbody>().velocity = barrel.transform.forward;
            //tempObj = null;
    }

    public override void Patrol()
    {
        //cause a turret can't patrol :D
    }
}
