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
$ pwsh .\Run.ps1

Benchmark for UltraFaceDotNet
Processing test.jpg
100.00% Step 1000 of 1000                                                                                                                          00:00:11───────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────Total Loop 1000
        Convert Image to Mat: Total 2069 ms, Avg 2.069 ms
                 Detect Face: Total 7875 ms, Avg 7.875 ms
            Total Throughput: Total 9944 ms, Avg 9.944 ms
Benchmark for UltraFaceDotNet.GPU
Initializes GPU
[0 GeForce GTX 1080]  queueC=2[8]  queueG=0[16]  queueT=1[2]
[0 GeForce GTX 1080]  bugsbn1=0  bugbilz=0  bugcopc=0  bugihfa=0
[0 GeForce GTX 1080]  fp16-p/s/a=1/1/0  int8-p/s/a=1/1/1
[0 GeForce GTX 1080]  subgroup=32  basic=1  vote=1  ballot=1  shuffle=1
[1 Intel(R) UHD Graphics 630]  queueC=0[1]  queueG=0[1]  queueT=0[1]
[1 Intel(R) UHD Graphics 630]  bugsbn1=0  bugbilz=0  bugcopc=0  bugihfa=0
[1 Intel(R) UHD Graphics 630]  fp16-p/s/a=1/1/1  int8-p/s/a=1/1/1
[1 Intel(R) UHD Graphics 630]  subgroup=32  basic=1  vote=1  ballot=1  shuffle=1
        GPU is 0
Processing test.jpg
100.00% Step 1000 of 1000                                                                                                                          00:00:11───────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────Total Loop 1000
        Convert Image to Mat: Total 2082 ms, Avg 2.082 ms
                 Detect Face: Total 8197 ms, Avg 8.197 ms
            Total Throughput: Total 10279 ms, Avg 10.279 ms
Uninitializes GPU
````