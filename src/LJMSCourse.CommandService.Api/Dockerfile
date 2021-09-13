#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/LJMSCourse.CommandService.Api/LJMSCourse.CommandService.Api.csproj", "src/LJMSCourse.CommandService.Api/"]
RUN dotnet restore "src/LJMSCourse.CommandService.Api/LJMSCourse.CommandService.Api.csproj"
COPY . .
WORKDIR "/src/src/LJMSCourse.CommandService.Api"
RUN dotnet build "LJMSCourse.CommandService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LJMSCourse.CommandService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LJMSCourse.CommandService.Api.dll"]