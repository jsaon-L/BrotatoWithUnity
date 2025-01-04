using UnityEngine;
using System.Collections;
namespace UnityEngine.UI
{
    /// <summary>
    /// 屏幕空白处点击的响应，不参与了绘制
    /// </summary>
    public class Empty4Raycast : MaskableGraphic
    {
        protected Empty4Raycast()
        {
            useLegacyMeshGeneration = false;
        }
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}