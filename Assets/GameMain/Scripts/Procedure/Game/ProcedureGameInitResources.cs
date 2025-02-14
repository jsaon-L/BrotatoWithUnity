﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Localization;
using System;
using UGFExtensions.Await;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    public class ProcedureGameInitResources : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            AwaitableExtensions.SubscribeEvent();

#if !UNITY_EDITOR
            GameEntry.Resource.InitResources(() => {

                ChangeState<ProcedureGamePreload>(procedureOwner);
            
            });
#else
            ChangeState<ProcedureGamePreload>(procedureOwner);

#endif

            //加载游戏

        }


       
     


    }
}
