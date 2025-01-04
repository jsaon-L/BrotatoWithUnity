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


            var player = await GameEntry.Entity.ShowEntityAsync(EntityID.GetID, typeof(PlayerEntityLogic), "Assets/GameMain/Entities/Player.prefab", "Player",1,null);


            var spawnEnemyData = EnemySpawnData.Create(new Vector2(5, 5), 5, 2);
            var enemy = await GameEntry.Entity.ShowEntityAsync(EntityID.GetID, typeof(EnemyEntityLogic), "Assets/GameMain/Entities/Enemy.prefab", "Enemy", 1, spawnEnemyData);


            var gun = await GameEntry.Entity.ShowEntityAsync(EntityID.GetID, typeof(GunEntityLogic), "Assets/GameMain/Entities/Gun.prefab", "Gun", 1, null);

            GameEntry.Entity.AttachEntity(gun, player);
            //加载游戏

           networkChannel = GameEntry.Network.CreateNetworkChannel("test", ServiceType.Tcp, new NetworkChannelHelper());


            //networkChannel.RegisterHandler(new MyPacketHandlerBase());
            networkChannel.Connect(IPAddress.Parse("127.0.0.1"), 13000);
            
            //GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId,)



           
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (Input.GetKeyDown(KeyCode.A))
            {


                networkChannel.Send(new MyPacket() {Name = 6666 });
            }
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
