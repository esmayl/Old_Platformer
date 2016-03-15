using UnityEngine;
using System.Collections;

public class ItemFunctions : MonoBehaviour {

    public GameObject pickupParticle;
    public ItemInfo item = new ItemInfo();

    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, -transform.up), out hit, 10f))
        {
            if (hit.transform.tag == "Ground")
            {
                transform.position = hit.point;
            }
        }
        item.startAnimation = true;

        item.pickedUp = false;

        if (item.mesh)
        {
            item.mesh.GetComponent<Renderer>().material = item.meshMaterial;
        }
    }

    void Update()
    {

        transform.Rotate(Vector3.up, 5,Space.Self);

        if (item.mesh)
        {
            if (item.mesh.GetComponent<Renderer>().material.color.a <= 0.01f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && !item.pickedUp)
        {
            if (col.GetComponent<PlayerMovement>())
            {
                col.GetComponent<PlayerMovement>().UseItem(item);
                Camera.main.GetComponentInParent<LevelCamera>().AddScore();
                transform.GetComponent<Collider>().enabled = false;
                StartCoroutine("PickedUp");
                item.pickedUp = true;
            }
        }
    }

    public IEnumerator PickedUp()
    {
        GameObject tempParticle =null;
        if (pickupParticle)
        {
            tempParticle = Instantiate(pickupParticle, transform.position, Quaternion.identity) as GameObject;
            tempParticle.SetActive(true);
        }
        while (item.mesh.GetComponent<Renderer>().material.color.a > 0.01f)
        {
            item.mesh.GetComponent<Renderer>().material.color -= new Color(0, 0, 0, 0.05f);
            yield return new WaitForEndOfFrame();
        }

        Destroy(tempParticle);
        yield return null;
    }
}
