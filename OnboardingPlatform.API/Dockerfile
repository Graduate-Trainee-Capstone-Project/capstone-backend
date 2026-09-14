# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OnboardingPlatform.API/OnboardingPlatform.API.csproj", "OnboardingPlatform.API/"]
COPY . .
RUN dotnet restore "OnboardingPlatform.API/OnboardingPlatform.API.csproj"
RUN dotnet publish "OnboardingPlatform.API/OnboardingPlatform.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OnboardingPlatform.API.dll"]