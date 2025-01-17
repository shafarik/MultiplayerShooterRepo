﻿using Fusion;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class BasicUIScript : NetworkBehaviour
    {
        [SerializeField] protected Canvas canvas;

        public virtual void ShowCanvas()
        {
            canvas.enabled = true;
        }
        public virtual void HideCanvas()
        {
            canvas.enabled = false;
        }
    }
}