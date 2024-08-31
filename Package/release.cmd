del "*.nupkg"
"..\..\oqtane.framework-dev\oqtane.package\nuget.exe" pack Ben.Module.LiveChat.nuspec 
XCOPY "*.nupkg" "..\..\oqtane.framework-dev\Oqtane.Server\Packages\" /Y

