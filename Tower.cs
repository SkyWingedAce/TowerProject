using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType { Stone, Fire, Ice }
public class Tower : MonoBehaviour {

    public float attackPower = 3f;
    public float timeBetweenAttacksInSeconds = 1f;
    public float aggroRadious = 15f;
    public int towerLevel = 1;

    public TowerType type;
    //public List<Enemy> debugList;
    public AudioClip shootSound;
    public Transform towerPieceToAim;
    public Enemy targetEnemy = null;
    private float attackCounter;

    private void SmoothlyLookAtTarget(Vector3 target)
    {
        towerPieceToAim.localRotation = UtilityMethods.SmoothlyLook(towerPieceToAim, target);

    }
    protected virtual void attackEnemy()
    {
        GetComponent<AudioSource>().PlayOneShot(shootSound, .15f);
    }
    public List<Enemy> getEnemiesInAggroRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach(Enemy enemy in EnemyManager.Instance.Enemies)
        {
            //print("Calling the manager");
            if(Vector3.Distance(transform.position,enemy.transform.position) <= aggroRadious)
            {
                enemiesInRange.Add(enemy);
            }
        }
        //debugList = enemiesInRange;
        return enemiesInRange;
    }
    public Enemy GetNearestEnemyInRange()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;

        foreach(Enemy enemy in getEnemiesInAggroRange())
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) < smallestDistance)
            {
                smallestDistance = Vector3.Distance(transform.position, enemy.transform.position);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    public virtual void Update()
    {
        attackCounter -= Time.deltaTime;

        if(targetEnemy == null)
        {
            if (towerPieceToAim)
            {
                SmoothlyLookAtTarget(towerPieceToAim.transform.position - new Vector3(0, 0, 1));

            }
            if(GetNearestEnemyInRange() != null && Vector3.Distance(transform.position, GetNearestEnemyInRange().transform.position) <= aggroRadious)
            {
                //print("Collecting  enemy in range");
                targetEnemy = GetNearestEnemyInRange();
            }
        } else
        {
            if (towerPieceToAim)
            {
                SmoothlyLookAtTarget(targetEnemy.transform.position);
            }
            if (attackCounter <= 0f)
            {
                attackCounter = timeBetweenAttacksInSeconds;
                attackEnemy();
                //attackCounter = timeBetweenAttacksInSeconds;
                
                //print(attackCounter);
            }

            if(Vector3.Distance(transform.position, targetEnemy.transform.position) > aggroRadious)
            {
                targetEnemy = null;
            }
        }
    }

    public void LevelUp()
    {
        towerLevel++;

        attackPower += 2;
        timeBetweenAttacksInSeconds *= 0.7f;
        aggroRadious *= 1.20f;
    }
    public void ShowTowerInfo()
    {
        UIManager.Instance.ShowTowerInfoWindow(this);
    }
}
