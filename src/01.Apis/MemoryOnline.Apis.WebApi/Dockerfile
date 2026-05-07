# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/01.Apis/MemoryOnline.Apis.WebApi/MemoryOnline.Apis.WebApi.csproj", "src/01.Apis/MemoryOnline.Apis.WebApi/"]
COPY ["src/03.Application/MemoryOnline.Application.Users/MemoryOnline.Application.Users.csproj", "src/03.Application/MemoryOnline.Application.Users/"]
COPY ["src/02.Domain/MemoryOnline.Domain.Domain/MemoryOnline.Domain.Domain.csproj", "src/02.Domain/MemoryOnline.Domain.Domain/"]
COPY ["src/02.Domain/MemoryOnline.Domain.Entities/MemoryOnline.Domain.Entities.csproj", "src/02.Domain/MemoryOnline.Domain.Entities/"]
COPY ["src/04.Infraestructure/MemoryOnline.Infraestructure.Repository/MemoryOnline.Infraestructure.IRepository.csproj", "src/04.Infraestructure/MemoryOnline.Infraestructure.Repository/"]
COPY ["src/03.Application/MemoryOnline.Application.Events/MemoryOnline.Application.Events.csproj", "src/03.Application/MemoryOnline.Application.Events/"]
COPY ["src/05.Common/MemoryOnline.Common.IOC/MemoryOnline.Common.IOC.csproj", "src/05.Common/MemoryOnline.Common.IOC/"]
COPY ["src/04.Infraestructure/MemoryOnline.Infraestructure.EF/MemoryOnline.Infraestructure.EF/MemoryOnline.Infraestructure.EF.csproj", "src/04.Infraestructure/MemoryOnline.Infraestructure.EF/MemoryOnline.Infraestructure.EF/"]
COPY ["src/04.Infraestructure/Hispalance.Infraestructure.DB_new/Hispalance.Infraestructure.DB.csproj", "src/04.Infraestructure/Hispalance.Infraestructure.DB_new/"]
COPY ["src/01.Apis/Hispalance.Presentation.Extensions/Hispalance.Presentation.Extensions.csproj", "src/01.Apis/Hispalance.Presentation.Extensions/"]
RUN dotnet restore "./src/01.Apis/MemoryOnline.Apis.WebApi/MemoryOnline.Apis.WebApi.csproj"
COPY . .
WORKDIR "/src/src/01.Apis/MemoryOnline.Apis.WebApi"
RUN dotnet build "./MemoryOnline.Apis.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MemoryOnline.Apis.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MemoryOnline.Apis.WebApi.dll"]