using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NcnnDotNet;

namespace UltraFaceDotNet
{

    /// <summary>
    /// Provides the method to find face methods. This class cannot be inherited.
    /// </summary>
    public sealed class UltraFace : DisposableObject
    {

        #region Fields

        private const float CenterVariance = 0.1f;

        private const float SizeVariance = 0.2f;

        private const int NumFeatureMap = 4;

        private readonly float[] _Strides = { 8.0f, 16.0f, 32.0f, 64.0f };

        private readonly float[] _MeanVals = { 127f, 127f, 127f };

        private readonly float[] _NormVals = { (float)(1.0 / 128), (float)(1.0 / 128), (float)(1.0 / 128) };

        private readonly float[][] _MinBoxes =
        {
            new[]{10.0f,  16.0f,  24.0f},
            new[]{32.0f,  48.0f},
            new[]{64.0f,  96.0f},
            new[]{128.0f, 192.0f, 256.0f}
        };

        private readonly int _NumThread;

        private readonly int _InW;

        private readonly int _InH;

        private readonly int _NumAnchors;

        private readonly int _TopK;

        private readonly float _ScoreThreshold;

        private readonly float _IouThreshold;

        private readonly IList<float[]> _FeatureMapSize = new List<float[]>();

        private readonly IList<float[]> _ShrinkageSize = new List<float[]>();

        private readonly IList<float[]> _Priors = new List<float[]>();

        private readonly Net _UltraFace;

        private int _ImageW;

        private int _ImageH;

        #endregion

        #region Constructors

        private UltraFace(UltraFaceParameter parameter)
        {
            this._NumThread = parameter.NumThread;
            this._TopK = parameter.TopK;
            this._ScoreThreshold = parameter.ScoreThreshold;
            this._IouThreshold = parameter.IouThreshold;
            this._InW = parameter.InputWidth;
            this._InH = parameter.InputLength;

            var whList = new float[] { parameter.InputWidth, parameter.InputLength };

            foreach (var size in whList)
            {
                var featureMapItem = this._Strides.Select(stride => (float)Math.Ceiling(size / stride)).ToArray();
                this._FeatureMapSize.Add(featureMapItem);
            }

            foreach (var _ in whList)
            {
                this._ShrinkageSize.Add(this._Strides);
            }

            for (var index = 0; index < NumFeatureMap; index++)
            {
                var scaleW = this._InW / this._ShrinkageSize[0][index];
                var scaleH = this._InH / this._ShrinkageSize[1][index];
                for (var j = 0; j < this._FeatureMapSize[1][index]; j++)
                    for (var i = 0; i < this._FeatureMapSize[0][index]; i++)
                    {
                        var xCenter = (float)((i + 0.5) / scaleW);
                        var yCenter = (float)((j + 0.5) / scaleH);

                        foreach (var k in this._MinBoxes[index])
                        {
                            var w = k / this._InW;
                            var h = k / this._InH;

                            this._Priors.Add(
                                new[]
                                {
                                Clip(xCenter, 1),
                                Clip(yCenter, 1),
                                Clip(w, 1),
                                Clip(h, 1)
                                });
                        }
                    }
            }

            this._NumAnchors = this._Priors.Count;

            this._UltraFace = new Net();
            this._UltraFace.LoadParam(parameter.ParamFilePath);
            this._UltraFace.LoadModel(parameter.BinFilePath);
            this._UltraFace.Opt.UseVulkanCompute = parameter.UseGpu;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new instance of the <see cref="UltraFace"/> class with the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The <see cref="UltraFace"/> this method creates.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="parameter"/> is null.</exception>
        /// <exception cref="ArgumentException">The model binary file is null or whitespace. Or the param file is null or whitespace.</exception>
        /// <exception cref="FileNotFoundException">The model binary file is not found. Or the param file is not found.</exception>
        public static UltraFace Create(UltraFaceParameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (string.IsNullOrWhiteSpace(parameter.BinFilePath))
                throw new ArgumentException("The model binary file is null or whitespace", nameof(parameter.BinFilePath));
            if (string.IsNullOrWhiteSpace(parameter.ParamFilePath))
                throw new ArgumentException("The param file is null or whitespace", nameof(parameter.ParamFilePath));
            if (!File.Exists(parameter.BinFilePath))
                throw new FileNotFoundException("The model binary file is not found.");
            if (!File.Exists(parameter.ParamFilePath))
                throw new FileNotFoundException("The param file is not found.");

            return new UltraFace(parameter);
        }

        /// <summary>
        /// Destroy gpu instance.
        /// </summary>
        public static void DestroyGpu()
        {
            if (Ncnn.IsSupportVulkan)
                Ncnn.DestroyGpuInstance();
        }

        /// <summary>
        /// Returns an enumerable collection of face location correspond to all faces in specified image.
        /// </summary>
        /// <param name="image">The image contains faces. The image can contain multiple faces.</param>
        /// <returns>An enumerable collection of face location correspond to all faces in specified image.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="image"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="image"/> is empty.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="image"/> or this object is disposed.</exception>
        public IEnumerable<FaceInfo> Detect(Mat image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            this.ThrowIfDisposed();
            image.ThrowIfDisposed();

            var faceList = new List<FaceInfo>();
            if (Detect(image, faceList) != 0)
                throw new ArgumentException("Image is empty.", nameof(image));

            return faceList.ToArray();
        }

        /// <summary>
        /// Initialize gpu instance if the host machine supports.
        /// </summary>
        /// <returns><code>true</code> if the host machine supports GPU; otherwise, <code>false</code>.</returns>
        public static bool InitializeGpu()
        {
            if (Ncnn.IsSupportVulkan)
            {
                Ncnn.CreateGpuInstance();
                return true;
            }

            return false;
        }

        #region Overrides 

        /// <summary>
        /// Releases all unmanaged resources.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();

            this._UltraFace?.Dispose();
        }

        #endregion

        #region Helpers

        private static float Clip(double x, float y)
        {
            return (float)(x < 0 ? 0 : x > y ? y : x);
        }

        private int Detect(Mat image, ICollection<FaceInfo> faceList)
        {
            if (image.IsEmpty)
            {
                Console.WriteLine("image is empty ,please check!");
                return -1;
            }

            this._ImageH = image.H;
            this._ImageW = image.W;

            using (var @in = new Mat())
            {
                Ncnn.ResizeBilinear(image, @in, this._InW, this._InH);

                var ncnnImg = @in;
                ncnnImg.SubstractMeanNormalize(this._MeanVals, this._NormVals);

                var boundingBoxCollection = new List<FaceInfo>();
                //var validInput = new List<FaceInfo>();

                using (var ex = this._UltraFace.CreateExtractor())
                {
                    ex.SetNumThreads(this._NumThread);
                    ex.Input("input", ncnnImg);

                    using (var scores = new Mat())
                    using (var boxes = new Mat())
                    {
                        ex.Extract("scores", scores);
                        ex.Extract("boxes", boxes);

                        GenerateBBox(boundingBoxCollection, scores, boxes, this._ScoreThreshold, this._NumAnchors);
                        NonMaximumSuppression(boundingBoxCollection, faceList);
                    }
                }
            }

            return 0;
        }

        private void GenerateBBox(ICollection<FaceInfo> boundingBoxCollection, Mat scores, Mat boxes, float scoreThreshold, int numAnchors)
        {
            using (var scoresChannel = scores.Channel(0))
            using (var boxesChannel = boxes.Channel(0))
            {
                for (var i = 0; i < numAnchors; i++)
                    if (scoresChannel[i * 2 + 1] > scoreThreshold)
                    {
                        var rects = new FaceInfo();
                        var xCenter = boxesChannel[i * 4] * CenterVariance * this._Priors[i][2] + this._Priors[i][0];
                        var yCenter = boxesChannel[i * 4 + 1] * CenterVariance * this._Priors[i][3] + this._Priors[i][1];
                        var w = Math.Exp(boxesChannel[i * 4 + 2] * SizeVariance) * this._Priors[i][2];
                        var h = Math.Exp(boxesChannel[i * 4 + 3] * SizeVariance) * this._Priors[i][3];

                        rects.X1 = Clip(xCenter - w / 2.0, 1) * this._ImageW;
                        rects.Y1 = Clip(yCenter - h / 2.0, 1) * this._ImageH;
                        rects.X2 = Clip(xCenter + w / 2.0, 1) * this._ImageW;
                        rects.Y2 = Clip(yCenter + h / 2.0, 1) * this._ImageH;
                        rects.Score = Clip(scoresChannel[i * 2 + 1], 1);

                        boundingBoxCollection.Add(rects);
                    }
            }
        }

        private void NonMaximumSuppression(List<FaceInfo> input, ICollection<FaceInfo> output, NonMaximumSuppressionMode type = NonMaximumSuppressionMode.Blending)
        {
            input.Sort((f1, f2) => f1.Score.CompareTo(f2.Score));

            var boxNum = input.Count;

            var merged = new int[boxNum];

            for (var i = 0; i < boxNum; i++)
            {
                if (merged[i] > 0)
                    continue;

                var buf = new List<FaceInfo>
                {
                    input[i]
                };

                merged[i] = 1;

                var h0 = input[i].Y2 - input[i].Y1 + 1;
                var w0 = input[i].X2 - input[i].X1 + 1;

                var area0 = h0 * w0;

                for (var j = i + 1; j < boxNum; j++)
                {
                    if (merged[j] > 0)
                        continue;

                    var innerX0 = input[i].X1 > input[j].X1 ? input[i].X1 : input[j].X1;
                    var innerY0 = input[i].Y1 > input[j].Y1 ? input[i].Y1 : input[j].Y1;

                    var innerX1 = input[i].X2 < input[j].X2 ? input[i].X2 : input[j].X2;
                    var innerY1 = input[i].Y2 < input[j].Y2 ? input[i].Y2 : input[j].Y2;

                    var innerH = innerY1 - innerY0 + 1;
                    var innerW = innerX1 - innerX0 + 1;

                    if (innerH <= 0 || innerW <= 0)
                        continue;

                    var innerArea = innerH * innerW;

                    var h1 = input[j].Y2 - input[j].Y1 + 1;
                    var w1 = input[j].X2 - input[j].X1 + 1;

                    var area1 = h1 * w1;

                    var score = innerArea / (area0 + area1 - innerArea);

                    if (score > this._IouThreshold)
                    {
                        merged[j] = 1;
                        buf.Add(input[j]);
                    }
                }

                switch (type)
                {
                    case NonMaximumSuppressionMode.Hard:
                        {
                            output.Add(buf[0]);
                            break;
                        }
                    case NonMaximumSuppressionMode.Blending:
                        {
                            var total = 0d;
                            for (var j = 0; j < buf.Count; j++)
                            {
                                total += Math.Exp(buf[j].Score);
                            }

                            var rects = new FaceInfo();
                            for (var j = 0; j < buf.Count; j++)
                            {
                                var rate = Math.Exp(buf[j].Score) / total;
                                rects.X1 += (float)(buf[j].X1 * rate);
                                rects.Y1 += (float)(buf[j].Y1 * rate);
                                rects.X2 += (float)(buf[j].X2 * rate);
                                rects.Y2 += (float)(buf[j].Y2 * rate);
                                rects.Score += (float)(buf[j].Score * rate);
                            }

                            output.Add(rects);
                            break;
                        }
                    default:
                        //{
                        //    Console.WriteLine("wrong type of nms.");
                        //    exit(-1);
                        //}
                        break;
                }
            }

            #endregion

            #endregion

        }

    }

}
