# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Stage 2: Run the app and install wkhtmltopdf
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Install necessary dependencies and wkhtmltopdf
RUN apt-get update && \
    apt-get install --no-install-recommends -y \
    apt-utils \
    wget \
    xfonts-base \
    xfonts-75dpi \
    libfontconfig1 \
    libxrender1 \
    libxext6 \
    libx11-6 \
    libxau6 \
    libxdmcp6 \
    libxkbcommon0 \
    libxkbcommon-x11-0 && \
    rm -rf /var/lib/apt/lists/*

# Download and install wkhtmltopdf
ARG WKHTML2PDF_VERSION='0.12.6-1'
RUN export VERSION_CODENAME=$(lsb_release -c | awk '{print $2}') && \
    wget https://github.com/wkhtmltopdf/packaging/releases/download/${WKHTML2PDF_VERSION}/wkhtmltox_${WKHTML2PDF_VERSION}.${VERSION_CODENAME}_amd64.deb -O wkhtmltox.deb && \
    dpkg -i wkhtmltox.deb || apt-get install -f -y && \
    rm wkhtmltox.deb

COPY --from=build /out ./

# Expose port and set entrypoint
EXPOSE 80
ENTRYPOINT ["dotnet", "WebAPI.dll"]
