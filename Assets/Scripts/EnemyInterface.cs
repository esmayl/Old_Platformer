using UnityEngine;
using System.Collections;

public interface EnemyInterface
{
    IEnumerator Idle();
    void Attack();
    void TakeDamage(float damage);
    void Jump();
    void Patrol();
    void MoveToPlayer(Transform target);
}
