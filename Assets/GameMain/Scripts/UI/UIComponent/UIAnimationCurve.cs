using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [System.Serializable]
    public class UICurve
    {
        public string name;
        public AnimationCurve curve;
    }

    public class UIAnimationCurve : MonoBehaviour
    {
        public List<UICurve> curves;

        public AnimationCurve GetAnimationCurveByName(string name)
        {
            if (curves != null)
            {
                foreach (var uiCurve in curves)
                {
                    if (uiCurve.name.Equals(name))
                    {
                        return uiCurve.curve;
                    }
                }
            }

            return null;
        }
    }
}