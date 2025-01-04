//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Download;
using System;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#else
using UnityEngine.Experimental.Networking;
#endif

namespace UnityGameFramework.Runtime
{
    public partial class UnityWebRequestDownloadAgentHelper : DownloadAgentHelperBase, IDisposable
    {
        private sealed class DownloadHandler : DownloadHandlerScript
        {
            private readonly UnityWebRequestDownloadAgentHelper m_Owner;

            private ulong m_ContentLength;
            private ulong m_DownloadedLength;

            public DownloadHandler(UnityWebRequestDownloadAgentHelper owner)
                : base(owner.m_CachedBytes)
            {
                m_Owner = owner;
            }

            protected override bool ReceiveData(byte[] data, int dataLength)
            {
                if (m_Owner != null && m_Owner.m_UnityWebRequest != null && dataLength > 0)
                {
                    m_DownloadedLength += (ulong)dataLength;
                    DownloadAgentHelperUpdateBytesEventArgs downloadAgentHelperUpdateBytesEventArgs = DownloadAgentHelperUpdateBytesEventArgs.Create(data, 0, dataLength);
                    m_Owner.m_DownloadAgentHelperUpdateBytesEventHandler(this, downloadAgentHelperUpdateBytesEventArgs);
                    ReferencePool.Release(downloadAgentHelperUpdateBytesEventArgs);

                    DownloadAgentHelperUpdateLengthEventArgs downloadAgentHelperUpdateLengthEventArgs = DownloadAgentHelperUpdateLengthEventArgs.Create(dataLength);
                    m_Owner.m_DownloadAgentHelperUpdateLengthEventHandler(this, downloadAgentHelperUpdateLengthEventArgs);
                    ReferencePool.Release(downloadAgentHelperUpdateLengthEventArgs);
                }

                return base.ReceiveData(data, dataLength);
            }


            protected override void ReceiveContentLengthHeader(ulong contentLength)
            {
                m_ContentLength = contentLength;
                base.ReceiveContentLengthHeader(contentLength);
            }

            protected override float GetProgress()
            {
                if (m_ContentLength <= 0 || m_DownloadedLength <= 0)
                {
                    return 0f;
                }
                else if (m_DownloadedLength == m_ContentLength)
                {
                    return 1f;
                }
                else
                {
                    return m_DownloadedLength / (float)m_ContentLength;
                }
            }
        }

  
    }
}
