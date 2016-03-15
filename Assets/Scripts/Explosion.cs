using UnityEngine;
using System.Collections;

public class Explosion : Bullet
 {
    void Start()
    {
        lifeTime = 2;
        StartCoroutine("DeathTimer");
    }

    public override void FixedUpdate()
    {
        //explosion dont need to move
    }

    public override void OnCollisionEnter(Collision coll)
    {
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Enemy") && gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Debug.LogError("object dmg - "+weaponDamage+" = " +other.name);
            other.SendMessage("TakeDamage",baseDamage *weaponDamage);
        }
    }
}
