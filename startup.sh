#!/bin/bash

# Set Node.js version for Azure App Service
export WEBSITE_NODE_DEFAULT_VERSION=18.19.0

# Install Node.js dependencies if needed
if [ ! -d "ClientApp/node_modules" ]; then
    echo "Installing Node.js dependencies..."
    cd ClientApp
    npm install --no-audit --no-fund
    cd ..
fi

# Start the .NET application
dotnet Portfolio.dll 