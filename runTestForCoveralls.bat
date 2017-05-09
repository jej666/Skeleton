nuget install OpenCover -Version 4.6.519 -OutputDirectory tools
nuget install coveralls.io -Version 1.3.4 -OutputDirectory tools

.\tools\OpenCover.4.6.519\OpenCover.Console.exe -register:user -filter:"+[*]*" -target:"tools\NUnit.ConsoleRunner.3.6.1\tools\nunit-console.exe"
-targetargs:"/noshadow /domain:single  Skeleton.Tests.Infrastructure\bin\debug\Skeleton.Tests.Infrastructure.dll" -output:coverage.xml

.\tools\coveralls.io.1.3.4\tools\coveralls.net.exe --opencover coverage.xml
