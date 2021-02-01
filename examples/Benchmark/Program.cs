using System;
using System.Diagnostics;
using System.Linq;
using NcnnDotNet.OpenCV;
using ShellProgressBar;
using UltraFaceDotNet;

namespace Benchmark
{

    internal class Program
    {

        #region Methods

        private static int Main(string[] args)
        {
            if (args.Length <= 3)
            {
                Console.WriteLine($"Usage: {nameof(Benchmark)} < ncnn bin > < ncnn param >  < image files > < loop count >");
                return 1;
            }

            var binPath = args[0];
            var paramPath = args[1];
            var imagePath = args[2];
            var maxLoop = int.TryParse(args[3], out var ret) ? ret : 1000;

            var param = new UltraFaceParameter
            {
                BinFilePath = binPath,
                ParamFilePath = paramPath,
                InputWidth = 320,
                InputLength = 240,
                NumThread = 1,
                ScoreThreshold = 0.7f
            };

            using (var ultraFace = UltraFace.Create(param))// config model input
            {
                Console.WriteLine($"Processing {imagePath}");

                using var frame = Cv2.ImRead(imagePath);

                var imageStopWatch = new Stopwatch();
                var detectStopWatch = new Stopwatch();

                var totalImageLoad = 0d;
                var totalDetect = 0d;

                var options = new ProgressBarOptions
                {
                    ProgressCharacter = '─',
                    ForegroundColor = ConsoleColor.Yellow,
                    ForegroundColorDone = ConsoleColor.DarkGreen,
                    BackgroundColor = ConsoleColor.DarkGray,
                    ProgressBarOnBottom = true,
                };

                using (var pbar = new ProgressBar(maxLoop, "Initial message", options))
                    for (var loop = 1; loop <= maxLoop; loop++)
                    {
                        imageStopWatch.Start();
                        using var inMat = NcnnDotNet.Mat.FromPixels(frame.Data, NcnnDotNet.PixelType.Bgr2Rgb, frame.Cols, frame.Rows);
                        imageStopWatch.Stop();
                        totalImageLoad += imageStopWatch.ElapsedMilliseconds;

                        detectStopWatch.Start();
                        var _ = ultraFace.Detect(inMat).ToArray();
                        detectStopWatch.Stop();
                        totalDetect += detectStopWatch.ElapsedMilliseconds;

                        pbar.Tick($"Step {loop} of {maxLoop}");
                    }

                Console.WriteLine($"Total Loop {maxLoop}");
                Console.WriteLine($"\tConvert Image to Mat: Total {totalImageLoad} ms, Avg {totalImageLoad / maxLoop} ms");
                Console.WriteLine($"\t         Detect Face: Total {totalDetect} ms, Avg {totalDetect / maxLoop} ms");
                Console.WriteLine($"\t    Total Throughput: Total {totalImageLoad + totalDetect} ms, Avg {(totalImageLoad + totalDetect) / maxLoop} ms");
            }

            return 0;
        }

        #endregion

    }

}
