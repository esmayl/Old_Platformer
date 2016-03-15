using UnityEngine;
using System.Collections;

public class SwitchToCollider : MonoBehaviour {


    void OnTriggerStay(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            col.transform.position = transform.position + (GetComponent<Collider>().bounds.extents.y *transform.up);
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
