set START_DIR=%cd%

dotnet restore .\Hangfire.StructureMap.sln
dotnet build .\src\Hangfire.StructureMap\Hangfire.StructureMap.csproj --configuration Release

dotnet test

cd %START_DIR%

dotnet pack .\src\Hangfire.StructureMap -c Release
