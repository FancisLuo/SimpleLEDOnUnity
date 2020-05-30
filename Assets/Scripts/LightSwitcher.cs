using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LEDDemo
{
    public class LightSwitcher : MonoBehaviour
    {
        [SerializeField] private Material whiteMaterial;
        [SerializeField] private Material redMaterial;

        [SerializeField] private MeshRenderer meshRenderer;

        public void Switch(bool showRed)
        {
            if(showRed)
            {
                meshRenderer.material = redMaterial;
            }
            else
            {
                meshRenderer.material = whiteMaterial;
            }
        }
    }
}
