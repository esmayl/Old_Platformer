using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Buster : Power 
{
    public int amountOfBullets = 5;
    public GameObject bullet;
    GameObject bulletInstance;
    internal int chargeLevel;

    float shootTimer = 0;
    float shootWaitTime = 0.25f;
    bool canShoot = true;
    GameObject temp;

	public override void Start () {
        transform.name = "BusterPower";
        
        base.Start();
        
	}

    public override void Attack(Transform player)
    {
        if (!canShoot) { Debug.Log("Catch!"); return; }

        canShoot = false;

        if (shotSound)
        {
            if (shotSource)
            {
                shotSource.clip = shotSound;
                shotSource.Play();
            }
        }

        gun = player.GetComponent<PlayerMovement>().gun;


        Vector3 start = gun.transform.position;

        temp = Instantiate(bullet,start,Quaternion.identity) as GameObject;
        temp.transform.LookAt(gun.transform.position + gun.transform.forward);

        StartCoroutine("TimeCounter");

        //Add charge dmg and edit size to fit charge lvl
        if (chargeLevel >= 2)
        {
            temp.GetComponent<Bullet>().weaponDamage = chargeLevel * temp.GetComponent<Bullet>().weaponDamage;
            temp.GetComponent<Bullet>().shotSource = shotSource;
            if (chargeLevel / 3 < 1)
            {
                temp.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            }
            else
            {
                temp.transform.localScale = new Vector3(chargeLevel / 3, chargeLevel / 3, chargeLevel / 3);
            }
        }
        else
        {
            temp.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }



        temp.transform.parent = transform;
		temp.GetComponent<Bullet> ().player = player.gameObject;

    }

    public IEnumerator TimeCounter()
    {

       while(shootTimer<=shootWaitTime)
       {
            shootTimer += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
       }
       if (shootTimer >= shootWaitTime)
       {
           canShoot = true;
           shootTimer = 0;
           yield return null;
       }
    }
    
}
