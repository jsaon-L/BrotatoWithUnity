// **********************************************************************
//
// Copyright (c) 2017
// All Rights Reserved
// Unity 组件的单例实现
// Author: Johance
// Email:421465201@qq.com
// Created:	2017/6/13   08:00
//
// **********************************************************************
using UnityEngine;

namespace Skyunion
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T m_Instance = null;
        public static T Instance()
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (m_Instance == null)
                {
                    m_Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    var root = GameObject.Find("MonoSingleton");
                    if (root == null)
                    {
                        root = new GameObject("MonoSingleton");
                        //DontDestroyOnLoad(root);
                    }
                    m_Instance.transform.parent = root.transform;
                }
            }
            return m_Instance;
        }

        private void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this as T;
                Init();
            }
            else
            {
                DestroyImmediate(this);
            }
        }
        private void OnDestroy()
        {
            if(m_Instance == this)
                m_Instance = null;
        }

        public virtual void Init() { }

        private void OnApplicationQuit()
        {
            m_Instance = null;
        }
    }
}