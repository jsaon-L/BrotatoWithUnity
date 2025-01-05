using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Pawn : EntityLogic
{
    public float MoveSpeed = 3;

    public ReactiveProperty<int> HP = new ReactiveProperty<int>();
    public ReactiveProperty<int> MaxHP = new ReactiveProperty<int>();

    public bool IsUsed;


    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        IsUsed = true;
    }
    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
        IsUsed = false;
    }

    public virtual void Move(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            return;
        }
        Vector2 moveDelta = (MoveSpeed * Time.deltaTime)*dir;
        // transform.Translate(moveDelta.x,moveDelta.y,0,Space.Self);

        transform.localPosition += new Vector3(moveDelta.x, moveDelta.y, 0);
    }

    public virtual void AddDamge(int damge)
    {
        HP.Value = Mathf.Clamp(HP.Value - damge,0, int.MaxValue );
    }

    public virtual void AddHeal(int heal)
    {
        HP.Value = Mathf.Clamp(HP.Value + heal,0, MaxHP.Value );
    }

    public void OnDie()
    {
        
    }

    public Vector2 GetWordPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

}
