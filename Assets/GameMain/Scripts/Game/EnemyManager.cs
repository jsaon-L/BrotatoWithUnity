using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using Skyunion;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

public class EnemyManager :MonoSingleton<EnemyManager>
{


    public List<EnemyEntityLogic> Enemys = new List<EnemyEntityLogic>();


    private void Start()
    {
        GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId,OnShowEntitySuccess);

        GameEntry.Timer.Register(0.2f, CreateEnemy, null, true);
    }

    private void OnShowEntitySuccess(object sender,GameEventArgs args)
    {
        ShowEntitySuccessEventArgs enemy = (args as ShowEntitySuccessEventArgs);
        if (enemy.EntityLogicType == typeof(EnemyEntityLogic))
        {
            Enemys.Add(enemy.Entity.Logic as EnemyEntityLogic);
        }
    }

    public void CreateEnemy()
    {
        float x = Random.Range(-5f, 5f);
        float y =  Random.Range(-5f, 5f);
        var spawnEnemyData = EnemySpawnData.Create(new Vector2(x, y), 5, 2);
        GameEntry.Entity.ShowEntity(EntityID.GetID, typeof(EnemyEntityLogic), "Assets/GameMain/Entities/Enemy.prefab", "Enemy", 1, spawnEnemyData);
    }

    public void KillEnemy(EnemyEntityLogic enemy)
    {
        if (enemy != null && enemy.IsUsed)
        {
            Enemys.Remove(enemy);
            GameEntry.Entity.HideEntity(enemy.Entity);
        }

    }

    /// <summary>
    /// 获取最近敌人
    /// </summary>
    /// <returns></returns>
    public (EnemyEntityLogic enemy,float distance) GetLatelyEnemy(Vector2 playerWorldPosition)
    {
        if (Enemys.Count<=0)
        {
            return (null,0);
        }

        //TODO:使用四叉树或者KD树等更高效的数据结构
        int enemyIndex = 0;
        float enemyDistance = (playerWorldPosition - Enemys[0].GetWordPosition()).sqrMagnitude;


        for (int i = 1; i < Enemys.Count; i++)
        {
            float distance = (playerWorldPosition- Enemys[i].GetWordPosition()).sqrMagnitude;

            if (distance < enemyDistance)
            {
                enemyIndex = i;
                enemyDistance = distance;
            }
        }

        return (Enemys[enemyIndex], enemyDistance);

    }
    
}
