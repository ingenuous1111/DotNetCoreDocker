#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DockeroDummy.csproj", "."]
RUN dotnet restore "./DockeroDummy.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DockeroDummy.csproj" -c Release -o /app/build

FROM build AS publish
RUN  dotnet publish "DockeroDummy.csproj" -c Release -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apt-get update && apt-get install -y zip curl
RUN mkdir /otel
RUN curl -L -o /otel/otel-dotnet-install.sh https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v0.7.0/otel-dotnet-auto-install.sh
RUN chmod +x /otel/otel-dotnet-install.sh

ENV OTEL_EXPORTER_OLTP_ENDPOINT=https://4760a6ce142b46fb9accf37b43fab918.apm.us-central1.gcp.cloud.es.io:443
ENV OTEL_EXPORTER_OLTP_HEADERS="Authorization=Bearer auBpHbjNHshXDDMd1O"
ENV OTEL_METRICS_EXPORTER=otlp
ENV OTEL_LOGS_EXPORTER=otlp
ENV OTEL_RESOURCE_ATTRIBUTES=service.name=dotNet, service.version=1.0, deployment.environment=production
ENV OTEL_DOTNET_AUTO_HOME=/otel

RUN /bin/bash /otel/otel-dotnet-install.sh

RUN chmod +x /otel/instrument.sh
EXPOSE 5000

#ENTRYPOINT ["dotnet", "DockeroDummy.dll"]
ENTRYPOINT ["/bin/bash", "-c", "source /otel/instrument.sh && dotnet DockeroDummy.dll"]
