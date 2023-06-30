FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /App
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet","CRUD_Console_Api_Client.dll"]

