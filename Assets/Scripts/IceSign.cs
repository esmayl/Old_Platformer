using UnityEngine;
using System.Collections;

public class IceSign : MonoBehaviour {

    public GameObject ice;

	// Use this for initialization
	void Start () {
        transform.localScale = Vector3.one * 0.1f;
        StartCoroutine("SizeUp");
	
	}

    public IEnumerator SizeUp()
    {
        while (transform.localScale.y < 0.5)
        {
            transform.localScale += Time.deltaTime *3.5f* Vector3.one;
            yield return new WaitForSeconds(0.025f);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        DoAttack();
        yield return null;
    }

    void DoAttack()
    {
        GameObject t;
        t = Instantiate(ice,transform.position+transform.forward/2,Quaternion.identity)as GameObject;
        t.transform.LookAt(Vector3.up+t.transform.position);
        t.transform.Rotate(t.transform.up, 90);
        t.transform.parent = transform;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
