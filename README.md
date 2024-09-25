# Call Center Management

This project is part of the `Emite.CCM` solution and represents the API and Web for managing call center.

### Running in Docker via Docker Compose

Before running this application in Docker, make sure you have the following installed:

- **Docker**: [Install Docker](https://www.docker.com/get-started) if you don't have it already.

### Step 1: Run docker compose
```bash
docker-compose up --build
```

### Step 2: Map localhost to host.docker.internal from etc/host file
from windows, the path is : C:\Windows\System32\drivers\etc
for linux /etc/hosts
```bash
127.0.0.1 host.docker.internal
```

### Step 3: Access the Web and API
API Base URL: https://host.docker.internal:48023
Swagger UI: https://host.docker.internal/index.html

Web UI: https://host.docker.internal:48021

Username : system@admin
Password: Admin123!@#


# Overview
This project is based on template generated from my personal tool. After uploading the table structure, I performed the customization based on the instruction from the exam.
The ff. are the features of this project.
- .NET 8, C# 10
- Serilog w/ HTTP Context Enrichers and Seq and text file sink
- CQRS implementation using Mediatr library
- Fluent validation
- Pagination using X.PagedList
- C# functional programming extensions using language-ext library
- OpenID Connect server and token validation using OpenIddict library for JWToken
- Authorization via role claims
- Switch to use in-memory DB for development
- Audit trail
- Batch Uploader on Web UI
- Dashboard / Report Builder


# Requirements Checklist

Requirements:
1. Create a Web API using ASP.NET Core (.NET 6 or later) -  **Done** .Net 8
2. Implement CRUD operations for all the data models -  **Done**
3. Use Entity Framework Core for database operations -  **Done**
4. Implement JWT authentication -  **Done**
5. Write unit tests for your services -  **Done**
6. Implement basic error handling and logging -  **Done**
7. Use dependency injection -  **Done**
8. Use https://www.usebruno.com/ as rest api client tool. **Done** (bruno folder included from the commit together with postman Emite.CCM API 1.0.postman_collection.json)

Testing:
- Write unit tests for your service layer
- Implement integration tests for your API endpoints


Bonus (Optional):
1. Implement pagination for calls and tickets. -  **Done** 
2. Add a search functionality to filter calls by status, date range, or agent -  **Done** 
3. Implement a simple in-memory cache for GET requests to improve performance -  **Done**
   <br>Sample Implementation From : 
    - Emite.CCM.Application.Features.CCM.Agent.Queries.GetAgentByIdQuery
    - Emite.CCM.Application.Features.CCM.Agent.Queries.GetAgentQuery
4. Create an endpoint to get basic statistics (e.g., average call duration, calls per agent) -  **Done** (Dashboard Controller)
5. Implement a simple rate limiting mechanism -  **Done**
   <br>Sample Implementation From : 
    - Emite.CCM.API.Controllers.v1.AgentController
6. Add real-time notifications for new calls using SignalR - **Done** (From the navigation menu of web UI, you can see a notification icon for number of tickets)
7. Implement a basic call routing algorithm to assign calls to available agents - **Done** (On assign agent end point, if agentid is null, the routing algorithm will execute)
8. If you know how to use Elasticsearch that would be a bonus. **Done** Implementation of Elastic Search from DashboardController/TicketsViaElasticSearch


# Test API Configuration
```bash
BaseUrl: https://localhost:48023
AuthUrl: https://localhost:48021
ClientId: 0332342A-FEA3-4250-B3EE-C96E423A0307
ClientSecret: ce31d66c-1971-4698-948d-d0b868e1a1f8
Version: 1
Scope: email profile roles name Entity demo_api Permission.Agent.View Permission.Agent.Create Permission.Agent.Delete Permission.Agent.Edit Permission.Agent.UpdateStatus Permission.Customer.View Permission.Customer.Create Permission.Customer.Delete Permission.Customer.Edit Permission.Call.View Permission.Call.Create Permission.Call.Delete Permission.Call.Edit Permission.Call.AssignToAgent Permission.Ticket.View Permission.Ticket.Create Permission.Ticket.Delete Permission.Ticket.Edit Permission.Ticket.AssignToAgent
```

