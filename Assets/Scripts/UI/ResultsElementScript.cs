using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ResultsElementScript : MonoBehaviour
    {
        public TMP_Text PlayerText;
        public TMP_Text DamageText;
        public TMP_Text KillsText;
        public TMP_Text StatusText;
        public Image PlayerSkin;

        public void NewResultElement(
            string PlayerString,
            int Damage, int Kills, string status, Sprite Skin)
        {
            PlayerText.text = PlayerString;
            DamageText.text = Damage.ToString();
            KillsText.text = Kills.ToString();
            StatusText.text = status;
            PlayerSkin.sprite = Skin;
        }
    }
}