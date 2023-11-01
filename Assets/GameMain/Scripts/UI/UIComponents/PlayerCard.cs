using System.Collections.Generic;
using TMPro;
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

        public List<Sprite> FireModeSprites;

        public void OnInit(Player player)
        {
            weaponModeImage.sprite = FireModeSprites[(int)player.showedWeapon.currentFireMode];
            HP.fillAmount = (float)player.m_PlayerData.HP / player.m_PlayerData.MaxHP;
            weaponName.text = player.showedWeapon.m_WeaponData.WeaponName;
            currentBullets.text = $"{player.showedWeapon.currentBullets}/{player.showedWeapon.currentMaxBullets}";
            playerMaxBullets.text = player.playerMaxBullets.ToString();
        }
    }
}