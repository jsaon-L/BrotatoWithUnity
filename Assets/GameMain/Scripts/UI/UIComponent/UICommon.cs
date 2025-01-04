using Skyunion;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public static class UICommon
    {
        public static void ResetAnimation(GameObject go, string ani_name)
        {
            Animation component = go.GetComponent<Animation>();
            if (component != null && component[ani_name] != null)
            {
                component[ani_name].time = 0f;
                component.Sample();
                component.Stop(ani_name);
            }
        }

        /// <summary>
        /// hex rgbת����color
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color HexRGBToColor(string hex)
        {
            byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            float r = br / 255f;
            float g = bg / 255f;
            float b = bb / 255f;
            return new Color(r, g, b, 1.0f);
        }
        
        public static void SetLocalEulerAngles(GameObject go, float z)
        {
            go.transform.localEulerAngles = new Vector3(0f, 0f, z);
        }
        
        public static void SetRectTransformSizeDelta(RectTransform rt, float x, float y)
        {
            rt.sizeDelta = new Vector2(x, y);
        }
        
        
        //public static void WorldToUIPos(GameObject go_canvas, float x, float y, float z, out float rx, out float ry)
        //{
        //    Vector3 vector = WorldCamera.Instance().GetCamera().WorldToScreenPoint(new Vector3(x, y, z));
        //    RectTransform component = go_canvas.GetComponent<RectTransform>();
        //    Vector2 sizeDelta = component.sizeDelta;
        //    float x2 = sizeDelta.x;
        //    Vector2 sizeDelta2 = component.sizeDelta;
        //    float y2 = sizeDelta2.y;
        //    rx = vector.x * (x2 / (float)Screen.width);
        //    ry = vector.y * (y2 / (float)Screen.height);
        //}

    }
}