#!/bin/bash

# Replace with your Docker Hub username
DOCKER_USERNAME="ianmuchesia"
IMAGE_NAME="todoapi"
TAG="latest"

# Build the Docker image
echo "Building Docker image..."
docker build -t $DOCKER_USERNAME/$IMAGE_NAME:$TAG .

# Push the image to Docker Hub
echo "Pushing image to Docker Hub..."
docker login
docker push $DOCKER_USERNAME/$IMAGE_NAME:$TAG

echo "Done! Your image is now available at $DOCKER_USERNAME/$IMAGE_NAME:$TAG"