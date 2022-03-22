using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : Tower {
    public GameObject IcePre;
    private void GetNonFrozenTarget()
    {
        foreach (Enemy enemy in getEnemiesInAggroRange())
        {
            if (!enemy.frozen)
            {
                targetEnemy = enemy;
                break;
            }
        }
    }
    protected override void attackEnemy()
    {
        base.attackEnemy();
        GameObject Ice = (GameObject)Instantiate(IcePre, towerPieceToAim.position, Quaternion.identity);
        Ice.GetComponent<FollowingProjectile>().enemyToFollow = targetEnemy;
    }
    public override void Update()
    {
        base.Update();
        GetNonFrozenTarget();
    }
}
