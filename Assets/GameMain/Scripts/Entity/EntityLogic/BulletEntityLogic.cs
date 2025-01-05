using GameKit.Dependencies.Utilities.ObjectPooling.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityGameFramework.Runtime;

public class BulletEntityLogic : EntityLogic
{
    private Vector2 _dir;
    private int _damage;
    private float _bulletRange;

    private float _flySpeed = 10f;


    private Rigidbody2D _rigidbody2D;
    private Projectile _projectile;

    private bool _bulletIsLive;
    
    ////���
    //private float _Radius;
    ////���Դ�͸����
    //private int 
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _projectile = gameObject.GetOrAddComponent<Projectile>();
        _projectile.OnEndBulletRange += _projectile_OnEndBulletRange;
    }

    private void _projectile_OnEndBulletRange()
    {
        GameEntry.Entity.HideEntity(Entity);
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        _bulletIsLive = true;
        SpawnBulletData spawnData  = userData as SpawnBulletData;

        if (spawnData == null)
        {
            GameFramework.GameFrameworkLog.Error("数据为空!");
            return;
        }

        transform.position = spawnData.Position;
        _dir = spawnData.Dir;
        _damage = spawnData.Damage;
        _bulletRange = spawnData.BulletRange;

        transform.right = _dir;
        _projectile.StartMove(_flySpeed, _bulletRange);


        GameFramework.ReferencePool.Release(spawnData);
    }
    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
        _projectile.StopMove();
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);


        //Move(_flySpeed * elapseSeconds * _dir);
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO:优化碰撞层级,让子弹单独层级并且不与子弹碰撞

        if (!_bulletIsLive)
        {
            return;
        }

       
        if (collision.CompareTag("Enemy"))
        {
            //TODO:使用更推荐的方式判断Tag
            _bulletIsLive = false;
            collision.GetComponent<EnemyEntityLogic>().OnHit(_damage);
            
            GameEntry.Entity.HideEntity(Entity);
        }
        //else if (collision.CompareTag("Neutrality"))
        //{

        //}
    }

}
