using Assets.Scripts.Fusion;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SkinChooseUIScript : BasicUIScript
    {
        [SerializeField] private SkinManager _skinManager;

        public List<Button> SkinButtonsList = new List<Button>();

        public Color DefaultSkinButtonColor;
        public Color ChosenSkinButonColor;

        // Use this for initialization
        void Start()
        {

        }

        private void UnselectAllSkins()
        {
            foreach (var SkinButton in SkinButtonsList)
            {
                SkinButton.image.color = DefaultSkinButtonColor;
            }
        }


        public void SetButtonColor(Button button)
        {
            UnselectAllSkins();

            button.image.color = ChosenSkinButonColor;
        }

        public void SetSkinIndex(int index)
        {
            _skinManager.SetSkinIndex(index);
        }
    }
}