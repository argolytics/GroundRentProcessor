#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/mssql/server AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/mssql/server AS build
WORKDIR /src
COPY ["BlazorServerUI/BlazorServerUI.csproj", "BlazorServerUI/"]
COPY ["DataLibrary/DataLibrary.csproj", "DataLibrary/"]
RUN dotnet restore "BlazorServerUI/BlazorServerUI.csproj"
COPY . .
WORKDIR "/src/BlazorServerUI"
RUN dotnet build "BlazorServerUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorServerUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorServerUI.dll"]