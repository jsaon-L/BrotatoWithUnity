
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// 函数图基础 XY轴 刻度等
    /// </summary>
    [Serializable]
    public class FunctionalGraphBase
    {
        /// <summary>
        /// 是否显示刻度
        /// </summary>
        public bool ShowScale = false;
        /// <summary>
        /// 是否显示XY轴单位
        /// </summary>
        public bool ShowXYAxisUnit = true;
        /// <summary>
        /// X轴单位
        /// </summary>
        public string XAxisUnit = "XUnit";
        /// <summary>
        /// Y轴单位
        /// </summary>
        public string YAxisUnit = "YUnit";
        /// <summary>
        /// 单位字体大小
        /// </summary>
        [Range(12, 30)] public int FontSize = 16;

        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor = Color.black;
        /// <summary>
        /// XY轴刻度
        /// </summary>
        [Range(20f, 100f)] public float ScaleValue = 50f;
        /// <summary>
        /// 刻度的长度
        /// </summary>
        [Range(2, 10)] public float ScaleLenght = 5.0f;
        /// <summary>
        /// XY轴宽度
        /// </summary>
        [Range(2f, 20f)] public float XYAxisWidth = 2.0f;
        /// <summary>
        /// XY轴颜色
        /// </summary>
        public Color XYAxisColor = Color.gray;

        /// <summary>
        /// 网格Enum
        /// </summary>
        public enum E_MeshType
        {
            None,
            FullLine,
            ImaglinaryLine
        }
        /// <summary>
        /// 网格类型
        /// </summary>
        public E_MeshType MeshType = E_MeshType.None;
        /// <summary>
        /// 网格线段宽度
        /// </summary>
        [Range(1.0f, 10f)] public float MeshLineWidth = 2.0f;
        /// <summary>
        /// 网格颜色
        /// </summary>
        public Color MeshColor = Color.gray;
        /// <summary>
        /// 虚线的长度
        /// </summary>
        [Range(0.5f, 20)] public float ImaglinaryLineWidth = 8.0f;
        /// <summary>
        /// 虚线空格长度
        /// </summary>
        [Range(0.5f, 10f)] public float SpaceingWidth = 5.0f;
    }

    /// <summary>
    /// 折线节点
    /// </summary>
    [Serializable]
    public class FunctionFormula
    {
        /// <summary>
        /// 高度百分比
        /// </summary>
        public float heightPercent;

        /// <summary>
        /// 长度百分比
        /// </summary>
        public float lengthPercent;

        /// <summary>
        /// 函数图对应线条颜色
        /// </summary>
        public Color FormulaColor = Color.white;
        public float FormulaWidth;

        public FunctionFormula() { }
        public FunctionFormula(Color formulaColor, float width)
        {
            FormulaColor = formulaColor;
            FormulaWidth = width;
        }
    }

    public class PolyLineChart : Image
    {
        public FunctionalGraphBase GraphBase = new FunctionalGraphBase();
        public List<FunctionFormula> Formulas = new List<FunctionFormula>();
        private RectTransform _myRect;
        private Vector2 _xPoint;
        private Vector2 _yPoint;


        private void OnGUI()
        {
            if (GraphBase.ShowXYAxisUnit)
            {
                Vector3 result = transform.localPosition;
                Vector3 realPosition = getScreenPosition(transform, ref result);
                GUIStyle guiStyleX = new GUIStyle();
                guiStyleX.normal.textColor = GraphBase.FontColor;
                guiStyleX.fontSize = GraphBase.FontSize;
                guiStyleX.fontStyle = FontStyle.Bold;
                guiStyleX.alignment = TextAnchor.MiddleLeft;
                GUI.Label(new Rect(local2Screen(realPosition, _xPoint) + new Vector2(20, 0), new Vector2(0, 0)), GraphBase.XAxisUnit, guiStyleX);

                GUIStyle guiStyleY = new GUIStyle();
                guiStyleY.normal.textColor = GraphBase.FontColor;
                guiStyleY.fontSize = GraphBase.FontSize;
                guiStyleY.fontStyle = FontStyle.Bold;
                guiStyleY.alignment = TextAnchor.MiddleCenter;
                GUI.Label(new Rect(local2Screen(realPosition, _yPoint) - new Vector2(0, 20), new Vector2(0, 0)), GraphBase.YAxisUnit, guiStyleY);
            }
        }

        /// <summary>
        /// 初始化函数信息
        /// </summary>
        private void Init()
        {
            _myRect = this.rectTransform;
        }

        /// <summary>
        /// 重写这个类以绘制UI
        /// </summary>
        /// <param name="vh"></param>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            Init();
            vh.Clear();

            #region 基础框架的绘制

            if(GraphBase.ShowXYAxisUnit)
            {
                //绘制X轴
                float lenght = _myRect.sizeDelta.x;
                Vector2 leftPoint = new Vector2(-lenght / 2.0f, 0);
                Vector2 rightPoint = new Vector2(lenght / 2.0f, 0);
                vh.AddUIVertexQuad(GetQuad(leftPoint, rightPoint, GraphBase.XYAxisColor, GraphBase.XYAxisWidth));
                // 绘制X轴的箭头
                float arrowUnit = GraphBase.XYAxisWidth * 3;
                Vector2 firstPointX = rightPoint + new Vector2(0, arrowUnit);
                Vector2 secondPointX = rightPoint;
                Vector2 thirdPointX = rightPoint + new Vector2(0, -arrowUnit);
                Vector2 fourPointX = rightPoint + new Vector2(Mathf.Sqrt(3) * arrowUnit, 0);
                vh.AddUIVertexQuad(GetQuad(firstPointX, secondPointX, thirdPointX, fourPointX, GraphBase.XYAxisColor));
                //绘制Y轴
                float height = _myRect.sizeDelta.y;
                Vector2 downPoint = new Vector2(0, -height / 2.0f);
                Vector2 upPoint = new Vector2(0, height / 2.0f);
                vh.AddUIVertexQuad(GetQuad(downPoint, upPoint, GraphBase.XYAxisColor, GraphBase.XYAxisWidth));
                // 绘制Y轴的箭头
                Vector2 firstPointY = upPoint + new Vector2(arrowUnit, 0);
                Vector2 secondPointY = upPoint;
                Vector2 thirdPointY = upPoint + new Vector2(-arrowUnit, 0);
                Vector2 fourPointY = upPoint + new Vector2(0, Mathf.Sqrt(3) * arrowUnit);
                vh.AddUIVertexQuad(GetQuad(firstPointY, secondPointY, thirdPointY, fourPointY, GraphBase.XYAxisColor));

                _xPoint = rightPoint;
                _yPoint = upPoint;
            }

            #region 刻度的绘制

            if (GraphBase.ShowScale)
            {
                // X 轴的正方向
                for (int i = 1; i * GraphBase.ScaleValue < _myRect.sizeDelta.x / 2.0f; i++)
                {
                    Vector2 firstPoint = Vector2.zero + new Vector2(GraphBase.ScaleValue * i, 0);
                    Vector2 secongPoint = firstPoint + new Vector2(0, GraphBase.ScaleLenght);
                    vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.XYAxisColor));
                }
                // X 轴的负方向
                for (int i = 1; i * -GraphBase.ScaleValue > -_myRect.sizeDelta.x / 2.0f; i++)
                {
                    Vector2 firstPoint = Vector2.zero + new Vector2(-GraphBase.ScaleValue * i, 0);
                    Vector2 secongPoint = firstPoint + new Vector2(0, GraphBase.ScaleLenght);
                    vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.XYAxisColor));
                }
                // Y 轴正方向
                for (int y = 1; y * GraphBase.ScaleValue < _myRect.sizeDelta.y / 2.0f; y++)
                {
                    Vector2 firstPoint = Vector2.zero + new Vector2(0, y * GraphBase.ScaleValue);
                    Vector2 secongPoint = firstPoint + new Vector2(GraphBase.ScaleLenght, 0);
                    vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.XYAxisColor));
                }
                // Y 轴负方向
                for (int y = 1; y * -GraphBase.ScaleValue > -_myRect.sizeDelta.y / 2.0f; y++)
                {
                    Vector2 firstPoint = Vector2.zero + new Vector2(0, y * -GraphBase.ScaleValue);
                    Vector2 secongPoint = firstPoint + new Vector2(GraphBase.ScaleLenght, 0);
                    vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.XYAxisColor));
                }
            }

            #endregion

            switch (GraphBase.MeshType)
            {
                case FunctionalGraphBase.E_MeshType.None:
                    break;
                case FunctionalGraphBase.E_MeshType.FullLine:
                    // X 轴的正方向
                    for (int i = 1; i * GraphBase.ScaleValue < _myRect.sizeDelta.x / 2.0f; i++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(GraphBase.ScaleValue * i, -_myRect.sizeDelta.y / 2.0f);
                        Vector2 secongPoint = firstPoint + new Vector2(0, _myRect.sizeDelta.y);
                        vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.MeshColor, GraphBase.MeshLineWidth));
                    }
                    // X 轴的负方向
                    for (int i = 1; i * -GraphBase.ScaleValue > -_myRect.sizeDelta.x / 2.0f; i++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-GraphBase.ScaleValue * i, -_myRect.sizeDelta.y / 2.0f);
                        Vector2 secongPoint = firstPoint + new Vector2(0, _myRect.sizeDelta.y);
                        vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.MeshColor, GraphBase.MeshLineWidth));
                    }
                    // Y 轴正方向
                    for (int y = 1; y * GraphBase.ScaleValue < _myRect.sizeDelta.y / 2.0f; y++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-_myRect.sizeDelta.x / 2.0f, y * GraphBase.ScaleValue);
                        Vector2 secongPoint = firstPoint + new Vector2(_myRect.sizeDelta.x, 0);
                        vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.MeshColor, GraphBase.MeshLineWidth));
                    }
                    // Y 轴负方向
                    for (int y = 1; y * -GraphBase.ScaleValue > -_myRect.sizeDelta.y / 2.0f; y++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-_myRect.sizeDelta.x / 2.0f, -y * GraphBase.ScaleValue);
                        Vector2 secongPoint = firstPoint + new Vector2(_myRect.sizeDelta.x, 0);
                        vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.MeshColor, GraphBase.MeshLineWidth));
                    }
                    break;
                case FunctionalGraphBase.E_MeshType.ImaglinaryLine:
                    // X 轴的正方向
                    for (int i = 1; i * GraphBase.ScaleValue < _myRect.sizeDelta.x / 2.0f; i++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(GraphBase.ScaleValue * i, -_myRect.sizeDelta.y / 2.0f);
                        Vector2 secondPoint = firstPoint + new Vector2(0, _myRect.sizeDelta.y);
                        GetImaglinaryLine(ref vh, firstPoint, secondPoint, GraphBase.MeshColor, GraphBase.ImaglinaryLineWidth, GraphBase.SpaceingWidth);
                    }
                    // X 轴的负方向
                    for (int i = 1; i * -GraphBase.ScaleValue > -_myRect.sizeDelta.x / 2.0f; i++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-GraphBase.ScaleValue * i, -_myRect.sizeDelta.y / 2.0f);
                        Vector2 secondPoint = firstPoint + new Vector2(0, _myRect.sizeDelta.y);
                        GetImaglinaryLine(ref vh, firstPoint, secondPoint, GraphBase.MeshColor, GraphBase.ImaglinaryLineWidth, GraphBase.SpaceingWidth);
                    }
                    // Y 轴正方向
                    for (int y = 1; y * GraphBase.ScaleValue < _myRect.sizeDelta.y / 2.0f; y++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-_myRect.sizeDelta.x / 2.0f, y * GraphBase.ScaleValue);
                        Vector2 secondPoint = firstPoint + new Vector2(_myRect.sizeDelta.x, 0);
                        GetImaglinaryLine(ref vh, firstPoint, secondPoint, GraphBase.MeshColor, GraphBase.ImaglinaryLineWidth, GraphBase.SpaceingWidth);
                    }
                    // Y 轴负方向
                    for (int y = 1; y * -GraphBase.ScaleValue > -_myRect.sizeDelta.y / 2.0f; y++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-_myRect.sizeDelta.x / 2.0f, -y * GraphBase.ScaleValue);
                        Vector2 secondPoint = firstPoint + new Vector2(_myRect.sizeDelta.x, 0);
                        GetImaglinaryLine(ref vh, firstPoint, secondPoint, GraphBase.MeshColor, GraphBase.ImaglinaryLineWidth, GraphBase.SpaceingWidth);
                    }
                    break;
            }

            #endregion

            #region 函数图的绘制
            if (Formulas.Count==1)
            {
                Vector2 startPos = new Vector2(0, _myRect.sizeDelta.y * Formulas[0].heightPercent);
                Vector2 endPos = new Vector2(_myRect.sizeDelta.x * Formulas[0].lengthPercent, _myRect.sizeDelta.y * Formulas[0].heightPercent);
                vh.AddUIVertexQuad(GetQuad(startPos, endPos, Formulas[0].FormulaColor, Formulas[0].FormulaWidth));
            }
            else
            {
                Vector2 offset = new Vector2(_myRect.sizeDelta.x/2, _myRect.sizeDelta.y/2);
                for (int i = 0; i < Formulas.Count - 1; i++)
                {
                    Vector2 startPos = new Vector2(_myRect.sizeDelta.x * Formulas[i].lengthPercent, _myRect.sizeDelta.y * Formulas[i].heightPercent)- offset;
                    Vector2 endPos = new Vector2(_myRect.sizeDelta.x * Formulas[i+1].lengthPercent, _myRect.sizeDelta.y * Formulas[i + 1].heightPercent)- offset;
                    vh.AddUIVertexQuad(GetQuad(startPos, endPos, Formulas[i].FormulaColor, Formulas[i].FormulaWidth));
                }
            }
            #endregion
        }

        //通过两个端点绘制矩形
        private UIVertex[] GetQuad(Vector2 startPos, Vector2 endPos, Color color0, float lineWidth = 2.0f)
        {
            float dis = Vector2.Distance(startPos, endPos);
            float y = lineWidth * 0.5f * (endPos.x - startPos.x) / dis;
            float x = lineWidth * 0.5f * (endPos.y - startPos.y) / dis;
            if (y <= 0) y = -y;
            else x = -x;
            UIVertex[] vertex = new UIVertex[4];
            vertex[0].position = new Vector3(startPos.x + x, startPos.y + y);
            vertex[0].uv0 = new Vector2(0, 1);
            vertex[1].position = new Vector3(endPos.x + x, endPos.y + y);
            vertex[1].uv0 = new Vector2(1,1);
            vertex[2].position = new Vector3(endPos.x - x, endPos.y - y);
            vertex[2].uv0 = new Vector2(1,0);
            vertex[3].position = new Vector3(startPos.x - x, startPos.y - y);
            vertex[3].uv0 = new Vector2(0,0);
            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i].color = color0;
            }
            return vertex;
        }

        //通过四个顶点绘制矩形
        private UIVertex[] GetQuad(Vector2 first, Vector2 second, Vector2 third, Vector2 four, Color color0)
        {
            UIVertex[] vertexs = new UIVertex[4];
            vertexs[0] = GetUIVertex(first, color0);
            vertexs[1] = GetUIVertex(second, color0);
            vertexs[2] = GetUIVertex(third, color0);
            vertexs[3] = GetUIVertex(four, color0);
            return vertexs;
        }

        //构造UIVertex
        private UIVertex GetUIVertex(Vector2 point, Color color0)
        {
            UIVertex vertex = new UIVertex
            {
                position = point,
                color = color0,
                uv0 = new Vector2(0, 0)
            };
            return vertex;
        }

        //绘制虚线
        private void GetImaglinaryLine(ref VertexHelper vh, Vector2 first, Vector2 second, Color color0, float imaginaryLenght, float spaceingWidth, float lineWidth = 2.0f)
        {
            if (first.y.Equals(second.y)) //  X轴
            {
                Vector2 indexSecond = first + new Vector2(imaginaryLenght, 0);
                while (indexSecond.x < second.x)
                {
                    vh.AddUIVertexQuad(GetQuad(first, indexSecond, color0));
                    first = indexSecond + new Vector2(spaceingWidth, 0);
                    indexSecond = first + new Vector2(imaginaryLenght, 0);
                    if (indexSecond.x > second.x)
                    {
                        indexSecond = new Vector2(second.x, indexSecond.y);
                        vh.AddUIVertexQuad(GetQuad(first, indexSecond, color0));
                    }
                }
            }
            if (first.x.Equals(second.x)) //  Y轴
            {
                Vector2 indexSecond = first + new Vector2(0, imaginaryLenght);
                while (indexSecond.y < second.y)
                {
                    vh.AddUIVertexQuad(GetQuad(first, indexSecond, color0));
                    first = indexSecond + new Vector2(0, spaceingWidth);
                    indexSecond = first + new Vector2(0, imaginaryLenght);
                    if (indexSecond.y > second.y)
                    {
                        indexSecond = new Vector2(indexSecond.x, second.y);
                        vh.AddUIVertexQuad(GetQuad(first, indexSecond, color0));
                    }
                }
            }
        }

        //本地坐标转化屏幕坐标绘制GUI文字
        private Vector2 local2Screen(Vector2 parentPos, Vector2 localPosition)
        {
            Vector2 pos = localPosition + parentPos;
            float xValue, yValue = 0;
            if (pos.x > 0)
                xValue = pos.x + Screen.width / 2.0f;
            else
                xValue = Screen.width / 2.0f - Mathf.Abs(pos.x);
            if (pos.y > 0)
                yValue = Screen.height / 2.0f - pos.y;
            else
                yValue = Screen.height / 2.0f + Mathf.Abs(pos.y);
            return new Vector2(xValue, yValue);
        }

        //递归计算位置
        private Vector2 getScreenPosition(Transform trans, ref Vector3 result)
        {
            if (null != trans.parent && null != trans.parent.parent)
            {
                result += trans.parent.localPosition;
                getScreenPosition(trans.parent, ref result);
            }
            if (null != trans.parent && null == trans.parent.parent)
                return result;
            return result;
        }
    }
}