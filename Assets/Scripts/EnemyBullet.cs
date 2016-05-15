    using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{

    internal float baseDamage = 1f;
    public float weaponDamage = 1f;
    public int bulletSpeed = 5;
    public Transform player;

    Rigidbody controller;
    float timer;
    float waitTime = 2;
    float lifeTime = 5;

    void Start()
    {
        controller = GetComponent<Rigidbody>();
        timer = 0;
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
            if(coll.gameObject.GetComponent<PlayerBase>().TakeDamage(Mathf.FloorToInt(baseDamage * weaponDamage)))
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
