# Ollama API Guide

## REST API Endpoints

### Generate Endpoint
```sh
POST http://localhost:11434/api/generate
```

Example request:
```json
{
  "model": "gemma3",
  "prompt": "What is quantum computing?"
}
```

### Chat Endpoint
```sh
POST http://localhost:11434/api/chat
```

Example request:
```json
{
  "model": "gemma3",
  "messages": [
    { "role": "user", "content": "What is quantum computing?" }
  ]
}
```

## Model Management API

### List Models
```sh
GET http://localhost:11434/api/tags
```

### Pull Model
```sh
POST http://localhost:11434/api/pull
```

Example request:
```json
{
  "name": "gemma3"
}
```

### Delete Model
```sh
DELETE http://localhost:11434/api/delete
```

Example request:
```json
{
  "name": "gemma3"
}
```

## Integration Examples

### Python Example
```python
import requests

def generate_response(prompt):
    response = requests.post('http://localhost:11434/api/generate', 
        json={
            "model": "gemma3",
            "prompt": prompt
        })
    return response.json()
```

### JavaScript Example
```javascript
async function generateResponse(prompt) {
    const response = await fetch('http://localhost:11434/api/generate', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            model: 'gemma3',
            prompt: prompt
        })
    });
    return await response.json();
}
