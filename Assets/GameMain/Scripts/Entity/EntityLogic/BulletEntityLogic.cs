using UnityEngine;
using UnityGameFramework.Runtime;

public class BulletEntityLogic : EntityLogic
{
    private Vector2 _dir;
    private float _damage;

    private float _flySpeed = 1f;


    private Rigidbody2D _rigidbody2D;
    ////���
    //private float _Radius;
    ////���Դ�͸����
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
            GameFramework.GameFrameworkLog.Error("�����ӵ���������,�޷������ӵ�");
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
        //ײ�����˺�����
        //�����ǵ���
        //��������������

        if (collision.CompareTag("Enemy"))
        {
            //ʩ���˺�

            //ʩ�Ӹ���bufЧ��


            //���ű�ը��Ч

            //�����ӵ�
            GameEntry.Entity.HideEntity(Entity);
            collision.GetComponent<EnemyEntityLogic>().OnHit(1);
        }
        //else if (collision.CompareTag("Neutrality"))
        //{

        //}
    }

}
