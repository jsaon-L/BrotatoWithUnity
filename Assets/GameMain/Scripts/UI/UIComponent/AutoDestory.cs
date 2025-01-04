using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class AutoDestory : MonoBehaviour
    {

        public float time = 5f;

        private float live = 0f;
        // Start is called before the first frame update
        void Start()
        {
            live = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if(Time.time-live>time)
            {
                GameObject.DestroyImmediate(this.gameObject);
            }
        }
    }
}

