using UnityEngine;
using System.Collections;

public interface BossInterface
{

    void Attack1();
    void Attack2();
    void Attack3();

    IEnumerator DetectPlayer(float Radius);

    void MoveToPlayer(Transform target);
    void MoveRandomDirection();

    void TakeDamage(float damage);
}
