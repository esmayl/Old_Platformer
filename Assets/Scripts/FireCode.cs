using UnityEngine;
using System.Collections;

public class FireCode : Bullet {

    public AudioClip napalmClip;

    RaycastHit hit;
    float globalTimer = 3;
    float globalCounter = 0;
    float tickTimer = 0.8f;
    float counter = 0;

    public override void Start()
    {
    }


    public void Update()
    {
        if (globalCounter < globalTimer)
        {
            if (counter > tickTimer)
            {
                transform.SendMessageUpwards("TakeDamage", baseDamage * weaponDamage);
                counter = 0;
            }
            else
            {
                counter += Time.deltaTime;
                globalCounter += Time.deltaTime;
            }
        }
        else
        {
            shotSource.Stop();
            Destroy(gameObject);
        }
    }

    public void OnDestroy()
    {
        shotSource.Stop();
    }
}
