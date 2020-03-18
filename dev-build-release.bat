dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\tests\TauCode.WebApi.Client.Tests\TauCode.WebApi.Client.Tests.csproj
dotnet test -c Release .\tests\TauCode.WebApi.Client.Tests\TauCode.WebApi.Client.Tests.csproj

nuget pack nuget\TauCode.WebApi.Client.nuspec
