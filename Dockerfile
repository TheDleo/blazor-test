FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["BlazorServerCRUD.Web/BlazorServerCRUD.Web.csproj", "BlazorServerCRUD.Web/"]
COPY ["BlazorServerCRUD.Tests/BlazorServerCRUD.Tests.csproj", "BlazorServerCRUD.Tests/"]
RUN dotnet restore "BlazorServerCRUD.Web/BlazorServerCRUD.Web.csproj"

# Copy the rest of the source code
COPY . .

# Build and test
RUN dotnet build "BlazorServerCRUD.Web/BlazorServerCRUD.Web.csproj" -c Release -o /app/build
RUN dotnet test "BlazorServerCRUD.Tests/BlazorServerCRUD.Tests.csproj" -c Release

# Publish
RUN dotnet publish "BlazorServerCRUD.Web/BlazorServerCRUD.Web.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "BlazorServerCRUD.Web.dll"]
