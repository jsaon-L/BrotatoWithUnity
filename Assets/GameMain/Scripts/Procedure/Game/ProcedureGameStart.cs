//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Localization;
using GameFramework.Network;
using ProtoBuf;
using System;
using System.Net;
using System.Net.NetworkInformation;
using UGFExtensions.Await;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    public class ProcedureGameStart : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }
        INetworkChannel networkChannel;
        protected override  async void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            AwaitableExtensions.SubscribeEvent();

            
            

            var player = await GameEntry.Entity.ShowEntityAsync(
                EntityID.GetID,
                typeof(BrotatoPawn),
                "Assets/GameMain/Entities/Player.prefab", 
                "Player",
                1,
                null);
            var playerControl = await GameEntry.Entity.ShowEntityAsync(
                EntityID.GetID,
                typeof(PlayerControl),
                "Assets/GameMain/Entities/PlayerControl.prefab", 
                "Player",
                1,
                null);

            (playerControl.Logic as PlayerControl).Process(player.Logic as Pawn);
            
            
            EnemyManager.Instance().CreateEnemy();

            var gun = await GameEntry.Entity.ShowEntityAsync(EntityID.GetID, typeof(GunEntityLogic), "Assets/GameMain/Entities/Gun.prefab", "Gun", 1, null);
            GameEntry.Entity.AttachEntity(gun, player);
            
            var gun2 = await GameEntry.Entity.ShowEntityAsync(EntityID.GetID, typeof(GunEntityLogic), "Assets/GameMain/Entities/Gun.prefab", "Gun", 1, null);
            GameEntry.Entity.AttachEntity(gun2, player);
            //加载游戏

      
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
 
        }

       public class MyPacketHandlerBase : PacketHandlerBase
        {
            public override int Id => 100;

            public override void Handle(object sender, Packet packet)
            {
                Debug.Log(packet);
            }
        }

        [Serializable, ProtoContract(Name = @"MyPacket")]
        public class MyPacket : CSHeartBeat
        {
            public override PacketType PacketType => PacketType.ClientToServer;

            public override int Id => 100;

            [ProtoMember(1)]
            public int Name { get; set; }

            public override void Clear()
            {
                
            }

            public override string ToString()
            {
                return Name.ToString();
            }
        }

    }
}
