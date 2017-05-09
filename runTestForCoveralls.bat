nuget install OpenCover -Version 4.6.519 -OutputDirectory tools
nuget install coveralls.net -Version 0.412.0 -OutputDirectory tools

.\tools\OpenCover.4.6.519\OpenCover.Console.exe 
-register:user -filter:"+[*]*" 
-target:"packages\NUnit.ConsoleRunner.3.6.1\tools\nunit-console.exe" 
-targetargs:"/noshadow /domain:single  Skeleton.Tests.Infrastructure\bin\debug\Skeleton.Tests.Infrastrucuture.dll" 
-output:coverage.xml
