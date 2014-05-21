$deploymentFolder = "C:\projects\desktopcs\Deployment"
$projectFile = "C:\projects\desktopcs\DesktopCS\DesktopCS.csproj"
$releaseFolder = "C:\projects\desktopcs\DesktopCS\bin\Release\app.publish"
$version = $env:APPVEYOR_BUILD_VERSION
$gitExe = Get-Command git -syntax
$gitClone = "clone --branch=gh-pages https://github.com/coldstorm/DesktopCS.git " + $deploymentFolder
$gitFormatPatch = "format-patch --stdout HEAD^"
$patchFile = "deploy-" + $version + ".patch"

Start-Process -FilePath $gitExe -ArgumentList $gitClone -Wait -NoNewWindow

& 'C:\Program Files (x86)\MSBuild\12.0\bin\msbuild.exe' /target:publish /p:Configuration=Release /p:Platform=AnyCPU /p:ApplicationVersion=$version $projectFile

ls $deploymentFolder -Recurse -Force

cd $deploymentFolder

mkdir ".\download"
cd ".\download"
mkdir ".\Application Files" 
cd $deploymentFolder

Move-Item -Path ($releaseFolder + "\Application Files\") -Destination ($deploymentFolder + "\download\Application Files") -Force -Verbose
Move-Item -Path ($releaseFolder + "\DesktopCS.application") -Destination ($deploymentFolder + "\download") -Force -Verbose

ls $deploymentFolder -Recurse -Force

git add .

git commit -m ("Release " + $version)

Start-Process -FilePath $gitExe -ArgumentList $gitFormatPatch -Wait -NoNewWindow -RedirectStandardOutput $patchFile

appveyor PushArtifact $patchFile