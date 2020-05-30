- 想法来源
    当别人提起汇编的时候，想起来以前用C51（或者汇编）写代码控制LED灯的显示，以及文字的移动等功能，于是想在Unity中也实现一个简易的LED灯显示及移动的效果，纯当练练手。

- 实现
    1. 先实现单个点的红色和白色的切换，来模拟LED灯的点的显示（只能单纯模拟灯的显示，单片机驱动LED显示的部分需查专门资料了解）。切换实现：
    ```
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
    ```
    2. 新建一个GameObject，重命名为VerticalLine，即单列的点，放8个，然后根据数据来显示8个中的哪几个点灯。实现：
    ```
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
    ```
    3. 新建一个GameObject，重命名为Matrix，实现8*8的点阵，作为一片（LED屏幕可以由多块点阵拼凑而成，这里选择一小片为8*8），实现其显示：
    ```
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
    ```
    3. 最后，加两片矩阵片连起来试试，并且可以实现移动效果：
    ```
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
    ```
    4. 这样就可以简单的实现一个LED灯的显示了，可以扩展更高密度的LED屏幕。