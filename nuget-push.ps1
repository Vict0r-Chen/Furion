Param(
    [string] $apikey
)

cd .\framework\nupkgs;
$framework_nupkgs = Get-ChildItem -Filter *.nupkg;

for ($i = 0; $i -le $framework_nupkgs.Length - 1; $i++){
    $item = $framework_nupkgs[$i];

    $nupkg = $item.FullName;
    $snupkg = $nupkg.Replace(".nupkg", ".snupkg");

    Write-Output "-----------------";
    $nupkg;

    dotnet nuget push $nupkg --skip-duplicate --api-key $apikey --source https://api.nuget.org/v3/index.json;
    dotnet nuget push $snupkg --skip-duplicate --api-key $apikey --source https://api.nuget.org/v3/index.json;

    Write-Output "-----------------";
}

cd ../../;

Write-Warning "Successfully.";