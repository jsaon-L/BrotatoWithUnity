using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 专门控制子弹飞行的组件
/// 默认向right方向飞行
/// </summary>
public class Projectile : MonoBehaviour
{
    private float _movedDistance;

    public float Speed;

    public bool IsActive;

    public float BulletRange;

    /// <summary>
    /// 到达射程尽头
    /// </summary>
    public event Action OnEndBulletRange;

    //TODO:把判断距离改为平方的比较,这样不用开根号了
    private void Update()
    {
        if (IsActive)
        {
            Vector3 delta = transform.right * (Speed * Time.deltaTime);
            _movedDistance += Vector3.Distance(Vector3.zero, delta);
            if (_movedDistance >= BulletRange)
            {
                OnEndBulletRange?.Invoke();
            }
            else 
            {
                transform.Translate(delta, Space.World);
            }
        }
    }


    public void StartMove(float speed,float bulletRange)
    {
        IsActive = true;
        _movedDistance = 0;

        Speed = speed;
        BulletRange = bulletRange;
    }

    public void StopMove()
    {
        IsActive = false;
    }

}
