FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV DOTNET_USE_POLLING_FILE_WATHCER 1
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY . .
RUN dotnet restore "DockerTest.csproj"
ENTRYPOINT [ "dotnet", "watch", "run" ]
