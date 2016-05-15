using UnityEngine;
using System.Collections;

public class MiniBoss : BossBase {

    public GameObject rocket;

	public override void Start () {
        base.Start();

	}

    public void Attack2()
    {
        attack2Counter++;

        pos = new Vector3[] {transform.position + transform.forward * 2+transform.up *1.5f, transform.position + transform.forward*2 + transform.up * 2};

            foreach (Vector3 v in pos)
            {
                GameObject temp = (GameObject)Instantiate(rocket, v, Quaternion.identity);
                temp.transform.LookAt((v - transform.position) + v);

                //make boss audio channels (Bullet shot,Bullet explosion,moving,jumping) using own audioSource
                temp.GetComponent<Bullet>().bulletSound = player.transform.FindChild("AttackSource 3").GetComponent<AudioSource>();
            }
    }
}
