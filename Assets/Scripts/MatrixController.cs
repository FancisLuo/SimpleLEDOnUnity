using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LEDDemo
{
    public class MatrixController : MonoBehaviour
    {
        [SerializeField] private VecticalController[] vecticalLine;

        private int[] lineLightIndex = new int[8] { 0, 1, 1 << 2, 1 << 3, 1 << 4, 1 << 3, 1 << 2, 1 };

        // Start is called before the first frame update
        void Start()
        {
            if (vecticalLine == null || vecticalLine.Length != 8)
            {
                throw new ArgumentException($"{nameof(vecticalLine)} should have 8");
            }

            for(var index = 0; index < 8; ++index)
            {
                vecticalLine[index].ShowLight(lineLightIndex[index]);
            }
        }

        public void ShowMatrix(int[] indexMatrix)
        {
            if(indexMatrix == null || indexMatrix.Length != 8)
            {
                throw new ArgumentException($"{nameof(indexMatrix)} should have 8");
            }

            lineLightIndex = indexMatrix;
            DoShowMatrix();
        }

        private void DoShowMatrix()
        {
            for (var index = 0; index < 8; ++index)
            {
                vecticalLine[index].ShowLight(lineLightIndex[index]);
            }
        }
    }
}
