using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class PlayerCard : MonoBehaviour
    {
        public TMP_Text Name;
        
        public Slider HP;
        public Slider HPBackground;

        public void PlayerInit()
        {
            HP.value = HP.maxValue;
            HPBackground.value = HPBackground.maxValue;
            Name.text = "Player";
        }
    }
}