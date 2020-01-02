using System;
using System.Linq;
using NcnnDotNet.OpenCV;
using UltraFaceDotNet;

namespace Demo
{

    internal class Program
    {

        #region Methods

        private static int Main(string[] args)
        {
            if (args.Length <= 2)
            {
                Console.WriteLine($"Usage: % s < ncnn bin > < ncnn param > [image files...]", nameof(Demo));
                return 1;
            }

            var binPath = args[0];
            var paramPath = args[1];

            var param = new UltraFaceParameter
            {
                BinFilePath = binPath,
                ParamFilePath = paramPath,
                InputWidth = 320, 
                InputLength = 240,
                NumThread = 1, 
                ScoreThreshold = 0.7f
            };

            using(var ultraFace = UltraFace.Create(param))// config model input
                for (var i = 2; i < args.Length; i++)
                {
                    var imageFile = args[i];
                    Console.WriteLine($"Processing {imageFile}");

                    using var frame = Cv2.ImRead(imageFile);
                    using var inMat = NcnnDotNet.Mat.FromPixels(frame.Data, NcnnDotNet.PixelType.Bgr2Rgb, frame.Cols, frame.Rows);

                    var faceInfos = ultraFace.Detect(inMat).ToArray();
                    for (var j = 0; j < faceInfos.Length; j++)
                    {
                        var face = faceInfos[j];
                        var pt1 = new Point<float>(face.X1, face.Y1);
                        var pt2 = new Point<float>(face.X2, face.Y2);
                        Cv2.Rectangle(frame, pt1, pt2, new Scalar<double>(0, 255, 0), 2);
                    }

                    Cv2.ImShow("UltraFace", frame);
                    Cv2.WaitKey();
                    Cv2.ImWrite("result.jpg", frame);
                }

            return 0;
        }

        #endregion

    }

}
