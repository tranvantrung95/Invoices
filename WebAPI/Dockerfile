# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Stage 2: Run the app
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /out ./

# Expose port and set entrypoint
EXPOSE 80
ENTRYPOINT ["dotnet", "WebAPI.dll"]
