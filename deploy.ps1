$version = $env:APPVEYOR_BUILD_VERSION

git clone --quiet --branch=gh-pages https://github.com/coldstorm/DesktopCS.git "C:\projects\desktopcs\Deployment"

& 'C:\Program Files (x86)\MSBuild\12.0\bin\msbuild.exe' /target:publish /p:Configuration=Release /p:Platform=AnyCPU /p:ApplicationVersion=$version "C:\projects\desktopcs\DesktopCS\DesktopCS.csproj"

robocopy "C:\projects\desktopcs\DesktopCS\bin\Release\app.publish" "C:\projects\desktopcs\Deployment\download" /S

cd "C:\projects\desktopcs\Deployment"

git add .

git commit -m ("Release " + $version)

git format-patch --stdout HEAD^ > ("deploy-" + $version + ".patch")

appveyor PushArtifact ("deploy-" + $version + ".patch")