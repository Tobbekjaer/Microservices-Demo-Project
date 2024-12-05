## Microservices Demo Project

Udvikl en microservices-løsning, hvor:
  - **Platform Service** registrerer nye platforme (fx .NET, Docker).
  - **Commands Service** håndterer oprettelse af commands for platforme og viser dem via REST API. (fx. "dotnet run", "docker build -t <docker user id>/<image name>:<version> .")

I projektet har jeg beskæftiget mig med følgende komponenter:
   - To .NET microservices (Platform Service og Commands Service).
   - Dedikerede databaser til hver service.
   - En API Gateway til at route requests.
   - Synkron kommunikation (HTTP & gRPC).
   - Asynkron kommunikation via Event Bus (RabbitMQ).
