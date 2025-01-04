using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

namespace Client
{
    public class AutoPlayAndDestroyTip : MonoBehaviour
    {
        private Animation ani;
        public AnimationClip m_showAni;
        public AnimationClip m_closeAni;
        public AnimationClip m_changeAni;

        private void Start()
        {
            this.ani = base.gameObject.GetComponent<Animation>();
            if (this.ani == null)
            {
                this.ani = base.gameObject.AddComponent<Animation>();
            }

            if(m_showAni!=null)
            {
                this.ani.AddClip(this.m_showAni, "Show");
                this.ani.Play("Show");
            }
            if (this.m_closeAni != null)
            {
                this.ani.AddClip(this.m_closeAni, "Close");
            }
            if (this.m_changeAni != null)
            {
                this.ani.AddClip(this.m_changeAni, "Change");
            }

        }

        public void PlayEndAni()
        {
            if(this.ani != null&&this.m_closeAni != null)
            {
                ani.Play("Close");
            }
            isPlayingClose = true;
        }

        public float PlayChangeAni()
        {
            float len = 0;
            if (this.ani != null && this.m_changeAni != null)
            {
                ani.Play("Change");
                len = m_changeAni.length;
            }
            isPlayingClose = true;
            return len;
        }

        private bool isPlayingClose = false;
        public void LateUpdate()
        {
            if(isPlayingClose)
            {
                if(this.ani == null||!this.ani.isPlaying)
                {
                    GameObject.DestroyImmediate(this.gameObject);
                }
            }
        }
    }
}

