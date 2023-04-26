# Start

### Shared services
docker run -d -p 5672:5672 -p 15672:15672 --name dtc-rabbitmq rabbitmq:3-management-alpine

### Insurance Service

```
cd Insurance
dapr run --app-id insuranceservice --app-port 6000 --dapr-http-port 3600 --config ../dapr/config/config.yaml --resources-path ../dapr/components dotnet run
```

### Test Insurance Service

```
curl -v -X POST --header 'Content-Type: application/json' http://localhost:6000/verify --data '{"VIN": "WBAKS123123123"}'
```