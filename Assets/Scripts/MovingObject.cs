using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class MovingObject : MonoBehaviour {

    public Vector3 start;
    public Vector3 end;
    public float speed = 3;
    float rangeToStop = 0.5f;

    public void Reset()
	{
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        gameObject.tag = "MovingObject";
		if (gameObject.isStatic) 
		{
			gameObject.isStatic = false;
		}
	}

	void Update () 
    {
        Vector3 walkDirection = end - transform.position;

        if (Mathf.Abs(walkDirection.magnitude) > rangeToStop)
        {
            GetComponent<Rigidbody>().velocity = walkDirection.normalized * speed;
        }
        else
        {
            Vector3 temp = start;
            start = end;
            end = temp;
        }
        
	}

    public void OnCollisionStay(Collision col)
    {
        if (col.transform.tag == "Player")
        {
        }
    }

}
