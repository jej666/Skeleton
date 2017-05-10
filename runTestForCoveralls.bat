nuget install OpenCover -Version 4.6.519 -OutputDirectory packages
nuget install coveralls.io -Version 1.3.4 -OutputDirectory packages
nuget install NUnit.ConsoleRunner -Version 3.6.1 -OutputDirectory packages

packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -filter:"+[*]*" -target:"packages\NUnit.ConsoleRunner.3.6.1\tools\nunit3-console.exe" -targetargs:"/domain:single Skeleton.Infrastructure.Tests.Infrastructure\bin\debug\Skeleton.Tests.Infrastructure.dll" -output:coverage.xml
packages\coveralls.io.1.3.4\tools\coveralls.net.exe --opencover coverage.xml
