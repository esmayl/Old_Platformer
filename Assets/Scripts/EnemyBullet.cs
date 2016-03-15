using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{

    public AudioSource bulletSound;
    public ParticleSystem particle;
    internal float baseDamage = 1f;
    public float weaponDamage = 1f;
    public float lifeTime = 5;
    public int bulletSpeed = 5;
    public Transform player;
    public float waitTime = 2;

    Rigidbody controller;
    float timer;


    void Start()
    {
        controller = GetComponent<Rigidbody>();
    }

    public virtual void FixedUpdate()
    {
        if (timer < waitTime)
        {
            controller.velocity = transform.forward * bulletSpeed;
            timer += Time.fixedDeltaTime;
        }
        else
        {
            if (player)
            {
                GoToTarget(player);
            }
            Destroy(gameObject);
        }

    }
    public virtual void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if(coll.gameObject.GetComponent<PlayerMovement>().TakeDamage(Mathf.FloorToInt(baseDamage * weaponDamage)))
            {
                Destroy(gameObject);
            }
        }
        if (coll.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
        if (coll.gameObject.tag == "Boss")
        {
            Destroy(gameObject);
        }
    }

    public void GoToTarget(Transform target)
    {
        transform.LookAt(target);
        controller.velocity = (target.position - transform.position).normalized * bulletSpeed;
    }

    public virtual IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }

    public void AddChargeDmg(float chargeDmg)
    {
        weaponDamage += chargeDmg * 10;
    }

}
