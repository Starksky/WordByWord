using SaintsField;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviours
{
    [ExecuteAlways]
    [RequireComponent(typeof(CanvasScaler))]
    public class AdaptiveView : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(CanvasScaler))]
        private CanvasScaler canvasScaler;

        private void LateUpdate()
        {
            canvasScaler.matchWidthOrHeight = (float)Screen.width / Screen.height > 0.5625f ? 1 : 0;
        }
    }
}
