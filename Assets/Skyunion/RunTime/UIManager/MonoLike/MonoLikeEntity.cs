/////////////////////////////////////////////////////////////////////////////////
// @desc 模拟Mono生命周期
// @copyright ©2018 iGG
// @release 2018年3月30日 星期五
// @author BobWong
// @mail 15959187562@qq.com
/////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

namespace Skyunion
{
    public abstract class MonoLikeEntity : IMonoLike
    {
        /// <summary>
        /// 等同Mono中的enable属性
        /// </summary>
        public bool enable = true;
        /// <summary>
        /// 是否需要接收帧循环和固定帧循环的信息
        /// </summary>
        public bool needUpdate = false;

        public Transform transform;

        public virtual void OnEnable(){}
        public virtual void Start() {}
        public virtual void Update(){}
        public virtual void OnDisable(){}
        public virtual void OnDestroy() {}
        public virtual void LateUpdate(){}
        public virtual void FixedUpdate(){}

    }
}
