#!/bin/bash
set -ev

dotnet restore ./Hangfire.StructureMap.sln
dotnet build ./src/Hangfire.StructureMap/Hangfire.StructureMap.csproj --configuration Release

# dotnet-xunit is a CLI tool that can only be executed from in the test folder
cd ./test/Hangfire.StructureMap.Test

dotnet xunit -framework netcoreapp1.1 -fxversion 1.1.7
dotnet xunit -framework netcoreapp2.0 -fxversion 2.1.4

cd ${TRAVIS_BUILD_DIR}
