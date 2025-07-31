# Azure AD Web Application

This ASP.NET Core web application demonstrates how to implement Azure Active Directory (Azure AD) authentication using Microsoft Identity Web library.

## Features

- **Azure AD Authentication**: Secure login using Azure Active Directory
- **User Profile**: Display user information from Azure AD claims
- **Protected Routes**: Pages that require authentication to access
- **Clean UI**: Bootstrap-based responsive design
- **Claims Display**: View all user claims from Azure AD

## Prerequisites

1. **Azure Subscription**: You need an active Azure subscription
2. **.NET 8 SDK**: Download from [Microsoft .NET](https://dotnet.microsoft.com/download)
3. **Azure AD Tenant**: Access to an Azure AD tenant where you can register applications

## Azure AD Setup

### 1. Register Application in Azure AD

1. Go to the [Azure Portal](https://portal.azure.com)
2. Navigate to **Azure Active Directory** → **App registrations**
3. Click **New registration**
4. Fill in the details:
   - **Name**: `AzureAdWebApp` (or your preferred name)
   - **Supported account types**: Choose based on your needs
   - **Redirect URI**: Select **Web** and enter `https://localhost:7000/signin-oidc`
5. Click **Register**

### 2. Configure Application Settings

After registration, note down these values:
- **Application (client) ID**
- **Directory (tenant) ID**

### 3. Create Client Secret

1. In your app registration, go to **Certificates & secrets**
2. Click **New client secret**
3. Add a description and select expiration
4. Copy the **Value** (this is your client secret)

### 4. Configure Authentication

1. Go to **Authentication** in your app registration
2. Under **Implicit grant and hybrid flows**, check:
   - **ID tokens**
3. Under **Advanced settings**:
   - **Allow public client flows**: No
4. Save the changes

## Application Configuration

### 1. Update appsettings.json

Replace the placeholder values in `appsettings.json`:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "your-domain.onmicrosoft.com",
    "TenantId": "your-tenant-id-here",
    "ClientId": "your-client-id-here", 
    "ClientSecret": "your-client-secret-here",
    "CallbackPath": "/signin-oidc"
  }
}
```

**Where to find these values:**
- **TenantId**: Directory (tenant) ID from Azure AD
- **ClientId**: Application (client) ID from app registration
- **ClientSecret**: The secret value you created
- **Domain**: Your Azure AD domain (usually yourcompany.onmicrosoft.com)

### 2. Environment Variables (Recommended for Production)

For production environments, use environment variables or Azure Key Vault instead of storing secrets in appsettings.json:

```bash
export AzureAd__TenantId="your-tenant-id"
export AzureAd__ClientId="your-client-id"
export AzureAd__ClientSecret="your-client-secret"
export AzureAd__Domain="your-domain.onmicrosoft.com"
```

## Running the Application

### 1. Restore Dependencies

```bash
dotnet restore
```

### 2. Run the Application

```bash
dotnet run
```

The application will start at `https://localhost:7000` (or the port shown in the console).

### 3. Test Authentication

1. Navigate to the application in your browser
2. Click **Sign in with Azure AD**
3. Enter your Azure AD credentials
4. After successful login, you'll be redirected back to the application
5. Access the **Profile** page to view your user information
6. Try the **Admin** page to see protected content

## Project Structure

```
AzureAdWebApp/
├── Pages/
│   ├── Shared/
│   │   └── _Layout.cshtml          # Main layout with auth UI
│   ├── Index.cshtml                # Home page with auth status
│   ├── Profile.cshtml              # Protected user profile page
│   ├── Admin.cshtml                # Protected admin page
│   └── Privacy.cshtml              # Privacy policy page
├── Program.cs                      # Application startup and auth config
├── appsettings.json               # Configuration including Azure AD
└── README.md                      # This file
```

## Key Components

### Authentication Configuration (Program.cs)

```csharp
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});
```

### Protected Pages

Pages use the `[Authorize]` attribute to require authentication:

```csharp
[Authorize]
public class ProfileModel : PageModel
{
    // Page requires authentication
}
```

### User Claims Access

Access user information through claims:

```csharp
@User.Identity.Name                    // User's display name
@User.FindFirst("email")?.Value        // Email address
@User.FindFirst("sub")?.Value          // User ID
```

## Security Considerations

1. **Never commit secrets**: Use environment variables or Azure Key Vault
2. **HTTPS only**: Always use HTTPS in production
3. **Token validation**: The library handles token validation automatically
4. **Session management**: Configure appropriate session timeouts
5. **Logging**: Monitor authentication events and failures

## Troubleshooting

### Common Issues

1. **Redirect URI mismatch**: Ensure the redirect URI in Azure AD matches your application URL
2. **Invalid client secret**: Check that the client secret is correct and not expired
3. **Tenant ID issues**: Verify you're using the correct tenant ID
4. **Permission errors**: Ensure your user account has access to the Azure AD tenant

### Debugging Tips

1. Enable detailed logging in `appsettings.Development.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.AspNetCore.Authentication": "Debug",
      "Microsoft.Identity": "Debug"
    }
  }
}
```

2. Check the browser's developer tools for redirect issues
3. Verify all configuration values are correctly set

## Additional Resources

- [Microsoft Identity Web Documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/microsoft-identity-web)
- [Azure AD App Registration Guide](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)
- [ASP.NET Core Authentication Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)

## License

This project is licensed under the MIT License.