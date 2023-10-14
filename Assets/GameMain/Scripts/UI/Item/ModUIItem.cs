using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class ModUIItem : MonoBehaviour
    {
        public Button modButton;
        public Image modImage;
        public TMP_Text modText;
        public Mod _mod;
        public GameObject modList;
        public GameObject showImage;
        public GameObject hideImage;
        private Entity m_Owner = null;
        public Entity Owner => m_Owner;

        public void Init(Mod mod,Entity entity)
        {
            _mod = mod;
            m_Owner = entity;
            modButton.onClick.RemoveAllListeners();
            modButton.onClick.AddListener((() =>
            {
                modList.SetActive(!modList.activeSelf);
                showImage.SetActive(!showImage.activeSelf);
                hideImage.SetActive(!hideImage.activeSelf);
            }));
        }

        public void Reset()
        {
            
        }
    }
}