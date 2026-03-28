# ContestParticipationSystem

Lightweight ASP.NET Core 8 Web API to manage contests, participants and scoring. Includes JWT authentication, role-based authorization, rate limiting, EF Core persistence and Swagger UI.

## Features
- User authentication with JWT (HS256)
- Role-based authorization (ADMIN / VIP / USER)
- Rate limiting and exception middleware
- EF Core (SQL Server) data store
- Swagger UI for API exploration
- Sample unit tests using xUnit and EF Core InMemory

## Prerequisites
- .NET 8 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 / VS Code or other editor

## Configuration
Application reads configuration from `appsettings.json` or environment variables. Important keys:
- `ConnectionStrings:Default` — SQL Server connection string
- `Jwt:Key` — signing key for JWTs (MUST be at least 32 bytes / 256 bits when decoded)
- `Jwt:Issuer`, `Jwt:Audience` — optional

Do not commit real secrets. Use environment variables, user-secrets, or a secret manager for production.

Example `appsettings.Sample.json` (provided in repository):

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost\\SQLEXPRESS;Database=ContestDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "<BASE64_32BYTE_KEY_OR_ENV_VAR>",
    "Issuer": "ContestAPI",
    "Audience": "ContestUsers"
  }
}
```

### Generate a secure Base64 32-byte key
- PowerShell:
```
$bytes = New-Object byte[] 32; (New-Object System.Security.Cryptography.RNGCryptoServiceProvider).GetBytes($bytes); [Convert]::ToBase64String($bytes)
```
- OpenSSL:
```
openssl rand -base64 32
```

## Run the API (local)
1. Copy `ContestParticipationSystem/appsettings.Sample.json` to `ContestParticipationSystem/appsettings.json` and fill values, or set environment variables.
2. From the solution root:
```
dotnet restore
# if migrations exist
dotnet ef database update
dotnet run --project ContestParticipationSystem
```
3. Open Swagger UI: `https://localhost:<port>/swagger`

The app validates the JWT key length at startup and will fail fast with a clear error if the key is too short.

## Tests
Tests should use xUnit and EF Core InMemory for fast, isolated unit tests. Recommended test packages:
- `xunit`
- `Microsoft.AspNetCore.Mvc.Testing` (for integration tests)
- `Microsoft.EntityFrameworkCore.InMemory`
- `FluentAssertions` (optional)
- `Moq` (optional)

Create a test project (example):
```
dotnet new xunit -o tests/ContestParticipationSystem.Tests
cd tests/ContestParticipationSystem.Tests
dotnet add reference ../../ContestParticipationSystem/ContestParticipationSystem.csproj
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package FluentAssertions
dotnet add package Moq
```

Run tests:
```
dotnet test tests/ContestParticipationSystem.Tests
```

### Example unit test: `JwtServiceTests`
Create `tests/ContestParticipationSystem.Tests/JwtServiceTests.cs` and use a 32-byte key (or Base64-decoded bytes) to validate token issuance and validation.

Key ideas for tests:
- Use an in-memory EF Core provider to construct an `AppDbContext` for services that depend on the DB.
- For `JwtService`, supply a valid 32-byte key and assert generated token validates using the same key.
- For controller/integration tests, use `WebApplicationFactory<TEntryPoint>` from `Microsoft.AspNetCore.Mvc.Testing`.

## Security notes
- Never commit secrets. Add `ContestParticipationSystem/appsettings.json` to `.gitignore` (done).
- Rotate keys if they were previously committed anywhere.
- For production, use a secret manager (Azure Key Vault, AWS Secrets Manager, etc.).

## Troubleshooting
- `IDX10720` on token generation: JWT signing key length is insufficient. Ensure `Jwt:Key` decodes to at least 32 bytes.
- `Invalid signature`: ensure issuer and validator use the same key/encoding.

## Contributing
Fork, open issues, or submit pull requests. Use `appsettings.Sample.json` as a template for configuration.

---
Updated README with test instructions and secure-sharing guidance.
