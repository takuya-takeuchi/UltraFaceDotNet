# Demo

This example demonstrates detecting faces from images file.
This program is ported by C# from https://github.com/Linzaer/Ultra-Light-Fast-Generic-Face-Detector-1MB/blob/master/ncnn/src/main.cpp.

## How to use?

## 1. Preparation

This example requires test images, model adn parameter files. 
Download test data from the following urls.

- https://github.com/Linzaer/Ultra-Light-Fast-Generic-Face-Detector-1MB/tree/master/ncnn/data
  - version-RFB/RFB-320.bin
  - version-RFB/RFB-320.param
  - version-slim/slim_320.bin
  - version-slim/slim_320.param
  - test.jpg

And copy to these files to &lt;Demo_dir&gt;.

## 2. Build

1. Open command prompt and change to &lt;Demo_dir&gt;
1. Type the following command
````
dotnet build -c Release
````

## 3. Run

1. Open command prompt and change to &lt;Demo_dir&gt;
1. Type the following sample command
````
$ dotnet run -c Release -- RFB-320.bin RFB-320.param test.jpg

Processing .\test.jpg
````

This example can accept multiple image file.
And you can also specify slim_320.bin and slim_320.param as argument.

![Demo](images/image.jpg "Demo")