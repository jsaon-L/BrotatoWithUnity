
using UnityEngine;
using UnityGameFramework.Runtime;



public class WeaponEntityLogic : EntityLogic
{

    protected float _currentAttackTime;

    protected bool CanAttack = true;


    //冷却
    public float ColdDown = 0.05f;
    //攻击范围
    public float AttackRange = 3;
    //瞄准范围
    public float AimingRange = 4;

    public int Damage = 1;

    public Transform TargetEnemy;
    public float TargetEnemyDistance;



    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        FindEnemy();

        LookTarget();

        if (CanAttack&& TargetEnemyDistance<= AttackRange)
        {
            _currentAttackTime += elapseSeconds;

            if (_currentAttackTime >= ColdDown)
            {
                Attack();
                _currentAttackTime = 0;
            }
        }
    }


    public virtual void FindEnemy()
    {
        var latelyEnemy = EnemyManager.Instance().GetLatelyEnemy(GetWordPosition());

        if (latelyEnemy.enemy != null)
        {
            TargetEnemy = latelyEnemy.enemy.transform;
            TargetEnemyDistance = latelyEnemy.distance;
        }
        else
        {
            TargetEnemy = null;
            TargetEnemyDistance = float.MaxValue;
        }
    }


    public virtual void Attack()
    {

    }


    public virtual void LookTarget()
    {
        if (TargetEnemy&& TargetEnemyDistance<= AimingRange)
        {
            transform.right = (TargetEnemy.transform.position - transform.position).normalized;
        }
        else
        {
            transform.localRotation = Quaternion.identity;
        }
    }
    
    public Vector2 GetWordPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
}
