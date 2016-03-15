using UnityEngine;
using System.Collections;

public class FirePower : Power {

    public GameObject fireObject;

    public override void Start()
    {
        base.Start();
    }

    public override void Attack(Transform player)
    {
        if (instance) { return; }

        gun = player.GetComponent<PlayerMovement>().gun;

        if (shotSound)
        {
            if (shotSource)
            {
                shotSource.clip = shotSound;
                shotSource.Play();
            }
        }

        Vector3 tempPos = gun.transform.position+gun.transform.forward*1.3f;
        tempPos.x = player.position.x;

        if (tempPos.y < player.position.y )
        {
            Debug.Log(tempPos);
            return;
        }
        else
        {
            instance = Instantiate(fireObject) as GameObject;
            instance.transform.position = tempPos;

            instance.transform.LookAt(gun.transform.position + (gun.transform.forward * 1.4f));

            instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * speed);

            bulletSource.transform.position = instance.transform.position;
            instance.GetComponent<FireBomb>().bulletSound = bulletSource;
            instance.GetComponent<FireBomb>().shotSource = shotSource;
       }
    }

    public override bool Detonate()
    {
        if (instance)
        {
            instance.GetComponent<FireBomb>().DoExplosion();
            return true;
        }

        return false;
    }
}
