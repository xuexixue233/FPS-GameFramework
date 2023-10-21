﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class PlayerCard : MonoBehaviour
    {
        public TMP_Text weaponName;
        public Image HP;
        public GameObject weaponItem;
        public Image weaponModeImage;
        public TMP_Text currentBullets;
        public TMP_Text playerMaxBullets;

        public void Refresh(Player player)
        {
            HP.fillAmount = (float)player.m_PlayerData.HP / player.m_PlayerData.MaxHP;
            currentBullets.text = $"{player.showedWeapon.currentBullets}/{player.showedWeapon.currentMaxBullets}";
        }
    }
}