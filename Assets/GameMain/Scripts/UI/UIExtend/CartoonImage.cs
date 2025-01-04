using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    public class CartoonImage : Image
    {

        [SerializeField]
        private Sprite[] m_sprites;

        [SerializeField]
        private bool m_switch = true;

        [SerializeField]
        private float m_fps;

        [SerializeField]
        private bool m_alwaysSetNativeSize;


        public Sprite[] Sprites
        {
            get => m_sprites;
            set => m_sprites = value;
        }

        public float Fps
        {
            get => m_fps;
            set
            {
                m_fps = value;
                frameRate = 1 / m_fps;
            }
        }

        public bool AlwaysSetNativeSize
        {
            get => m_alwaysSetNativeSize;
            set => m_alwaysSetNativeSize = value;
        }

        protected override void Start()
        {
            base.Start();

            if(Sprites!=null)
            {
                for(int i = 0;i<Sprites.Length;i++)
                {
                    if(Sprites[i]==sprite)
                    {
                        Index = i;
                        break;
                    }
                }
            }
            lastTime = Time.time;
            frameRate = 1 / m_fps;
        }

        public int Index
        {
            set
            {
                Index = value;
                if (value<0)
                {
                    Index = 0;
                }
            }
            get => Index;
        }

        public bool Switch
        {
            set
            {
                m_switch = value;
                lastTime = Time.time;
            }
            get => m_switch;
        }

        private float frameRate;
        private float lastTime;
        void Update()
        {
            if (Sprites == null||!Switch)
            {
                return;
            }

            if (Time.time - lastTime > frameRate)
            {
                lastTime = Time.time;
                if (Index>=Sprites.Length)
                {
                    Index = 0;
                }
                sprite = Sprites[Index];
                if(AlwaysSetNativeSize)
                {
                    this.SetNativeSize();
                }
            }
        }
    }
}

