# Stage 1: restore dependencies
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
WORKDIR /src
COPY BlazorWebTemplate.Shared/BlazorWebTemplate.Shared.csproj BlazorWebTemplate.Shared/
COPY BlazorWebTemplate.Client/BlazorWebTemplate.Client.csproj BlazorWebTemplate.Client/
COPY BlazorWebTemplate.Web/BlazorWebTemplate.Web.csproj BlazorWebTemplate.Web/
RUN dotnet restore BlazorWebTemplate.Web/BlazorWebTemplate.Web.csproj

# Stage 2: build and publish
FROM restore AS publish
COPY BlazorWebTemplate.Shared/ BlazorWebTemplate.Shared/
COPY BlazorWebTemplate.Client/ BlazorWebTemplate.Client/
COPY BlazorWebTemplate.Web/ BlazorWebTemplate.Web/
RUN dotnet publish BlazorWebTemplate.Web/BlazorWebTemplate.Web.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Stage 3: runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BlazorWebTemplate.Web.dll"]
