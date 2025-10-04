# Ollama Guide

Ollama is an open-source project that allows you to run large language models (LLMs) locally on your machine. It provides an easy way to download, run, and manage various AI models.

> **Note**: This project's code was generated with the assistance of GitHub Copilot, showcasing the power of AI-assisted development.

This repository contains:
1. Documentation for using Ollama
2. A .NET Core minimal API project for interacting with Ollama
3. Model library information

## Table of Contents
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Available Models](#available-models)
- [System Requirements](#system-requirements)
- [Model Management](#model-management)
- [API Usage](#api-usage)
- [.NET Core API Project](#net-core-api-project)

## Installation

### Windows Installation
1. Download the [OllamaSetup.exe](https://ollama.com/download/OllamaSetup.exe)
2. Run the installer
3. Follow the installation wizard
4. After installation, Ollama will start automatically

### System Requirements

- Minimum 8GB RAM for 7B models
- Minimum 16GB RAM for 13B models
- Minimum 32GB RAM for 33B models
- NVIDIA GPU (optional but recommended)

## Quick Start

1. Open a terminal/command prompt
2. Pull a model:
```sh
ollama pull phi3
```

3. Run the model:
```sh
ollama run phi3
```

## Model Management

Basic commands:
```sh
# List available models
ollama list

# Remove a model
ollama rm modelname

# Show model info
ollama show modelname

# Copy a model
ollama cp source destination
```

## Available Models

Some popular models include:

| Model | Size | Description |
|-------|------|-------------|
| Gemma 3 | 4B | General purpose model |
| Mistral | 7B | General chat model |
| CodeLlama | 7B | Code generation |
| LLaVA | 7B | Vision model |
| Phi 4 | 14B | Advanced reasoning |

[View full model library](https://ollama.com/library)

## API Usage

Ollama provides a REST API for integration:

```sh
# Generate a response
curl http://localhost:11434/api/generate -d '{
  "model": "phi3",
  "prompt": "Why is the sky blue?"
}'

# Chat conversation
curl http://localhost:11434/api/chat -d '{
  "model": "phi3",
  "messages": [
    { "role": "user", "content": "Why is the sky blue?" }
  ]
}'
```

## .NET Core API Project

The repository includes a .NET Core minimal API project that provides a RESTful interface to Ollama. The API is built using minimal API architecture and includes:

### Features
- Swagger/OpenAPI documentation
- Strongly typed responses
- Proper error handling
- Clean architecture with service layer

### API Endpoints

All endpoints are available under `/api/ollama` and return typed responses:

#### List Models
```http
GET /api/ollama/models
```
Returns a list of available models.

#### Generate Text
```http
POST /api/ollama/generate
Content-Type: application/json

{
    "model": "modelname",
    "prompt": "Your prompt here"
}
```
Generates text using the specified model.

#### Chat
```http
POST /api/ollama/chat
Content-Type: application/json

{
    "model": "modelname",
    "messages": [
        {
            "role": "user",
            "content": "Your message here"
        }
    ]
}
```
Start or continue a chat conversation.

### Response Format
All endpoints return properly typed responses:
- 200: Successful operation with typed response
- 404: Model not found
- 500: Internal server error

### API Documentation
The API includes Swagger documentation available at `/swagger` when running in development mode.

### Features
- Minimal API design for better performance
- Swagger/OpenAPI documentation
- Error handling with proper HTTP status codes
- Async/await pattern
- Dependency injection

### API Endpoints

1. Generate Text
```http
POST /api/ollama/generate
Content-Type: application/json

{
  "model": "phi3",
  "prompt": "What is quantum computing?"
}
```

2. Chat
```http
POST /api/ollama/chat
Content-Type: application/json

{
  "model": "phi3",
  "messages": [
    { "role": "user", "content": "What is quantum computing?" }
  ]
}
```

3. List Models
```http
GET /api/ollama/models
```

### Running the Project

1. Make sure Ollama is installed and running
2. Navigate to the project directory:
```sh
cd OllamaApi
```

3. Run the project:
```sh
dotnet run
```

4. Open Swagger UI:
```
https://localhost:7000/swagger
```

### Error Handling
- 404: Model not found
- 500: Internal server error with detailed message
- Proper exception handling for HTTP and deserialization errors

## Additional Resources
- [Official Documentation](https://github.com/ollama/ollama/tree/main/docs)
- [GitHub Repository](https://github.com/ollama/ollama)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
