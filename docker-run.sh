#!/bin/bash

# Start the application using docker-compose
echo "Pulling and starting the Todo API application..."
docker-compose -f docker-compose.pull.yml up -d

echo "Application is running!"
echo "API is available at http://localhost:5000"
echo "Swagger is available at http://localhost:5000/swagger"