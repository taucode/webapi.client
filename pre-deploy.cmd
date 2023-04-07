dotnet restore

dotnet build TauCode.WebApi.Client.sln -c Debug
dotnet build TauCode.WebApi.Client.sln -c Release

dotnet test TauCode.WebApi.Client.sln -c Debug
dotnet test TauCode.WebApi.Client.sln -c Release

nuget pack nuget\TauCode.WebApi.Client.nuspec