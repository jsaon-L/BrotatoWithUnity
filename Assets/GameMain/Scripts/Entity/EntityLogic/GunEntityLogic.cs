
using UnityEngine;
using UnityGameFramework.Runtime;



//Ӧ�ý�����
public class GunEntityLogic : WeaponEntityLogic
{
    Transform _bulletSpawnPostion;


    private string _bulletAsset = "Assets/GameMain/Entities/Bullet.prefab";


    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        _bulletSpawnPostion = transform.Find("BulletSpawnPostion");
    }
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

    }


   public override void Attack()
    {
        base.Attack();

        if (TargetEnemy)
        {

            Vector2 dir = (TargetEnemy.transform.position - _bulletSpawnPostion.transform.position).normalized;
            var spawnData = SpawnBulletData.Create(_bulletSpawnPostion.position, dir, Damage, AttackRange);
            GameEntry.Entity.ShowEntity(EntityID.GetID, typeof(BulletEntityLogic), _bulletAsset, "Bullet", 10, spawnData);

            //TODO:使用GC更少的字符串方式
            GameEntry.Sound.PlaySound($"Assets/GameMain/Brotato/Weapons/ranged/minigun/gun_machinegun_auto_heavy_shot_{Random.Range(1, 9).ToString("D2")}.wav", "Bullet");
        }


    }


   
   
}

public class SpawnBulletData : GameFramework.IReference
{
    public Vector2 Position;
    public Vector2 Dir;
    //射程
    public float BulletRange;
    public int Damage;


    public static SpawnBulletData Create(Vector2 position, Vector2 dir,int damage,float bulletRange)
    {
        var spawnData = GameFramework.ReferencePool.Acquire<SpawnBulletData>();
        spawnData.Position = position;
        spawnData.Dir = dir;
        spawnData.Damage = damage;
        spawnData.BulletRange = bulletRange;
        return spawnData;
    }

    public void Clear()
    {
        Position = Vector2.zero; 
        Dir = Vector2.zero;
        Damage = 0;
    }
}