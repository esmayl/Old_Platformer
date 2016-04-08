using UnityEngine;
using System.Collections;

public class IceSpike : Bullet {

    public AudioClip explosionClip;
    public int amountOfIce;
    public float height = 10;

    internal int currentAmountOfIce;

    float iceStartDistance = 1.3f;
    float iceSpacing = 0.25f;
    float iceDistance = 1f;
    Vector3 iceDirection;
    Vector3 icePos;
    RaycastHit rayHit;
    Rigidbody rb;

    RaycastHit hit;

    float timer = 0;
    float counter = 0;


    public override void Start()
    {
        StartCoroutine("DeathTimer");

        var layermask = 1<<LayerMask.NameToLayer("Enemy");
        layermask |= 1 << gameObject.layer;
        layermask = ~layermask;

        if (Physics.Raycast(new Ray(transform.position + Vector3.up * height, -Vector3.up), out hit, height * 2,layermask)) 
        {
            Debug.Log("sdfsdf");
            var p = transform.position + hit.point;
            if (hit.distance >4) { Destroy(gameObject); }
            else
            {
                transform.position = p / 2;
            }
        }
        transform.localScale = new Vector3(1, 1, 0.1f);
        StartCoroutine("SizeUp");
    }
    public override void FixedUpdate()
    {

    }

    public override void OnCollisionEnter(Collision coll)
    {

    }

    public IEnumerator SizeUp()
    {
        int counter = 0;
        while (transform.localScale.z < Random.Range(0.2f,0.4f))
        {
            transform.localScale += new Vector3(0, 0, Mathf.Sin(0.01f*counter));
            yield return new WaitForSeconds(0.01f);
            counter++;
        }
        currentAmountOfIce++;
        CreateNew();
        if (transform.localScale.z < 0.2f) { Destroy(gameObject); }
        yield return null;
    }

    public void CreateNew()
    {
        if (currentAmountOfIce < amountOfIce)
        {
            GameObject t = Instantiate(gameObject, transform.position + transform.right*Random.Range(0.5f,1.5f), transform.rotation) as GameObject;
            t.GetComponent<IceSpike>().amountOfIce = amountOfIce;
            t.GetComponent<IceSpike>().currentAmountOfIce = currentAmountOfIce;
            t.transform.parent = transform.parent;
        }
        else
        {
            Destroy(transform.parent.gameObject);
            return;
        }
    }

    public void OnTriggerEnter(Collider coll)
    {

        if (coll.tag == "Enemy")
        {
            coll.gameObject.GetComponent<EnemyBase>().TakeDamage(baseDamage * weaponDamage);
            StartCoroutine("DeathTimer");
        }
        if (coll.tag == "Ground")
        {
            StartCoroutine("DeathTimer");
        }
    }
}
