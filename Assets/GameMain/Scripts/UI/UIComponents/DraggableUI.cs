using UnityEngine;
using UnityEngine.EventSystems;

namespace FPS
{
    public class DraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private RectTransform rectTransform;
        private Vector3 offset;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // 计算鼠标点击位置与UI元素的偏移
            offset = transform.position - Input.mousePosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // 移动UI元素到鼠标位置加上偏移
            rectTransform.position = Input.mousePosition + offset;
        }
    
    }
}