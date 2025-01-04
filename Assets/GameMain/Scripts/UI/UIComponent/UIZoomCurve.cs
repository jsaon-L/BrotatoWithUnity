using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [DisallowMultipleComponent]
    public class UIZoomCurve : MonoBehaviour
    {
        public static UIZoomCurve Instance;

        public void Awake()
        {
            Instance = this;
        }

        [Header("UI缩放")]
        public AnimationCurve AllUIScaleCurve;

        [Header("UI3D缩放")]
        public  AnimationCurve AllUIScaleCurve3D;

        [Header("部队UI")]
        public  AnimationCurve TroopCurve;

        [Header("部队普通情况半径")]
        public  AnimationCurve TroopRadiusCure;

        [Header("部队战斗情况半径")]
        public  AnimationCurve TroopFightRadiusCure;

        [Header("部队战斗连线")]
        public  AnimationCurve TroopFightLineCurve;

        [Header("主城名字age1偏移")]
        public  AnimationCurve CastleTitleOffsetCurve;

        [Header("主城交战开始age1")]
        public  AnimationCurve CastleFightStartOffsetCurve;

        [Header("主城交战结束age1")]
        public  AnimationCurve CastleFightEndOffsetCurve;

        [Header("主城名字age2")]
        public  AnimationCurve CastleTitleOffsetCurve2;

        [Header("主城名字age3")]
        public  AnimationCurve CastleTitleOffsetCurve3;

        [Header("主城名字age4")]
        public  AnimationCurve CastleTitleOffsetCurve4;

        [Header("联盟标记偏移")]
        public  AnimationCurve GuildMarkerOffsetCurve;

        [Header("野蛮人部队")]
        public  AnimationCurve NpcTroopCurve;

        [Header("资源点等级")]
        public  AnimationCurve RssTitleCurve;

        [Header("资源点状态")]
        public  AnimationCurve RssStatusCurve;

        [Header("关卡名字")]
        public  AnimationCurve PassTitleCurve;

        [Header("关卡交战开始点")]
        public  AnimationCurve PassFightStartOffsetCurve;

        [Header("关卡交战结束点")]
        public  AnimationCurve PassFightEndOffsetCurve;

        [Header("主权塔名字")]
        public  AnimationCurve FortressTitleCurve;

        [Header("主权塔交战开始点")]
        public  AnimationCurve FortressFightStartOffsetCurve;

        [Header("主权塔交战结束点")]
        public  AnimationCurve FortressFightEndOffsetCurve;

        [Header("联盟建筑名字")]
        public  AnimationCurve AsBuildingNameCurve;

        [Header("联盟建筑交战开始点")]
        public  AnimationCurve AsBuildingFightStartOffsetCurve;

        [Header("联盟建筑交战结束点")]
        public  AnimationCurve AsBuildingFightEndOffsetCurve;

        [Header("联盟建筑HP")]
        public  AnimationCurve AsBuildingHpCurve;

        [Header("联盟旗帜名字")]
        public  AnimationCurve AsFlagNameCurve;

        [Header("联盟旗帜交战开始点")]
        public  AnimationCurve AsFlagFightStartOffsetCurve;

        [Header("联盟旗帜交战结束点")]
        public  AnimationCurve AsFlagFightEndOffsetCurve;

        [Header("联盟旗帜HP")]
        public  AnimationCurve AsFlagHpCurve;

        [Header("村庄")]
        public  AnimationCurve VillageCurve;

        [Header("斥候")]
        public  AnimationCurve ScoutCurve;

        [Header("山洞")]
        public  AnimationCurve CaveCurve;

        [Header("野蛮人城寨")]
        public  AnimationCurve NpcCastleCurve;

        [Header("部队击溃")]
        public  AnimationCurve TroopDefeatCurve;

        [Header("联盟资源点")]
        public  AnimationCurve VeinCure;
    }
}
