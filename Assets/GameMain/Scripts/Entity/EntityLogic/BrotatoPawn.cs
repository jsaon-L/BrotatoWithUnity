using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class BrotatoPawn : Pawn
{
    private Transform _GunPosition1;
    private Transform _GunPosition2;
    
    
    
    
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        _GunPosition1 = transform.Find("GunPosition1");
        _GunPosition2 = transform.Find("GunPosition2");
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);

        MoveSpeed = 5;
        
        VarGameObject playerPawn = new VarGameObject();
        playerPawn.Value = gameObject;
        GameEntry.DataNode.SetData("PlayerPawn",playerPawn);
    }


    protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
    {
        base.OnAttached(childEntity, parentTransform, userData);
        
    }
    
    public virtual void AddWeapon(WeaponEntityLogic weapon)
    {

    }
}
