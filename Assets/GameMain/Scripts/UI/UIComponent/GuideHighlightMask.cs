using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using GameFramework;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{

    public class GuideHighlightMask : MaskableGraphic, UnityEngine.ICanvasRaycastFilter
    {
        public RectTransform arrow;
        public Vector2 center = Vector2.zero;
        public Vector2 size = new Vector2(100, 100);

        private Canvas canvas;

        private Canvas Canvas
        {
            get
            {
                if (!canvas)
                {
                    canvas = transform.GetComponentInParent<Canvas>();
                }

                return canvas;
            }
        }

        protected override void Awake()
        {
            canvas = transform.GetComponentInParent<Canvas>();
        }

        public void ForceUpdatePos()
        {
            DoUpdate();
        }

        public void DoUpdate()
        {
            if (!Canvas)
            {
                Debug.LogError("Error!Can not find Canvas!");
                return;
            }
            if (!arrow)
            {
                return;
            }
            // 当引导箭头位置或者大小改变后更新，注意：未处理拉伸模式
            if (arrow && center != arrow.anchoredPosition || size != arrow.sizeDelta)
            {
                Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(Canvas.transform, arrow);
                //this.center = arrow.anchoredPosition;
                center = bounds.center;
                this.size = arrow.sizeDelta;
                SetAllDirty();
            }
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            // 点击在箭头框内部则无效，否则生效
            return !RectTransformUtility.RectangleContainsScreenPoint(arrow, sp, eventCamera);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            //Debug.Log("OnPopulateMesh");
            Vector4 outer = new Vector4(-rectTransform.pivot.x * rectTransform.rect.width, -rectTransform.pivot.y * rectTransform.rect.height,
                                       (1 - rectTransform.pivot.x) * rectTransform.rect.width, (1 - rectTransform.pivot.y) * rectTransform.rect.height);

            Vector4 inner = new Vector4(center.x - size.x / 2,
                                        center.y - size.y / 2,
                                        center.x + size.x * 0.5f,
                                        center.y + size.y * 0.5f);

            vh.Clear();

            var vert = UIVertex.simpleVert;

            // left
            vert.position = new Vector2(outer.x, outer.y);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(outer.x, outer.w);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(inner.x, outer.w);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(inner.x, outer.y);
            vert.color = color;
            vh.AddVert(vert);
          
            // top
            vert.position = new Vector2(inner.x, inner.w);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(inner.x, outer.w);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(inner.z, outer.w);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(inner.z, inner.w);
            vert.color = color;
            vh.AddVert(vert);

            // right
            vert.position = new Vector2(inner.z, outer.y);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(inner.z, outer.w);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(outer.z, outer.w);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(outer.z, outer.y);
            vert.color = color;
            vh.AddVert(vert);

            // bottom
            vert.position = new Vector2(inner.x, outer.y);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(inner.x, inner.y);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(inner.z, inner.y);
            vert.color = color;
            vh.AddVert(vert);

            vert.position = new Vector2(inner.z, outer.y);
            vert.color = color;
            vh.AddVert(vert);

            for (int i = 0; i < 4; i++)
            {
                int index = i * 4;
                vh.AddTriangle(0 + index, 1 + index, 2 + index);
                vh.AddTriangle(2 + index, 3 + index, 0 + index);
            }
        }
        

        private void Update()
        {
            DoUpdate();
        }
    }
}