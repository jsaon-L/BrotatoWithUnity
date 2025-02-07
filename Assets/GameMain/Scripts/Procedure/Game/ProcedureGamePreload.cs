//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using GameFramework.Localization;
using GameFramework.Network;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using UGFExtensions.Await;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    public class ProcedureGamePreload : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        Dictionary<string,bool> m_LoadedFlag = new Dictionary<string,bool>();

        protected override  async void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);


            GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);



            //加载多语言文本
            string assetName = AssetUtility.GetDictionaryAsset("Default", false);
            m_LoadedFlag[assetName] = false;
            GameEntry.Localization.ReadData(assetName, this);
            
            //加载游戏

      
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            foreach (KeyValuePair<string, bool> loadedFlag in m_LoadedFlag)
            {
                if (!loadedFlag.Value)
                {
                    return;
                }
            }

            ChangeState<ProcedureGameStart>(procedureOwner);
        }
        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[ne.DictionaryAssetName] = true;
            Log.Info("Load dictionary '{0}' OK.", ne.DictionaryAssetName);
        }

        private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryAssetName, ne.DictionaryAssetName, ne.ErrorMessage);
        }
    }
}
