// **********************************************************************
//
// Copyright (c) 2017
// All Rights Reserved
// 非 Unity 组件的单例实现
// Author: Johance
// Email:421465201@qq.com
// Created:	2017/6/13   08:00
//
// **********************************************************************
using System.Reflection;
using System;

namespace Skyunion
{
    //非继承MonoBehaviour的单例继承此类，并加入私有构造函数
    public class TSingleton<T> where T : class//, new()
    {
        private static T _instance;
        private static readonly object syslock = new object();
        public static readonly Type[] EmptyTypes = new Type[0];
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syslock)
                    {
                        if (_instance == null)
                        {
                            ConstructorInfo ci = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, EmptyTypes, null);
                            if (ci == null) { throw new InvalidOperationException("class must contain a private constructor"); }
                            _instance = (T)ci.Invoke(null);
                        }
                    }
                }
                return _instance;
            }

        }
    }
}

