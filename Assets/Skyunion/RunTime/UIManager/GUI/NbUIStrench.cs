/////////////////////////////////////////////////////////////////////////////////
// @desc UI全屏等比拉伸
// @copyright ©2018 iGG
// @release 2018年4月28日 星期六
// @author BobWong
// @mail 15959187562@qq.com
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class NbUIStrench : MonoBehaviour
{

	// Use this for initialization
	public void Awake ()
	{
		float width = Screen.width;
        float height = Screen.height;
        float designWidth = 1440;//开发时分辨率宽
        float designHeight = 810;//开发时分辨率高
        float newDesignWidth = designWidth;//开发时分辨率宽
        float newDesignHeight = designHeight;//开发时分辨率高

		float s1 = (float)designWidth / (float)designHeight;
		float s2 = (float)width / (float)height;

        if (s1 < s2)
        {
            newDesignWidth = (int)Mathf.FloorToInt(designHeight * s2);
            newDesignHeight = (int)(((float)newDesignWidth / designWidth) * designHeight);
        }
        else if (s1 > s2)
        {
            newDesignHeight = (int)Mathf.FloorToInt(designWidth / s2);
            newDesignWidth = (int)(((float)newDesignHeight / designHeight) * designWidth);
        }

        RectTransform rectTransform = this.transform as RectTransform;
		if (rectTransform != null) {
			rectTransform.sizeDelta = new Vector2 (newDesignWidth, newDesignHeight);
		}
	}

}
