using GameFramework.Event;
using TMPro;
using UnityEngine.UI;

namespace FPS
{
    public class PlayerForm : UGuiForm
    {
        public ProcedureMain procedureMain;
        public PlayerCard PlayerCard;
        public GameOver gameOver;
        public GameWin gameWin;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(PlayerBulletsChangeEventArgs.EventId, OnPlayerBulletsChange);
            GameEntry.Event.Subscribe(WeaponBulletsChangeEventArgs.EventId,OnWeaponBulletsChange);
            GameEntry.Event.Subscribe(PlayerHPChangeEventArgs.EventId,OnPlayerHPChange);
            GameEntry.Event.Subscribe(WeaponFireModeChangeEventArgs.EventId,OnWeaponFireModeChange);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(PlayerBulletsChangeEventArgs.EventId, OnPlayerBulletsChange);
            GameEntry.Event.Unsubscribe(WeaponBulletsChangeEventArgs.EventId,OnWeaponBulletsChange);
            GameEntry.Event.Unsubscribe(PlayerHPChangeEventArgs.EventId,OnPlayerHPChange);
            GameEntry.Event.Unsubscribe(WeaponFireModeChangeEventArgs.EventId,OnWeaponFireModeChange);
            base.OnClose(isShutdown, userData);
        }
        
        private void OnPlayerBulletsChange(object sender, GameEventArgs e)
        {
            PlayerBulletsChangeEventArgs ne = (PlayerBulletsChangeEventArgs)e;
            if (ne == null)
                return;

            PlayerCard.playerMaxBullets.text = ne.CurrentBullets.ToString();
        }
        
        private void OnWeaponBulletsChange(object sender, GameEventArgs e)
        {
            WeaponBulletsChangeEventArgs ne = (WeaponBulletsChangeEventArgs)e;
            if (ne == null||sender is not Weapon weapon)
                return;

            PlayerCard.currentBullets.text = $"{ne.CurrentBullets}/{weapon.currentMaxBullets}";
        }
        
        private void OnPlayerHPChange(object sender, GameEventArgs e)
        {
            PlayerHPChangeEventArgs ne = (PlayerHPChangeEventArgs)e;
            if (ne == null||sender is not Player player)
                return;
            if (ne.CurrentHP!=ne.LastHP)
            {
                PlayerCard.ShowHurt();
            }
            PlayerCard.HP.fillAmount = (float)ne.CurrentHP / player.m_PlayerData.MaxHP;
        }
        
        private void OnWeaponFireModeChange(object sender, GameEventArgs e)
        {
            WeaponFireModeChangeEventArgs ne = (WeaponFireModeChangeEventArgs)e;
            if (ne == null)
                return;

            PlayerCard.weaponModeImage.sprite = PlayerCard.FireModeSprites[(int)ne.CurrentFireMode];
        }

        public void ShowGameOver()
        {
            gameOver.gameObject.SetActive(true);
            gameOver.OnShow();
        }

        public void ShowGameWin()
        {
            gameWin.gameObject.SetActive(false);
            gameWin.OnShow();
        }
    }
}