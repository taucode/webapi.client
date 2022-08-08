dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\test\TauCode.WebApi.Client.Tests\TauCode.WebApi.Client.Tests.csproj
dotnet test -c Release .\test\TauCode.WebApi.Client.Tests\TauCode.WebApi.Client.Tests.csproj

nuget pack nuget\TauCode.WebApi.Client.nuspec
