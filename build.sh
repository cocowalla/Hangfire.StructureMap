#!/bin/bash
set -ev

dotnet restore ./Hangfire.StructureMap.sln
dotnet build ./src/Hangfire.StructureMap/Hangfire.StructureMap.csproj --configuration Release

dotnet test

cd ${TRAVIS_BUILD_DIR}
