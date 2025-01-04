using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class Image3D : Image
    {
        public Vector3 pos = new Vector3(0, 0, 1);
        public Vector3 rotate = Vector3.zero;
        public Vector3 scale = Vector3.one;
        public float fov = 60;
        public float aspect = 1;
        public float near = 1;
        public float far = 1000;

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);

            var roteM = Matrix4x4.Rotate(Quaternion.Euler(rotate.x, rotate.y, rotate.z));
            var tranM = Matrix4x4.Translate(pos);
            var scaleM = Matrix4x4.Scale(scale);
            var perM = Matrix4x4.Perspective(30, 1, 1, 100);

            var center = Vector3.zero;
            center = roteM.MultiplyPoint(center);
            center = tranM.MultiplyPoint(center);
            center = perM.MultiplyPoint(center);

            for (int i = 0; i < toFill.currentVertCount; i++)
            {
                UIVertex vertex = new UIVertex();
                toFill.PopulateUIVertex(ref vertex, i);
                vertex.position = roteM.MultiplyPoint(vertex.position);
                vertex.position = tranM.MultiplyPoint(vertex.position);
                vertex.position = perM.MultiplyPoint(vertex.position);
                vertex.position = vertex.position - center;
                vertex.position = scaleM.MultiplyPoint(vertex.position);
                toFill.SetUIVertex(vertex, i);
            }
        }
    }
}