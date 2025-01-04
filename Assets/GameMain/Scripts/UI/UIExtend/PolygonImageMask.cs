using System;
using System.Collections.Generic;
using GameFramework;
using Skyunion;

namespace UnityEngine.UI
{
    public class PolygonImageMask : PolygonImage
    {
        [SerializeField]
        public Sprite maskSprite;
        Material cloneMaterial = null;

        protected override void Start()
        {
            if (Application.isPlaying)
            {
                cloneMaterial = Instantiate(material);
                material = cloneMaterial;
            }
        }
        public void SetMaterial(Material material)
        {
            if (material != null && cloneMaterial != material)
            {
                cloneMaterial = Instantiate(material);
                this.material = cloneMaterial;
            }
        }
        protected override void UpdateMaterial()
        {
            if (IsActive())
            {
                canvasRenderer.materialCount = 1;
#if !UNITY_EDITOR
                canvasRenderer.SetMaterial(cloneMaterial, 0);
#else
                canvasRenderer.SetMaterial(material, 0);
#endif
                canvasRenderer.SetTexture(mainTexture);

#if !UNITY_EDITOR
                cloneMaterial.SetTexture("_MaskTex", maskSprite.texture);
#else
                material.SetTexture("_MaskTex", maskSprite.texture);
#endif
                base.UpdateMaterial();
            }
        }
    }
}