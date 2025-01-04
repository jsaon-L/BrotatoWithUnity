using UnityEngine;
using UnityGameFramework.Runtime;

public class EnemyEntityLogic : EntityLogic
{

    public static Entity Enemy;

    public int HP { get; set; }
    public float Speed { get; set; } = 3;

    public SpriteRenderer Body { get;set; }
    public Material BodyMaterial { get; set; }

    //�Ƿ�����չʾ�˺���Ч
    private bool _showHitEffecting;
    //�˺�չʾʱ��
    private float _hitEffectDuration = 0.1f;
    private float _currentHitEffectTime;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        Body = transform.Find("Body").GetComponent<SpriteRenderer>();
        BodyMaterial = Body.material;
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        Enemy = this.Entity;

        EnemySpawnData spawnData = userData as EnemySpawnData;

        if (spawnData == null) 
        {
            GameFramework.GameFrameworkLog.Error("���ɵ��˲�������,�޷���ʼ������");
            return;
        }

        HP = spawnData.SpawnHP; 
        Speed = spawnData.SpawnSpeed;
        transform.position = spawnData.SpawnPosition;

        GameFramework.ReferencePool.Release(spawnData);
    }
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);


        if (PlayerEntityLogic.Player) 
        {
            var polayerDir = (PlayerEntityLogic.Player.transform.position - transform.position).normalized;
            Move(polayerDir);

        }

        //�˺���Ч
        if (_showHitEffecting)
        {
            _currentHitEffectTime += elapseSeconds;

            if (_currentHitEffectTime>=_hitEffectDuration)
            {
                HideHitEffect();
            }
        }

    }


    public void ShowHitEffect()
    {
        _showHitEffecting = true;
        _currentHitEffectTime = 0;
        BodyMaterial.EnableKeyword("HITEFFECT_ON");
    }
    public void HideHitEffect()
    {
        _showHitEffecting = false;
        _currentHitEffectTime = 0;
        BodyMaterial.DisableKeyword("HITEFFECT_ON");
    }


    public void Move(Vector2 move)
    {
        transform.Translate(Speed * Time.deltaTime * move);
    }



    public void OnHit(float damage)
    {
        ShowHitEffect();
    }
}

public class EnemySpawnData : GameFramework.IReference
{
    public Vector2 SpawnPosition;
    public int SpawnHP;
    public float SpawnSpeed;


    public static EnemySpawnData Create(Vector2 spawnPosition, int spawnHp, float spawnSpeed)
    {
        var spawnData = GameFramework.ReferencePool.Acquire<EnemySpawnData>();

        spawnData.SpawnPosition = spawnPosition;
        spawnData.SpawnHP = spawnHp;
        spawnData.SpawnSpeed = spawnSpeed;
        return spawnData;

    }

    public void Clear()
    {
        SpawnPosition = Vector2.zero;
        SpawnHP = 0;
        SpawnSpeed = 0;
    }
}
