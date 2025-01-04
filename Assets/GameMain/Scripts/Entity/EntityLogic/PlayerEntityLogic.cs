using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class PlayerEntityLogic : EntityLogic
{

    public static Entity Player { get; private set; }

    public int HP { get; set; }
    public float Speed { get; set; } = 4.5f;

    //private InputAction _moveInputAction;

    List<Transform> _gunPositions;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        //_moveInputAction = InputSystem.actions.FindAction("Player/Move");

        ////init gun position
        //_gunPositions = new List<Transform>(2);
        //_gunPositions.Add(transform.Find("GunPosition1"));
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        Player = this.Entity;
    }


    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);


        //var move =  _moveInputAction.ReadValue<Vector2>();
        //Move(move);
    }

    protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
    {
        base.OnAttached(childEntity, parentTransform, userData);

        Debug.Log("OnAttached");
        childEntity.transform.localPosition = _gunPositions[0].localPosition;
        // childEntity.transform.localPosition = Vector3.one*100;

        
    }





    public void Move(Vector2 move)
    {
        transform.Translate(Speed * Time.deltaTime * move);
    }
    public void OnDamage(int damage) 
    {

        HP = Mathf.Clamp(HP - damage, 0, int.MaxValue);

        if (HP < 0)
        {
            Debug.LogError("Player Die");
        }
    }


}
