using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ר�ſ����ӵ����е����
/// Ĭ����right�������
/// </summary>
public class Projectile : MonoBehaviour
{
    private float _movedDistance;

    public float Speed;

    public bool IsActive;

    public float BulletRange;

    /// <summary>
    /// ������̾�ͷ
    /// </summary>
    public event Action OnEndBulletRange;

    //TODO:���жϾ����Ϊƽ���ıȽ�,�������ÿ�������
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
