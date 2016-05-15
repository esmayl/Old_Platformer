using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public AudioSource shotSource;
    public AudioSource bulletSound;
    public GameObject hitParticle;
    internal float baseDamage=1f;
    public float weaponDamage = 1f;
    public float lifeTime = 5;
    public int bulletSpeed = 5;
	public GameObject player;

    internal Rigidbody controller;


    public virtual void Start()
    {
        controller = GetComponent<Rigidbody>();
        hitParticle.SetActive(false);
        StartCoroutine("DeathTimer");
    }

    public virtual void FixedUpdate()
    {
			if (controller) 
			{
				controller.velocity = transform.forward * bulletSpeed;
                if (player)
                {
                    player.GetComponent<PlayerBase>().transition = false;
                }
			}

    }

    public virtual void OnCollisionEnter(Collision coll)
    {
        gameObject.GetComponent<Collider>().enabled = false;

        switch (coll.gameObject.tag)
        {
            case "Shield":
                if (gameObject.layer != LayerMask.NameToLayer("EnemyProjectiles"))
                {
                    Debug.Log("Hit shield");
                    Destroy(coll.gameObject.GetComponent<Collider>());
                    Destroy(GetComponent<Rigidbody>());
                }
                return;
            case "Enemy":
                if (gameObject.layer != LayerMask.NameToLayer("EnemyProjectiles"))
                {
                    Debug.Log("Hit enemy");

                    StartCoroutine("Hit", coll.contacts[0].point);
                    coll.gameObject.GetComponent<EnemyBase>().TakeDamage(baseDamage * weaponDamage);
                    Destroy(GetComponent<Rigidbody>());
                }
                return;
            case "Ground":
                StartCoroutine("Hit", coll.contacts[0].point);
                Destroy(GetComponent<Rigidbody>());
                return;
            case "Boss":
                if (gameObject.layer != LayerMask.NameToLayer("EnemyProjectiles"))
                {
                    Debug.Log("Hit boss");

                    StartCoroutine("Hit", coll.contacts[0].point);
                    coll.gameObject.GetComponent<BossBase>().TakeDamage(baseDamage * weaponDamage);
                    Destroy(GetComponent<Rigidbody>());
                }
                return;

        }

       if (gameObject.layer == LayerMask.NameToLayer("EnemyProjectiles"))
       {

            if (coll.gameObject.tag == "Player")
            {
                coll.gameObject.GetComponent<PlayerBase>().TakeDamage(Mathf.FloorToInt(baseDamage * weaponDamage));
                StartCoroutine("Hit", coll.contacts[0].point);
                Destroy(GetComponent<Rigidbody>());
            }
       }

    }

    public IEnumerator Hit(Vector3 particlePos)
    {

        StopCoroutine("DeathTimer");

        var t = Instantiate(hitParticle, particlePos, Quaternion.identity) as GameObject;
        t.SetActive(true);
        yield return new WaitForSeconds(1);
        Destroy(t);
        Destroy(gameObject);
    }

    public virtual IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
