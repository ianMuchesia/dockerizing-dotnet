# Todo API with JWT Authentication

A simple Todo API built with .NET 8.0 and SQL Server, featuring JWT authentication.

## Features

- RESTful API for Todo items management
- JWT Authentication
- SQL Server database
- Dockerized application

## Prerequisites

- Docker
- Docker Compose

## Running the Application

### Option 1: Using docker-compose with local build

```bash
# Clone the repository
git clone https://github.com/yourusername/todoapi.git
cd todoapi

# Start the application
docker-compose up -d
```

### Option 2: Pull the image from Docker Hub

```bash
# Create a docker-compose.yml file or use the provided docker-compose.pull.yml
# Start the application
docker-compose -f docker-compose.pull.yml up -d
```

Or use the provided script:

```bash
chmod +x docker-run.sh
./docker-run.sh
```

## API Endpoints

Once the application is running, you can access:

- API: http://localhost:5000
- Swagger UI: http://localhost:5000/swagger
- Health check: http://localhost:5000/health

### Authentication

- POST `/api/Auth/register` - Register a new user
- POST `/api/Auth/login` - Login and get JWT token

### Todo Items

All Todo endpoints require authentication with the JWT token in the Authorization header.

- GET `/api/Todo` - Get all todos for the authenticated user
- GET `/api/Todo/{id}` - Get a specific todo by ID
- POST `/api/Todo` - Create a new todo
- PUT `/api/Todo/{id}` - Update a todo
- DELETE `/api/Todo/{id}` - Delete a todo

## Building and Pushing to Docker Hub

To build and push the Docker image to your Docker Hub account:

```bash
# Replace yourusername with your Docker Hub username in docker-build-push.sh
chmod +x docker-build-push.sh
./docker-build-push.sh
```

## Environment Variables

You can customize the application using these environment variables:

- `ASPNETCORE_ENVIRONMENT` - Set to 'Development' or 'Production'
- `ConnectionStrings__DefaultConnection` - SQL Server connection string

## License

MIT