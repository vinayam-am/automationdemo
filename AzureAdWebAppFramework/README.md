# Azure AD Web Application (ASP.NET Framework)

This ASP.NET Framework 4.8 MVC application demonstrates how to implement Azure Active Directory (Azure AD) authentication using OWIN middleware and OpenID Connect.

## Features

- **Azure AD Authentication**: Secure login using Azure Active Directory
- **OWIN Middleware**: Modern authentication pipeline using OWIN
- **Claims-Based Identity**: Access user information from Azure AD claims
- **Protected Routes**: Controllers/actions that require authentication
- **Session Management**: Automatic session handling with cookies
- **Responsive UI**: Bootstrap 5-based modern interface
- **Error Handling**: Comprehensive error handling and logging

## Prerequisites

1. **Visual Studio**: Visual Studio 2019 or later (or Visual Studio Code with appropriate extensions)
2. **Azure Subscription**: Active Azure subscription with Azure AD tenant
3. **.NET Framework 4.8**: Installed on development machine
4. **IIS Express**: For local development (included with Visual Studio)

## Azure AD Setup

### 1. Register Application in Azure AD

1. Go to the [Azure Portal](https://portal.azure.com)
2. Navigate to **Azure Active Directory** → **App registrations**
3. Click **New registration**
4. Fill in the registration details:
   - **Name**: `AzureAdWebAppFramework` (or your preferred name)
   - **Supported account types**: Choose based on your requirements
   - **Redirect URI**: Select **Web** and enter `https://localhost:44300/`
5. Click **Register**

### 2. Configure Application Settings

After registration, note down these values from the **Overview** page:
- **Application (client) ID**
- **Directory (tenant) ID**

### 3. Create Client Secret

1. In your app registration, go to **Certificates & secrets**
2. Click **New client secret**
3. Add a description and select expiration period
4. Copy the **Value** (this is your client secret - save it immediately)

### 4. Configure Authentication

1. Go to **Authentication** in your app registration
2. Under **Implicit grant and hybrid flows**, check:
   - **ID tokens (used for implicit and hybrid flows)**
3. Under **Advanced settings**:
   - **Allow public client flows**: No
4. Add additional redirect URIs if needed:
   - `https://localhost:44300/`
   - `https://localhost:44300/signin-oidc`
5. Save the changes

## Application Configuration

### 1. Update Web.config

Replace the placeholder values in `Web.config`:

```xml
<appSettings>
  <!-- Azure AD Configuration -->
  <add key="ida:ClientId" value="your-client-id-here" />
  <add key="ida:ClientSecret" value="your-client-secret-here" />
  <add key="ida:AADInstance" value="https://login.microsoftonline.com/" />
  <add key="ida:TenantId" value="your-tenant-id-here" />
  <add key="ida:Domain" value="your-domain.onmicrosoft.com" />
  <add key="ida:PostLogoutRedirectUri" value="https://localhost:44300/" />
  <add key="ida:RedirectUri" value="https://localhost:44300/" />
</appSettings>
```

**Configuration Values:**
- **ida:ClientId**: Application (client) ID from Azure AD
- **ida:ClientSecret**: The client secret you created
- **ida:TenantId**: Directory (tenant) ID from Azure AD  
- **ida:Domain**: Your Azure AD domain (usually yourcompany.onmicrosoft.com)
- **ida:PostLogoutRedirectUri**: Where to redirect after logout
- **ida:RedirectUri**: Your application's base URL

### 2. Install NuGet Packages

The following packages are required and should be referenced in `packages.config`:

```xml
<package id="Microsoft.Owin" version="4.2.0" targetFramework="net48" />
<package id="Microsoft.Owin.Host.SystemWeb" version="4.2.0" targetFramework="net48" />
<package id="Microsoft.Owin.Security" version="4.2.0" targetFramework="net48" />
<package id="Microsoft.Owin.Security.Cookies" version="4.2.0" targetFramework="net48" />
<package id="Microsoft.Owin.Security.OpenIdConnect" version="4.2.0" targetFramework="net48" />
<package id="System.IdentityModel.Tokens.Jwt" version="6.8.0" targetFramework="net48" />
```

### 3. Production Configuration

For production environments, use secure configuration methods:

**Option 1: Environment Variables**
```csharp
// In Startup.Auth.cs, read from environment variables
private static string clientId = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_ID") ?? 
                                 ConfigurationManager.AppSettings["ida:ClientId"];
```

**Option 2: Azure Key Vault**
```csharp
// Use Azure Key Vault for secrets in production
// Install: Microsoft.Azure.KeyVault and Microsoft.Azure.Services.AppAuthentication
```

## Running the Application

### 1. Using Visual Studio

1. Open `AzureAdWebAppFramework.csproj` in Visual Studio
2. Restore NuGet packages (should happen automatically)
3. Set the project as startup project
4. Press **F5** or click **Start Debugging**
5. The application will launch at `https://localhost:44300`

### 2. Using IIS Express (Command Line)

```bash
# Navigate to project directory
cd AzureAdWebAppFramework

# Start IIS Express
"C:\Program Files\IIS Express\iisexpress.exe" /path:"C:\path\to\your\project" /port:44300 /systray:true
```

### 3. Testing Authentication

1. Navigate to `https://localhost:44300` in your browser
2. Click **Sign In with Azure AD**
3. Enter your Azure AD credentials
4. After successful login, you'll be redirected back to the application
5. Access protected pages:
   - **Profile**: View your user information and claims
   - **Secure Area**: Demonstrates protected content

## Project Structure

```
AzureAdWebAppFramework/
├── App_Start/
│   ├── BundleConfig.cs          # CSS/JS bundling configuration
│   ├── FilterConfig.cs          # Global filters (including [Authorize])
│   ├── RouteConfig.cs           # MVC routing configuration
│   └── Startup.Auth.cs          # OWIN authentication configuration
├── Controllers/
│   ├── AccountController.cs     # Authentication actions (Sign In/Out, Profile)
│   └── HomeController.cs        # Main application controllers
├── Views/
│   ├── Account/
│   │   └── Profile.cshtml       # User profile page
│   ├── Home/
│   │   ├── Index.cshtml         # Home page with auth status
│   │   ├── About.cshtml         # About page
│   │   ├── Contact.cshtml       # Contact page
│   │   └── Secure.cshtml        # Protected content page
│   └── Shared/
│       ├── _Layout.cshtml       # Main layout with auth UI
│       └── Error.cshtml         # Error handling page
├── Content/                     # CSS files
├── Scripts/                     # JavaScript files
├── Global.asax.cs              # Application startup
├── Startup.cs                  # OWIN startup
├── Web.config                  # Configuration and Azure AD settings
└── packages.config             # NuGet package references
```

## Key Components

### 1. OWIN Authentication Configuration

**Startup.Auth.cs** configures the authentication pipeline:

```csharp
public void ConfigureAuth(IAppBuilder app)
{
    app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

    // Configure cookie authentication
    app.UseCookieAuthentication(new CookieAuthenticationOptions
    {
        AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
        LoginPath = new PathString("/Account/SignIn"),
        ExpireTimeSpan = TimeSpan.FromMinutes(60)
    });

    // Configure OpenID Connect with Azure AD
    app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
    {
        ClientId = clientId,
        Authority = authority,
        PostLogoutRedirectUri = postLogoutRedirectUri,
        RedirectUri = redirectUri,
        Scope = "openid profile email",
        ResponseType = "id_token"
    });
}
```

### 2. Protected Controllers

Controllers use `[Authorize]` attribute for protection:

```csharp
[Authorize] // Requires authentication
public class SecureController : Controller
{
    public ActionResult Index()
    {
        var identity = (ClaimsIdentity)User.Identity;
        ViewBag.UserClaims = identity.Claims.ToList();
        return View();
    }
}
```

### 3. Claims Access

Access user information through claims:

```csharp
// In controllers or views
var displayName = User.Identity.Name;
var email = ((ClaimsIdentity)User.Identity).FindFirst("email")?.Value;
var objectId = ((ClaimsIdentity)User.Identity).FindFirst("oid")?.Value;
```

### 4. Anonymous Access

Allow anonymous access to specific actions:

```csharp
[AllowAnonymous] // Allows anonymous access
public ActionResult PublicPage()
{
    return View();
}
```

## Security Considerations

### 1. Configuration Security
- **Never commit secrets**: Keep client secrets out of source control
- **Use secure storage**: Azure Key Vault for production secrets
- **Environment-specific configs**: Different configs for dev/staging/production

### 2. Authentication Security
- **HTTPS only**: Always use HTTPS in production
- **Token validation**: Configure proper token validation parameters
- **Session management**: Configure appropriate session timeouts
- **CSRF protection**: Use anti-forgery tokens for sensitive operations

### 3. Authorization
- **Role-based access**: Implement role-based authorization where needed
- **Claim-based access**: Use claims for fine-grained access control
- **Resource protection**: Protect sensitive resources and APIs

## Troubleshooting

### Common Issues

1. **Redirect URI mismatch**
   - Ensure redirect URIs in Azure AD match your application URLs
   - Check both base URL and `/signin-oidc` callback

2. **Invalid client secret**
   - Verify the client secret is correct and not expired
   - Check for extra spaces or characters when copying

3. **Token validation errors**
   - Ensure tenant ID is correct
   - Check if `ValidateIssuer` is properly configured

4. **OWIN middleware conflicts**
   - Ensure correct package versions
   - Check assembly binding redirects in Web.config

### Debugging Tips

1. **Enable detailed logging** in Web.config:
```xml
<system.diagnostics>
  <trace autoflush="true">
    <listeners>
      <add name="textWriterTraceListener"
           type="System.Diagnostics.TextWriterTraceListener"
           initializeData="trace.log" />
    </listeners>
  </trace>
</system.diagnostics>
```

2. **Check browser developer tools** for redirect issues

3. **Verify configuration values** are correctly loaded

4. **Test with different browsers** to isolate issues

## Additional Features

### 1. Role-Based Authorization

Add role-based authorization:

```csharp
[Authorize(Roles = "Admin")]
public ActionResult AdminOnly()
{
    return View();
}
```

### 2. Custom Claims Processing

Process custom claims in `Startup.Auth.cs`:

```csharp
SecurityTokenValidated = (context) =>
{
    // Add custom claims or modify existing ones
    var identity = context.AuthenticationTicket.Identity;
    identity.AddClaim(new Claim("CustomClaim", "CustomValue"));
    return Task.FromResult(0);
}
```

### 3. API Protection

Protect Web API endpoints:

```csharp
[Authorize]
[Route("api/secure")]
public class SecureApiController : ApiController
{
    public IHttpActionResult Get()
    {
        return Ok("Protected data");
    }
}
```

## Resources

- [Azure AD Documentation](https://docs.microsoft.com/en-us/azure/active-directory/)
- [ASP.NET Framework Authentication](https://docs.microsoft.com/en-us/aspnet/mvc/overview/security/)
- [OWIN and Katana Documentation](https://docs.microsoft.com/en-us/aspnet/aspnet/overview/owin-and-katana/)
- [OpenID Connect Protocol](https://openid.net/connect/)
- [Azure Portal](https://portal.azure.com)

## License

This project is licensed under the MIT License.

## Support

For issues and questions:
1. Check the troubleshooting section above
2. Review Azure AD application configuration
3. Check application logs and browser developer tools
4. Consult Microsoft documentation for Azure AD and ASP.NET Framework