$version = $env:APPVEYOR_BUILD_VERSION
$gitExe = Get-Command git -syntax
$gitFormatPatch = "format-patch --stdout HEAD^"
$patchFile = "deploy-" + $version + ".patch"

git clone --quiet --branch=gh-pages https://github.com/coldstorm/DesktopCS.git "C:\projects\desktopcs\Deployment"

& 'C:\Program Files (x86)\MSBuild\12.0\bin\msbuild.exe' /target:publish /p:Configuration=Release /p:Platform=AnyCPU /p:ApplicationVersion=$version "C:\projects\desktopcs\DesktopCS\DesktopCS.csproj"

robocopy "C:\projects\desktopcs\DesktopCS\bin\Release\app.publish" "C:\projects\desktopcs\Deployment\download" /S

cd "C:\projects\desktopcs\Deployment"

git add .

git commit -m ("Release " + $version)

Start-Process -FilePath $gitExe -ArgumentList $gitFormatPatch -Wait -NoNewWindow -RedirectStandardOutput $patchFile

appveyor PushArtifact $patchFile