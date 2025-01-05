using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;

public class PlayerControl : EntityLogic
{

    [SerializeField]
    private Pawn _pawn;

    private Vector2 _moveDir;

    private void Start()
    {
    }

    public void Process(Pawn pawn)
    {
        _pawn = pawn;
    }
    public void UnProcess()
    {
        _pawn = null;
    }

    public Pawn GetPawn()
    {
        return _pawn;
    }

    private void Update()
    {
        _pawn.Move(_moveDir);
    }

    public void OnMove(InputValue input)
    {
        if (_pawn)
        {
            _moveDir = input.Get<Vector2>();
        }
        
    }
}
