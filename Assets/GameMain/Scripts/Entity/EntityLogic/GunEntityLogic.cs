
using UnityEngine;
using UnityGameFramework.Runtime;



//应该叫武器
public class GunEntityLogic : EntityLogic
{
    Transform _attackTarget;
    Transform _bulletSpawnPostion;

    //冷却时间
    float _coldDown = 0.1f;

    float _currentAttackTime;

    private bool _canAttack = true;


    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        _bulletSpawnPostion = transform.Find("BulletSpawnPostion");
    }
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);


        //索敌
        if(_attackTarget == null)
        {
            _attackTarget = EnemyEntityLogic.Enemy.transform;
        }

        //计算cd
        if(!_canAttack) 
        {
            _currentAttackTime += elapseSeconds;

            if(_currentAttackTime >= _coldDown) 
            {
                _canAttack = true;
            }
        }

        LookTarget();
        //攻击
        if (_canAttack && _attackTarget)
        {
            
            Attack();
            _canAttack = false;
            _currentAttackTime = 0;
            //进入cd
        }
    }


    void Attack()
    {
        //发射子弹

        Vector2 dir = (_attackTarget.transform.position - _bulletSpawnPostion.transform.position).normalized;
        var spawnData = SpawnBulletData.Create(_bulletSpawnPostion.position, dir, 1);
        GameEntry.Entity.ShowEntity(EntityID.GetID, typeof(BulletEntityLogic), "Assets/GameMain/Entities/Bullet.prefab", "Bullet", 1, spawnData);


        //GameEntry.Sound.PlaySound()
        //GameEntry.Sound.PlaySound($"Assets/GameMain/Brotato/Weapons/ranged/minigun/gun_machinegun_auto_heavy_shot_{Random.Range(0, 8).ToString("D2")}.wav", "Bullet");

    }


    void LookTarget()
    {
        if (_attackTarget)
        {
            transform.right = (_attackTarget.transform.position - transform.position).normalized;
        }
        else
        {
            transform.localRotation = Quaternion.identity;
        }
    }
}

public class SpawnBulletData : GameFramework.IReference
{
    public Vector2 Position;
    public Vector2 Dir;
    public float Damage;


    public static SpawnBulletData Create(Vector2 position, Vector2 dir,float damage)
    {
        var spawnData = GameFramework.ReferencePool.Acquire<SpawnBulletData>();
        spawnData.Position = position;
        spawnData.Dir = dir;
        spawnData.Damage = damage;

        return spawnData;
    }

    public void Clear()
    {
        Position = Vector2.zero; 
        Dir = Vector2.zero;
        Damage = 0;
    }
}