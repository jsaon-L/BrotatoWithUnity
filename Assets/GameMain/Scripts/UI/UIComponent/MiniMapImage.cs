using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class MiniMapImage : Image
    {
        public float lineWidth = 0.5f;

        public Vector2[] pos;

        public Vector2[] uvs;

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (pos==null||uvs==null|| pos.Length<4)
            {
                return;
            }
            toFill.Clear();
            toFill.AddUIVertexQuad(GetVertex(pos[0],pos[1]));
            toFill.AddUIVertexQuad(GetVertex(pos[1],pos[2]));
            toFill.AddUIVertexQuad(GetVertex(pos[2],pos[3]));
            toFill.AddUIVertexQuad(GetVertex(pos[3],pos[0]));
        }

        private UIVertex[] GetVertex(Vector2 startPos,Vector2 endPos)
        {
            startPos.x *= this.rectTransform.sizeDelta.x;
            startPos.x -= this.rectTransform.sizeDelta.x / 2;
            endPos.x *= this.rectTransform.sizeDelta.x;
            endPos.x -= this.rectTransform.sizeDelta.x / 2;
            startPos.y *= this.rectTransform.sizeDelta.y;
            startPos.y -= this.rectTransform.sizeDelta.y / 2;
            endPos.y *= this.rectTransform.sizeDelta.y;
            endPos.y -= this.rectTransform.sizeDelta.y / 2;
            float dis = Vector2.Distance(startPos, endPos);
            float y = lineWidth * 0.5f * (endPos.x - startPos.x) / dis;
            float x = lineWidth * 0.5f * (endPos.y - startPos.y) / dis;
            UIVertex[] vertex = new UIVertex[4];
            vertex[0].position = new Vector3(startPos.x + x, startPos.y + y);
            vertex[0].uv0 = uvs[0];
            vertex[1].position = new Vector3(endPos.x + x, endPos.y + y);
            vertex[1].uv0 = uvs[1];
            vertex[2].position = new Vector3(endPos.x - x, endPos.y - y);
            vertex[2].uv0 = uvs[2];
            vertex[3].position = new Vector3(startPos.x - x, startPos.y - y);
            vertex[3].uv0 = uvs[3];
            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i].color = this.color;
            }
            return vertex;
        }

        public void SetUvs(Vector2[] uvs)
        {
            this.uvs = uvs;
            this.SetVerticesDirty();
        }

        public void SetPos(Vector2[] pos)
        {
            this.pos = pos;
            this.SetVerticesDirty();
        }
    }
}

