using UnityEngine;
using UnityGameFramework.Runtime;

public class BulletEntityLogic : EntityLogic
{
    private Vector2 _dir;
    private float _damage;

    private float _flySpeed = 1f;


    private Rigidbody2D _rigidbody2D;
    ////射程
    //private float _Radius;
    ////可以穿透几个
    //private int 
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        SpawnBulletData spawnData  = userData as SpawnBulletData;

        if (spawnData == null)
        {
            GameFramework.GameFrameworkLog.Error("生成子弹参数错误,无法生成子弹");
            return;
        }

        transform.position = spawnData.Position;
        _dir = spawnData.Dir;
        _damage = spawnData.Damage;

        transform.right = _dir;
        _rigidbody2D.velocity = _flySpeed * _dir;


        GameFramework.ReferencePool.Release(spawnData);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);


        //Move(_flySpeed * elapseSeconds * _dir);
    }


    void Move(Vector2 translation)
    {
        transform.Translate(translation);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //撞到可伤害对象
        //可能是敌人
        //可能是中立生物

        if (collision.CompareTag("Enemy"))
        {
            //施加伤害

            //施加各种buf效果


            //播放爆炸特效

            //隐藏子弹
            GameEntry.Entity.HideEntity(Entity);
            collision.GetComponent<EnemyEntityLogic>().OnHit(1);
        }
        //else if (collision.CompareTag("Neutrality"))
        //{

        //}
    }

}
