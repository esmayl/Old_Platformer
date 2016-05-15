using UnityEngine;
using System.Collections;

public class SpecialBullet : EnemyBullet
{

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);
    }

    public override IEnumerator DeathTimer()
    {
        return base.DeathTimer();
    }

}
