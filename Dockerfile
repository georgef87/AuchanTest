# Use the 8.0 official ASP.NET Core image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the 8.0 SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AuchanTest.csproj", "./"]
RUN dotnet restore "./AuchanTest.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AuchanTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuchanTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuchanTest.dll"]