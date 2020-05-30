using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LEDDemo
{
    public class LEDScreenController : MonoBehaviour
    {
        [SerializeField] private float moveInterval = 0.5f;
        [SerializeField] private MatrixController[] matrices;

        private int matrixCount;

        private int[] ledData;
        private int currentStartIndex = 0;
        private List<int> currentMatrix = new List<int>();
        private int totalLength = 0;

        private float shakeTime;

        // Start is called before the first frame update
        void Start()
        {
            if (matrices == null)
            {
                throw new ArgumentException($"{nameof(matrices)} should have value");
            }

            ledData = new int[] { 0, 1, 1 << 2, 1 << 3, 1 << 4, 1 << 3, 1 << 2, 1,
                                  0, 1, 1 << 2, 1 << 3, 1 << 4, 1 << 3, 1 << 2, 1 };
            currentStartIndex = 0;
            currentMatrix.Clear();
            totalLength = ledData.Length;
            matrixCount = matrices.Length;
            ShowLEDScreenMoving();
        }

        // Update is called once per frame
        void Update()
        {
            shakeTime += Time.deltaTime;

            if(shakeTime >= moveInterval)
            {
                currentStartIndex = (currentStartIndex + 1) % totalLength;
                ShowLEDScreenMoving();
                shakeTime = 0;
            }
        }

        private void ShowLEDScreenMoving()
        {
            int outIndex = 0;
            int innerIndex = 0;
            int count = 0;
            while(outIndex < matrixCount)
            {
                count = 0;
                currentMatrix.Clear();
                while (count < 8)
                {
                    var currentIndex = (innerIndex + currentStartIndex) % totalLength;

                    currentMatrix.Add(ledData[currentIndex]);
                    innerIndex = (innerIndex + 1) % totalLength;
                    ++count;
                }

                ShowLEDOne(outIndex, currentMatrix.ToArray());
                ++outIndex;
            }
        }

        private void ShowLEDOne(int matrixIndex, int[] matrix)
        {
            matrices[matrixIndex].ShowMatrix(matrix);
        }
    }
}
