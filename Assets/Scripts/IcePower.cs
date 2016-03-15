using UnityEngine;
using System.Collections;

public class IcePower : Power 
{
    public GameObject iceObj;
    Vector3 iceDirection;
    GameObject explosion;
    RaycastHit hit;
    float iceSpeed = 200;
    float snowTextureAmount;
    bool instancing = false;

	public override void Start () 
    {
        base.Start();

        powerHolder = transform.gameObject;
        powerHolder.name = "IcePower";

	}
	
    public override void Attack(Transform player)
    {
        if (instance) { return; }

        if (shotSound)
        {
            if (shotSource)
            {
                shotSource.clip = shotSound;
                shotSource.Play();
            }
        }


        if (player.GetComponent<PlayerMovement>())
        {
            gun = player.GetComponent<PlayerMovement>().gun;
            iceDirection = gun.transform.forward;

        }
        else
        {
            return;
        }

        if (!instance)
        {

            instance = Instantiate(iceObj, gun.transform.position + iceDirection / 1.2f, Quaternion.identity) as GameObject;
            instance.transform.LookAt(gun.transform.position + (iceDirection * 1.1f));

        }
        else
        {
            Destroy(instance);

            instance = Instantiate(iceObj, gun.transform.position + iceDirection / 1.2f, Quaternion.identity) as GameObject;
            instance.transform.LookAt(gun.transform.position + (iceDirection * 1.1f));
            bulletSource.transform.position = instance.transform.position;
        }
    }

    public override bool Detonate()
    {
        if (instance)
        {
            //instance.GetComponent<IceSpike>().DoExplosion(instance.transform.position);
            return true;
        }

        return false;
    }

}
