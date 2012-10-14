@echo off

:: Set repository info
set key={your-api-key}
set url={nuget-gallery-url}

:: Make sure the nuget executable is writable
attrib -R NuGet.exe

:: Make sure the nupkg files are writeable and create backup
IF EXIST *.nupkg (
	echo.
	echo Creating backup...
	forfiles /m *.nupkg /c "cmd /c attrib -R @File"
	forfiles /m *.nupkg /c "cmd /c move /Y @File @File.bak"
)

echo.
echo Updating NuGet...
cmd /c nuget.exe update -Self

echo.
echo Creating package...
nuget.exe pack Package.nuspec -Verbose -Version "1.0.0.0"

:: Check if package should be published
IF /I "NotPublish"=="Publish" goto :publish
goto :eof

:publish
IF EXIST *.nupkg (
	echo.
	echo Publishing package...
	echo API Key: %key%
	echo NuGet Url: %url%
	forfiles /m *.nupkg /c "cmd /c nuget.exe push @File "3BB1F400-DE5D-4F09-86D3-BEAE8EAB3E0E" -Source "../../../NuGet.Repository/"
	goto :eof
)

:eof