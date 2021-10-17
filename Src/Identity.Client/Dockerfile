#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Src/Identity.Client/Identity.Client.csproj", "Src/Identity.Client/"]
RUN dotnet restore "Src/Identity.Client/Identity.Client.csproj"
COPY . .
WORKDIR "/src/Src/Identity.Client"
RUN dotnet build "Identity.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Client.dll"]