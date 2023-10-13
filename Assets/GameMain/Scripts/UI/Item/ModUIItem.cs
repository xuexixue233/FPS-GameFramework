using GameFramework.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class ModUIItem : MonoBehaviour
    {
        public Button modButton;
        public Image modImage;
        private Entity m_Owner = null;
        
        public Entity Owner => m_Owner;

        
        
    }
}