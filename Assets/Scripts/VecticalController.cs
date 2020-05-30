using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEDDemo
{
    public class VecticalController : MonoBehaviour
    {
        [SerializeField] private LightSwitcher[] lights;

#if UNITY_EDITOR
        [SerializeField]
#endif
        private int lightIndex = 1;

        // Start is called before the first frame update
        void Start()
        {
            if (lights == null || lights.Length != 8)
            {
                throw new ArgumentException($"{nameof(lights)} should have 8");
            }

            //ShowLightSequence();
        }

        public void ShowLight(int index)
        {
            lightIndex = index;
            ShowLightSequence();
        }

        private void ShowLightSequence()
        {
            for(var i = 0; i < 8; ++i)
            {
                var index = 1 << i;
                bool showRed = (index & lightIndex) == index;
                lights[i].Switch(showRed);
            }
        }
    }
}
