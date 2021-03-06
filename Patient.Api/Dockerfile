#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Patient.Api/Patient.Api.csproj", "Patient.Api/"]
COPY ["PatientNavigation.Common/PatientNavigation.Common.csproj", "PatientNavigation.Common/"]
RUN dotnet restore "Patient.Api/Patient.Api.csproj"
COPY . .
WORKDIR "/src/Patient.Api"
RUN dotnet build "Patient.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Patient.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Patient.Api.dll"]
