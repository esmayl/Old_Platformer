using UnityEngine;
using System.Collections;

public class FireBomb : Bullet {

    public AudioClip explosionClip;
    public GameObject particle2;
    GameObject explosion;
    float fireCounter;

	public override void Start ()
    {
        StartCoroutine("AutoExplosion");
        controller = GetComponent<Rigidbody>();
	}

    void Update()
    {
        transform.Rotate(transform.forward, 5f);
    }
    public IEnumerator AutoExplosion()
    {
        yield return new WaitForSeconds(lifeTime);
        explosion = Instantiate(hitParticle, transform.position, Quaternion.identity) as GameObject;
        if (bulletSound)
        {
            if (explosionClip)
            {
                shotSource.Stop();
                bulletSound.clip = explosionClip;
                bulletSound.Play();
            }
        }
        Destroy(gameObject);
    }

    public void DoExplosion()
    {
        StopCoroutine("AutoExplosion");
        explosion = Instantiate(hitParticle, transform.position, Quaternion.identity) as GameObject;
        if (bulletSound)
        {
            if (explosionClip)
            {
                shotSource.Stop();
                bulletSound.clip = explosionClip;
                bulletSound.Play();
            }
        }
        Destroy(gameObject);
    }

    public override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" && gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            if (bulletSound)
            {
                if (explosionClip)
                {
                    shotSource.Stop();
                    bulletSound.clip = explosionClip;
                    bulletSound.Play();
                }
            }
            explosion = Instantiate(particle2, transform.position, Quaternion.identity) as GameObject;
            col.gameObject.GetComponent<EnemyBase>().TakeDamage(baseDamage * weaponDamage);
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "Ground")
        {
            if (bulletSound)
            {
                if (explosionClip)
                {
                    shotSource.Stop();
                    bulletSound.clip = explosionClip;
                    bulletSound.Play();
                }
            }
            explosion = Instantiate(hitParticle, transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject);
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (bulletSound)
            {
                if (explosionClip)
                {
                    shotSource.Stop();
                    bulletSound.clip = explosionClip;
                    bulletSound.Play();
                }
            }
            explosion = Instantiate(particle2, transform.position, Quaternion.identity) as GameObject;
            col.gameObject.GetComponent<PlayerMovement>().TakeDamage(Mathf.FloorToInt(baseDamage * weaponDamage));
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "Boss") 
        {

            if (bulletSound)
            {
                if (explosionClip)
                {
                    shotSource.Stop();
                    bulletSound.clip = explosionClip;
                    bulletSound.Play();
                }
            }
            explosion = Instantiate(hitParticle, transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject);
            
        }
    }
}
