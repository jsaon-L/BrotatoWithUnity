using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class SmoothProgressBar : MonoBehaviour
    {
        public float maxSmoothTime = 1f;

        private bool bPlay;

        private Slider slider;

        private float targetValue;

        private bool bAdd = true;

        private float maxValue = 0f;

        private float minValue = 0f;

        private void Start()
        {
            this.slider = base.GetComponent<Slider>();
            maxValue = slider.maxValue;
            minValue = slider.minValue;
        }

        private void Update()
        {
            if (this.bPlay)
            {
                if (this.bAdd)
                {
                    float num = this.slider.value + Time.deltaTime * (1f / this.maxSmoothTime);
                    if (num > this.targetValue)
                    {
                        this.slider.value = this.targetValue;
                        this.bPlay = false;
                    }
                    else
                    {
                        if (num > maxValue)
                        {
                            this.bPlay = false;
                        }
                        this.slider.value = num;
                    }
                }
                else
                {
                    float num2 = this.slider.value - Time.deltaTime * (1f / this.maxSmoothTime);
                    if (num2 < this.targetValue)
                    {
                        this.slider.value = this.targetValue;
                        this.bPlay = false;
                    }
                    else
                    {
                        if (num2 < minValue)
                        {
                            this.bPlay = false;
                        }
                        this.slider.value = num2;
                    }
                }
            }
        }

        public void SetValue(float value)
        {
            if (this.slider == null)
            {
                this.slider = base.GetComponent<Slider>();
            }
            this.bAdd = (value - this.slider.value > 0f);
            this.targetValue = value;
            this.bPlay = true;
        }

        public void SetValue(float value, bool smooth = true)
        {
            if (this.slider == null)
            {
                this.slider = base.GetComponent<Slider>();
            }
            if (smooth)
            {
                this.bAdd = (value - this.slider.value > 0f);
                this.targetValue = value;
                this.bPlay = true;
            }
            else
            {
                this.slider.value = value;
                this.bPlay = false;
            }
        }

        public void Reset()
        {
            if (this.slider == null)
            {
                this.slider = base.GetComponent<Slider>();
            }
            this.bPlay = false;
            this.slider.value = 0f;
        }
    }
}