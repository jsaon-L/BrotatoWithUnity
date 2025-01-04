
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class UIRenderTexture : MonoBehaviour
    {
        RawImage m_RawImage;
        public Camera m_Camera;
        RenderTexture m_RenderTexture;
        void Start()
        {
            //Fetch the RawImage component from the GameObject
            m_RawImage = GetComponent<RawImage>();
            var rectTransform = GetComponent<RectTransform>();
            //Change the Texture to be the one you define in the Inspector
            if (m_RawImage != null)
            {
                m_RenderTexture = new RenderTexture((int)rectTransform.rect.size.x, (int)rectTransform.rect.size.y, 0);
                m_RawImage.texture = m_RenderTexture;
            }
            if(m_Camera != null)
            {
                m_Camera.targetTexture = m_RenderTexture;
            }
        }

        private void OnDestroy()
        {
            if (m_RenderTexture != null)
            {
                m_RenderTexture.Release();
            }
            m_RenderTexture = null;
            m_RawImage.texture = null;
        }
    }
}