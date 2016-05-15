using UnityEngine;
using System.Collections;

public class DestroyAllTouching : MonoBehaviour {

    public void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Player")
        {
            Debug.Log(col.gameObject.name);
            col.gameObject.GetComponent<PlayerBase>().TakeDamage(100);
        }
    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawIcon(transform.position, "Death.bmp",true);
    }
}
