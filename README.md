# ![Alt text](nuget/face48.png "UltraFaceDotNet") UltraFaceDotNet [![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)]() [![codecov](https://codecov.io/gh/takuya-takeuchi/UltraFaceDotNet/branch/master/graph/badge.svg)](https://codecov.io/gh/takuya-takeuchi/UltraFaceDotNet)

C# version of Ultra-Light-Fast-Generic-Face-Detector-1MB
This repository is porting https://github.com/Linzaer/Ultra-Light-Fast-Generic-Face-Detector-1MB by C#.

This package supports cross platform, Windows, MacOS, Linux, iOS and Android!!

|Package|OS|x86|x64|ARM|ARM64|Nuget|
|---|---|---|---|---|---|---|
|UltraFaceDotNet (CPU)|Windows|-|✓|-|-|[![NuGet version](https://img.shields.io/nuget/v/UltraFaceDotNet.svg)](https://www.nuget.org/packages/UltraFaceDotNet)|
||Linux|-|✓|-|✓|[![NuGet version](https://img.shields.io/nuget/v/UltraFaceDotNet.svg)](https://www.nuget.org/packages/UltraFaceDotNet)|
||OSX|-|✓|-|-|[![NuGet version](https://img.shields.io/nuget/v/UltraFaceDotNet.svg)](https://www.nuget.org/packages/UltraFaceDotNet)|
|UltraFaceDotNet (GPU)|Windows|-|✓|-|-|[![NuGet version](https://img.shields.io/nuget/v/UltraFaceDotNet.GPU.svg)](https://www.nuget.org/packages/UltraFaceDotNet.GPU)|
||Linux|-|✓|-|-|[![NuGet version](https://img.shields.io/nuget/v/UltraFaceDotNet.GPU.svg)](https://www.nuget.org/packages/UltraFaceDotNet.GPU)|
||OSX|-|✓|-|-|[![NuGet version](https://img.shields.io/nuget/v/UltraFaceDotNet.GPU.svg)](https://www.nuget.org/packages/UltraFaceDotNet.GPU)|
|UltraFaceDotNet (Xamarin)|UWP|✓|✓|✓|✓|[![NuGet version](https://img.shields.io/nuget/v/UltraFaceDotNet.Xamarin.svg)](https://www.nuget.org/packages/UltraFaceDotNet.Xamarin)|
||Android|✓|✓|✓|✓|[![NuGet version](https://img.shields.io/nuget/v/UltraFaceDotNet.Xamarin.svg)](https://www.nuget.org/packages/UltraFaceDotNet.Xamarin)|
||iOS|-|✓|-|✓|[![NuGet version](https://img.shields.io/nuget/v/UltraFaceDotNet.Xamarin.svg)](https://www.nuget.org/packages/UltraFaceDotNet.Xamarin)|

## Demo

#### Console (using OpenCV UI)

<img src="examples/Demo/images/image.jpg" width="480"/>

#### YoloV3 on Xamarin.Android, iOS and UWP

<img src="examples/Xamarin/Demo/images/android.png" width="300" /> <img src="examples/Xamarin/Demo/images/ios.png" width="300" />
<img src="examples/Xamarin/Demo/images/uwp.png" width="600" />

## Dependencies Libraries and Products

#### [Ultra-Light-Fast-Generic-Face-Detector-1MB](https://github.com/Linzaer/Ultra-Light-Fast-Generic-Face-Detector-1MB/)

> **License:** The MIT License
>
> **Author:** Linzaer
> 
> **Principal Use:** 1MB lightweight face detection model. Main goal of UltraFaceDotNet is what ports Ultra-Light-Fast-Generic-Face-Detector-1MB by C#.

#### [ncnn](https://github.com/Tencent/ncnn/)

> **License:** The BSD 3-Clause License
>
> **Author:** THL A29. Limited, a Tencent company
> 
> **Principal Use:** A high-performance neural network inference framework optimized for the mobile platform in C++. Main goal of UltraFaceDotNet is what wraps ncnn by C#.

#### [OpenCV](https://opencv.org/)

> **License:** The BSD 3-Clause License
>
> **Author:** Intel Corporation, Willow Garage, Itseez
> 
> **Principal Use:** Uses to read and show image data.

#### [NcnnDotNet](https://github.com/takuya-takeuchi/NcnnDotNet/)

> **License:** The MIT License
>
> **Author:** Takuya Takeuchi
> 
> **Principal Use:** Use ncnn interface via .NET. This library is developed by this owner.

#### [ShellProgressBar](https://github.com/Mpdreamz/shellprogressbar/)

> **License:** The MIT License
>
> **Author:** Martijn Laarman
> 
> **Principal Use:** Visualize progress in Benchmark program.

