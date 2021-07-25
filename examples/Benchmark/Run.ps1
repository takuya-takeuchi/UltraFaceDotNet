$Tables =
@{
    "NcnnDotNet"     = "UltraFaceDotNet";
    "NcnnDotNet.GPU" = "UltraFaceDotNet.GPU";
}

# remove packages
foreach ($key in $Tables.keys)
{
    $ncnn = $key
    $uf = $Tables[$key]
    dotnet remove package $ncnn > $null
    dotnet remove package $uf > $null
}

# do test
foreach ($key in $Tables.keys)
{
    $ncnn = $key
    $uf = $Tables[$key]
    dotnet add package $ncnn > $null
    dotnet add package $uf > $null

    Write-Host "Benchmark for ${uf}" -ForegroundColor Blue
    dotnet run -c Release -- RFB-320.bin RFB-320.param test.jpg 1000

    dotnet remove package $ncnn > $null
    dotnet remove package $uf > $null
}