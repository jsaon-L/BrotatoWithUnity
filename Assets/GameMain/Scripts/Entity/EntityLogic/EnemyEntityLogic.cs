using UnityEngine;
using UnityGameFramework.Runtime;

public class EnemyEntityLogic : Pawn
{

    public SpriteRenderer Body { get;set; }
    public Material BodyMaterial { get; set; }

    //�Ƿ�����չʾ�˺���Ч
    private bool _showHitEffecting;
    //�˺�չʾʱ��
    private float _hitEffectDuration = 0.1f;
    private float _currentHitEffectTime;

    private GameObject _playerPawn;
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        Body = transform.Find("Body").GetComponent<SpriteRenderer>();
        BodyMaterial = Body.material;
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        HideHitEffect();

        _playerPawn = GameEntry.DataNode.GetData<VarGameObject>("PlayerPawn");

        EnemySpawnData spawnData = userData as EnemySpawnData;

        if (spawnData == null) 
        {
            GameFramework.GameFrameworkLog.Error("数据为空!");
            return;
        }

        HP.Value = spawnData.SpawnHP; 
        MoveSpeed = spawnData.SpawnSpeed;
        transform.position = spawnData.SpawnPosition;

        GameFramework.ReferencePool.Release(spawnData);
    }
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);


        
        if (_playerPawn) 
        {
            var polayerDir = (_playerPawn.transform.position - transform.position).normalized;
            Move(polayerDir);
        
        }

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
        //TODO:这里不应该使用材质开关,而应该控制数值 需要看文档确认一下
        _showHitEffecting = false;
        _currentHitEffectTime = 0;
        BodyMaterial.DisableKeyword("HITEFFECT_ON");
    }





    public void OnHit(int damage)
    {
        ShowHitEffect();
        AddDamge(damage);

        if (HP.Value<=0)
        {
            EnemyManager.Instance().KillEnemy(this);
        }
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
