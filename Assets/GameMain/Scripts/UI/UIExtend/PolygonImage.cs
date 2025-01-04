using System;
using System.Collections.Generic;
using GameFramework;

namespace UnityEngine.UI
{
    public class PolygonImage : Image, IMeshModifier
    {
#if UNITY_EDITOR
        [SerializeField]
        public Sprite m_fakeSprite;

        [SerializeField]
        public Sprite m_editOverrideSprite
        {
            get { return m_fakeSprite != null ? m_fakeSprite : sprite; }
        }
#endif
        public string assetName = "";
        public void ModifyMesh(VertexHelper vh)
        {
        
        }

        public void ModifyMesh(Mesh mesh)
        {
      
        }


#if UNITY_EDITOR
        protected override void UpdateMaterial()
        {
            base.UpdateMaterial();

            if(Application.isPlaying)
            {
                return;
            }

            if (m_editOverrideSprite != null)
            {
                this.canvasRenderer.SetTexture(m_editOverrideSprite.texture);
                return;
            }

            if (s_WhiteTexture != null)
            {
                canvasRenderer.SetAlphaTexture(s_WhiteTexture);
            }
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);
            if (Application.isPlaying)
            {
                return;
            }
            if (m_editOverrideSprite == null)
            {
                base.OnPopulateMesh(toFill);
                return;
            }
            if(m_fakeSprite!=null)
            {
                if (!useSpriteMesh)
                    GenerateSimpleSprite(toFill, this.preserveAspect);
                else
                    GenerateSprite(toFill, this.preserveAspect);
            }
        }


        void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
        {
            Vector4 v = GetDrawingDimensions(lPreserveAspect);
            var uv = (m_editOverrideSprite != null) ? Sprites.DataUtility.GetOuterUV(m_editOverrideSprite) : Vector4.zero;

            var color32 = color;
            vh.Clear();
            vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(uv.x, uv.y));
            vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(uv.x, uv.w));
            vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(uv.z, uv.w));
            vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(uv.z, uv.y));

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        private void PreserveSpriteAspectRatio(ref Rect rect, Vector2 spriteSize)
        {
            var spriteRatio = spriteSize.x / spriteSize.y;
            var rectRatio = rect.width / rect.height;

            if (spriteRatio > rectRatio)
            {
                var oldHeight = rect.height;
                rect.height = rect.width * (1.0f / spriteRatio);
                rect.y += (oldHeight - rect.height) * rectTransform.pivot.y;
            }
            else
            {
                var oldWidth = rect.width;
                rect.width = rect.height * spriteRatio;
                rect.x += (oldWidth - rect.width) * rectTransform.pivot.x;
            }
        }

        private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
        {
            var padding = m_editOverrideSprite == null ? Vector4.zero : Sprites.DataUtility.GetPadding(m_editOverrideSprite);
            var size = m_editOverrideSprite == null ? Vector2.zero : new Vector2(m_editOverrideSprite.rect.width, m_editOverrideSprite.rect.height);

            Rect r = GetPixelAdjustedRect();
            // Debug.Log(string.Format("r:{2}, size:{0}, padding:{1}", size, padding, r));

            int spriteW = Mathf.RoundToInt(size.x);
            int spriteH = Mathf.RoundToInt(size.y);

            var v = new Vector4(
                padding.x / spriteW,
                padding.y / spriteH,
                (spriteW - padding.z) / spriteW,
                (spriteH - padding.w) / spriteH);

            if (shouldPreserveAspect && size.sqrMagnitude > 0.0f)
            {
                PreserveSpriteAspectRatio(ref r, size);
            }

            v = new Vector4(
                r.x + r.width * v.x,
                r.y + r.height * v.y,
                r.x + r.width * v.z,
                r.y + r.height * v.w
            );

            return v;
        }


        private void GenerateSprite(VertexHelper vh, bool lPreserveAspect)
        {
            var spriteSize = new Vector2(m_editOverrideSprite.rect.width, m_editOverrideSprite.rect.height);

            // Covert sprite pivot into normalized space.
            var spritePivot = m_editOverrideSprite.pivot / spriteSize;
            var rectPivot = rectTransform.pivot;
            Rect r = GetPixelAdjustedRect();

            if (lPreserveAspect & spriteSize.sqrMagnitude > 0.0f)
            {
                PreserveSpriteAspectRatio(ref r, spriteSize);
            }

            var drawingSize = new Vector2(r.width, r.height);
            var spriteBoundSize = m_editOverrideSprite.bounds.size;

            // Calculate the drawing offset based on the difference between the two pivots.
            var drawOffset = (rectPivot - spritePivot) * drawingSize;

            var color32 = color;
            vh.Clear();

            Vector2[] vertices = m_editOverrideSprite.vertices;
            Vector2[] uvs = m_editOverrideSprite.uv;
            for (int i = 0; i < vertices.Length; ++i)
            {
                vh.AddVert(new Vector3((vertices[i].x / spriteBoundSize.x) * drawingSize.x - drawOffset.x, (vertices[i].y / spriteBoundSize.y) * drawingSize.y - drawOffset.y), color32, new Vector2(uvs[i].x, uvs[i].y));
            }

            UInt16[] triangles = m_editOverrideSprite.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                vh.AddTriangle(triangles[i + 0], triangles[i + 1], triangles[i + 2]);
            }
        }
#endif
    }
}