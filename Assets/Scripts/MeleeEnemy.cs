using UnityEngine;
using System.Collections;

public class MeleeEnemy : EnemyBase
{

	public override void Start ()
    {
        base.Start();
    }
	
    public override void Update()
    {
        base.Update();
    }

    public override void Patrol()
    {
        base.Patrol();
        
        Renderer test = transform.GetChild(0).GetComponent<Renderer>();
        Material testMat = test.material;
        testMat.SetColor("_EmissionColor", Color.green);
    }

    public override void Attack(Vector3 Direction)
    {
        base.Attack(Direction);
    }
}
