//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.WebRequest;
using System;
using UnityEngine;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#else
using UnityEngine.Experimental.Networking;
#endif
using Utility = GameFramework.Utility;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 使用 UnityWebRequest 实现的 Web 请求代理辅助器。
    /// </summary>
    public class UnityWebRequestAgentHelper : WebRequestAgentHelperBase, IDisposable
    {
        private UnityWebRequest m_UnityWebRequest = null;
        private bool m_Disposed = false;
        private float m_Progress = 0;

        private EventHandler<WebRequestAgentHelperCompleteEventArgs> m_WebRequestAgentHelperCompleteEventHandler = null;
        private EventHandler<WebRequestAgentHelperErrorEventArgs> m_WebRequestAgentHelperErrorEventHandler = null;

        private EventHandler<WebRequestAgentHelperProgressEventArgs> m_WebRequestAgentHelperProgressEventHandler = null;

        /// <summary>
        /// 最大重试次数
        /// </summary>
        private const int MaximumRetry = 2;
        /// <summary>
        /// 重试时间间隔
        /// </summary>
        private const float RetryInterval = 1.0f;

        /// <summary>
        /// 已经重试次数
        /// </summary>
        private int m_RetryCount = 0;

        private readonly RetryData m_RetryData = new();

        class RetryData
        {
            public string webRequestUri;
            public byte[] postData;
            public object userData;

            public void SetData(string webRequestUri, object userData)
            {
                this.webRequestUri = webRequestUri;
                this.userData = userData;
                this.postData = null;
            }
            public void SetData(string webRequestUri, byte[] postData, object userData)
            {
                this.webRequestUri = webRequestUri;
                this.userData = userData;
                this.postData = postData;
            }

            public void Reset()
            {
                webRequestUri = null;
                userData = null;
                postData = null;
            }
        }

        /// <summary>
        /// Web 请求代理辅助器完成事件。
        /// </summary>
        public override event EventHandler<WebRequestAgentHelperProgressEventArgs> WebRequestAgentHelperProgress
        {
            add
            {
                m_WebRequestAgentHelperProgressEventHandler += value;
            }
            remove
            {
                m_WebRequestAgentHelperProgressEventHandler -= value;
            }
        }

        /// <summary>
        /// Web 请求代理辅助器完成事件。
        /// </summary>
        public override event EventHandler<WebRequestAgentHelperCompleteEventArgs> WebRequestAgentHelperComplete
        {
            add
            {
                m_WebRequestAgentHelperCompleteEventHandler += value;
            }
            remove
            {
                m_WebRequestAgentHelperCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// Web 请求代理辅助器错误事件。
        /// </summary>
        public override event EventHandler<WebRequestAgentHelperErrorEventArgs> WebRequestAgentHelperError
        {
            add
            {
                m_WebRequestAgentHelperErrorEventHandler += value;
            }
            remove
            {
                m_WebRequestAgentHelperErrorEventHandler -= value;
            }
        }

        /// <summary>
        /// 通过 Web 请求代理辅助器发送请求。
        /// </summary>
        /// <param name="webRequestUri">要发送的远程地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void Request(string webRequestUri, object userData)
        {
            if (m_WebRequestAgentHelperCompleteEventHandler == null || m_WebRequestAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            m_RetryData.SetData(webRequestUri, userData);
            m_UnityWebRequest = CreateWebRequest(m_RetryData);

            //m_UnityWebRequest.SetRequestHeader("Accept", "*/*");
            //m_UnityWebRequest.SetRequestHeader("Accept-Encoding", "gzip, deflate");
            //m_UnityWebRequest.SetRequestHeader("Accept-Language", "zh-CN,zh;q=0.9");
            //m_UnityWebRequest.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

#if UNITY_2017_2_OR_NEWER
            m_UnityWebRequest.SendWebRequest();
#else
            m_UnityWebRequest.Send();
#endif
        }

        /// <summary>
        /// 通过 Web 请求代理辅助器发送请求。
        /// </summary>
        /// <param name="webRequestUri">要发送的远程地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void Request(string webRequestUri, byte[] postData, object userData)
        {
            if (m_WebRequestAgentHelperCompleteEventHandler == null || m_WebRequestAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            m_RetryData.SetData(webRequestUri, postData, userData);
            m_UnityWebRequest = CreateWebRequest(m_RetryData);

#if UNITY_2017_2_OR_NEWER
            m_UnityWebRequest.SendWebRequest();
#else
            m_UnityWebRequest.Send();
#endif
        }

        private UnityWebRequest CreateWebRequest(RetryData retryData)
        {
            if (retryData.postData != null)
            {
                return UnityWebRequest.Post(retryData.webRequestUri, Utility.Converter.GetString(retryData.postData));
            }

            WWWFormInfo wwwFormInfo = (WWWFormInfo)retryData.userData;
            if (wwwFormInfo.WWWForm == null)
            {
                return UnityWebRequest.Get(retryData.webRequestUri);
            }
            else
            {
               return UnityWebRequest.Post(retryData.webRequestUri, wwwFormInfo.WWWForm);
            }
        }

        System.Collections.IEnumerator RetryRequest()
        {
            //清理
            if (m_UnityWebRequest != null)
            {
                m_UnityWebRequest.Dispose();
                m_UnityWebRequest = null;
            }

            yield return new WaitForSeconds(RetryInterval);
            
            m_RetryCount++;
            Log.Debug("RetryRequest:" + m_RetryCount);
    
            //重试
            if (m_RetryData.postData !=null)
            {
                Request(m_RetryData.webRequestUri, m_RetryData.postData, m_RetryData.userData);
            }
            else
            {
                Request(m_RetryData.webRequestUri, m_RetryData.userData);
            }
        }

        /// <summary>
        /// 重置 Web 请求代理辅助器。
        /// </summary>
        public override void Reset()
        {
            if (m_UnityWebRequest != null)
            {
                m_UnityWebRequest.Dispose();
                m_UnityWebRequest = null;
            }
            m_Progress = 0;
            m_RetryCount = 0;
            m_RetryData.Reset();
            StopAllCoroutines();
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">释放资源标记。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                if (m_UnityWebRequest != null)
                {
                    m_UnityWebRequest.Dispose();
                    m_UnityWebRequest = null;
                }
            }

            m_Disposed = true;
        }

        private void Update()
        {

            if (m_UnityWebRequest != null && !m_UnityWebRequest.isDone)
            {
                SendProgress();
            }


            if (m_UnityWebRequest == null || !m_UnityWebRequest.isDone)
            {
                return;
            }

            bool isError = false;
#if UNITY_2020_2_OR_NEWER
            isError = m_UnityWebRequest.result != UnityWebRequest.Result.Success;
#elif UNITY_2017_1_OR_NEWER
            isError = m_UnityWebRequest.isNetworkError || m_UnityWebRequest.isHttpError;
#else
            isError = m_UnityWebRequest.isError;
#endif
            if (isError)
            {
                if (m_RetryCount >= MaximumRetry)
                {
                    WebRequestAgentHelperErrorEventArgs webRequestAgentHelperErrorEventArgs = WebRequestAgentHelperErrorEventArgs.Create(m_UnityWebRequest.error);
                    m_WebRequestAgentHelperErrorEventHandler(this, webRequestAgentHelperErrorEventArgs);
                    ReferencePool.Release(webRequestAgentHelperErrorEventArgs);
                }
                else
                {
                    isError = false;
                    StartCoroutine(RetryRequest());
                }
            }
            else if (m_UnityWebRequest.downloadHandler.isDone)
            {
                //先发送进度
                SendProgress();

                WebRequestAgentHelperCompleteEventArgs webRequestAgentHelperCompleteEventArgs = WebRequestAgentHelperCompleteEventArgs.Create(m_UnityWebRequest.downloadHandler.data);
                m_WebRequestAgentHelperCompleteEventHandler(this, webRequestAgentHelperCompleteEventArgs);
                ReferencePool.Release(webRequestAgentHelperCompleteEventArgs);
            }
        }


        private void SendProgress()
        {
            if (m_Progress != m_UnityWebRequest.downloadProgress)
            {
                m_Progress = m_UnityWebRequest.downloadProgress;
                WebRequestAgentHelperProgressEventArgs webRequestAgentHelperCompleteEventArgs = WebRequestAgentHelperProgressEventArgs.Create(m_UnityWebRequest.downloadProgress);
                m_WebRequestAgentHelperProgressEventHandler(this, webRequestAgentHelperCompleteEventArgs);
                ReferencePool.Release(webRequestAgentHelperCompleteEventArgs);
            }
        }
    }
}
