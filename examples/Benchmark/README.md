# Benchmark

This example demonstrates performance of detecting faces from images file.

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

And copy to these files to &lt;Benchmark_dir&gt;.

## 2. Build

1. Open command prompt and change to &lt;Benchmark_dir&gt;
1. Type the following command
````
dotnet build -c Release
````

## 3. Run

1. Open command prompt and change to &lt;Benchmark_dir&gt;
1. Type the following sample command
````
$ dotnet run -c Release -- RFB-320.bin RFB-320.param test.jpg 1000

Processing .\test.jpg
100.00% Step 100 of 100                                                                                         00:00:06────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
Total Loop 100
        Convert Image to Mat: Total 17139 ms, Avg 171.39 ms
                 Detect Face: Total 282969 ms, Avg 2829.69 ms
            Total Throughput: Total 300108 ms, Avg 3001.08 ms
````