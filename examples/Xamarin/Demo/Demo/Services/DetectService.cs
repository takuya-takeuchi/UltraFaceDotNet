﻿using System;
using System.Collections.Generic;
using System.IO;
using NcnnDotNet;
using NcnnDotNet.OpenCV;
using Demo.Models;
using Demo.Services.Interfaces;
using Mat = NcnnDotNet.Mat;
using Object = Demo.Models.Object;

namespace Demo.Services
{

    public sealed class DetectService : IDetectService
    {

        #region Constructors

        public DetectService()
        {
            var resourcePrefix = $"Demo.data.";
            // note that the prefix includes the trailing period '.' that is required
            var files = new [] { "RFB-320.bin", "RFB-320.param" };
            var assembly = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(DetectService)).Assembly;
            foreach (var file in files)
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);
                using (var fs = File.Create(path))
                using (var stream = assembly.GetManifestResourceStream(resourcePrefix + file))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fs);                    
                }
            }
        }

        #endregion

        #region IDetectService Members

        public DetectResult Detect(byte[] file)
        {
            using var frame = Cv2.ImDecode(file, CvLoadImage.Grayscale);
            if (frame.IsEmpty)
                throw new NotSupportedException("This file is not supported!!");

            if (Ncnn.IsSupportVulkan)
                Ncnn.CreateGpuInstance();

            using var inMat = NcnnDotNet.Mat.FromPixels(frame.Data, NcnnDotNet.PixelType.Bgr2Rgb, frame.Cols, frame.Rows);

            var faceInfos = ultraFace.Detect(inMat).ToArray();

            if (Ncnn.IsSupportVulkan)
                Ncnn.DestroyGpuInstance();

            return new DetectResult(frame.Cols, m.Rows, faceInfos);
        }


        #region Helpers

        private static int DetectYoloV3(NcnnDotNet.OpenCV.Mat bgr, List<Object> objects)
        {
            var param = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mobilenetv2_yolov3.param");
            var model = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mobilenetv2_yolov3.bin");

            if (!File.Exists(param))
                throw new FileNotFoundException("param file is missing");
            if (!File.Exists(model))
                throw new FileNotFoundException("model file is missing");

            using (var yolov3 = new Net())
            {
                if (Ncnn.IsSupportVulkan)
                    yolov3.Opt.UseVulkanCompute = true;

                // original pretrained model from https://github.com/eric612/MobileNet-YOLO
                // param : https://drive.google.com/open?id=1V9oKHP6G6XvXZqhZbzNKL6FI_clRWdC-
                // bin : https://drive.google.com/open?id=1DBcuFCr-856z3FRQznWL_S5h-Aj3RawA
                // the ncnn model https://github.com/nihui/ncnn-assets/tree/master/models
                yolov3.LoadParam(param);
                yolov3.LoadModel(model);

                const int targetSize = 352;

                var imgW = bgr.Cols;
                var imgH = bgr.Rows;

                using var @in = Mat.FromPixelsResize(bgr.Data, PixelType.Bgr, bgr.Cols, bgr.Rows, targetSize, targetSize);
                var meanVals = new[] { 127.5f, 127.5f, 127.5f };
                var normVals = new[] { 0.007843f, 0.007843f, 0.007843f };
                @in.SubstractMeanNormalize(meanVals, normVals);

                using var ex = yolov3.CreateExtractor();
                ex.SetNumThreads(4);

                ex.Input("data", @in);

                using var @out = new Mat();
                ex.Extract("detection_out", @out);

                //     printf("%d %d %d\n", out.w, out.h, out.c);
                objects.Clear();
                for (var i = 0; i < @out.H; i++)
                {
                    var values = @out.Row(i);

                    var @object = new Object();
                    @object.Label = (int)values[0];
                    @object.Prob = values[1];
                    @object.Rect.X = values[2] * imgW;
                    @object.Rect.Y = values[3] * imgH;
                    @object.Rect.Width = values[4] * imgW - @object.Rect.X;
                    @object.Rect.Height = values[5] * imgH - @object.Rect.Y;

                    objects.Add(@object);
                }
            }

            return 0;
        }

        #endregion

        #endregion

    }

}
