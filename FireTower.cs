using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    public GameObject fireParticlesPreFab;

    protected override void attackEnemy()
    {
        base.attackEnemy();
        GameObject particles = (GameObject)Instantiate(fireParticlesPreFab, 
            transform.position + new Vector3(0, .5f),
            fireParticlesPreFab.transform.rotation);

        particles.transform.localScale *= aggroRadious / 10f;

        foreach(Enemy enemy in EnemyManager.Instance.GetEnemiesInRange(transform.position, aggroRadious))
        {
            enemy.TakeDamage(attackPower);
        }
    }
}
