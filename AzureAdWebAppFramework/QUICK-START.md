# Quick Start Guide - ASP.NET Framework Azure AD Authentication

## 🚀 What You Have

A complete **ASP.NET Framework 4.8** web application with:
- ✅ Azure Active Directory authentication via OWIN middleware
- ✅ OpenID Connect protocol implementation
- ✅ Claims-based identity management
- ✅ Protected routes and authorization
- ✅ Bootstrap 5 responsive UI
- ✅ Error handling and logging

## 📁 Project Structure

```
AzureAdWebAppFramework/
├── 🔧 App_Start/               # Configuration files
│   ├── Startup.Auth.cs         # Azure AD authentication setup
│   ├── RouteConfig.cs          # MVC routing
│   ├── FilterConfig.cs         # Global filters ([Authorize])
│   └── BundleConfig.cs         # CSS/JS bundling
├── 🎮 Controllers/             # MVC Controllers
│   ├── HomeController.cs       # Main pages (Index, About, Secure)
│   └── AccountController.cs    # Auth actions (Sign In/Out, Profile)
├── 🖼️ Views/                    # Razor views
│   ├── Home/                   # Home pages
│   ├── Account/                # Profile page
│   └── Shared/                 # Layout and shared views
├── ⚙️ Web.config                # Azure AD configuration
└── 📦 packages.config          # NuGet packages
```

## ⚡ Quick Setup (5 Minutes)

### 1. Azure AD App Registration

1. Go to [Azure Portal](https://portal.azure.com) → **Azure Active Directory** → **App registrations**
2. Click **New registration**:
   - **Name**: `AzureAdWebAppFramework`
   - **Redirect URI**: `https://localhost:44300/`
3. **Copy these values**:
   - Application (client) ID
   - Directory (tenant) ID
4. **Create client secret**:
   - Go to **Certificates & secrets** → **New client secret**
   - Copy the secret value immediately!

### 2. Update Configuration

Edit `Web.config` and replace placeholders:

```xml
<add key="ida:ClientId" value="YOUR_CLIENT_ID_HERE" />
<add key="ida:ClientSecret" value="YOUR_CLIENT_SECRET_HERE" />
<add key="ida:TenantId" value="YOUR_TENANT_ID_HERE" />
<add key="ida:Domain" value="your-domain.onmicrosoft.com" />
```

### 3. Run the Application

- **Visual Studio**: Press `F5`
- **IIS Express**: Use port 44300
- **URL**: `https://localhost:44300`

## 🧪 Test Authentication

1. **Home Page**: Shows auth status
2. **Sign In**: Click "Sign In" → Azure AD login
3. **Profile**: View user claims from Azure AD
4. **Secure Area**: Protected content demo
5. **Sign Out**: Clean logout

## 🔑 Key Features Demo

### Protected Routes
```csharp
[Authorize] // Requires authentication
public ActionResult Secure()
{
    // Only authenticated users can access
    return View();
}
```

### Claims Access
```csharp
// Get user information
var userName = User.Identity.Name;
var email = ((ClaimsIdentity)User.Identity).FindFirst("email")?.Value;
```

### Anonymous Access
```csharp
[AllowAnonymous] // Public access
public ActionResult Index()
{
    return View();
}
```

## 🛠️ Core Components

### OWIN Authentication Pipeline
- **Startup.cs**: OWIN startup configuration
- **Startup.Auth.cs**: Azure AD + Cookie authentication
- **FilterConfig.cs**: Global [Authorize] filter

### Authentication Flow
1. User clicks "Sign In"
2. OWIN redirects to Azure AD
3. User authenticates with Azure AD
4. Azure AD returns to callback
5. OWIN creates authentication cookie
6. User accesses protected resources

### Claims Processing
- **Automatic**: Azure AD claims are mapped automatically
- **Custom**: Add custom claims in `SecurityTokenValidated`
- **Access**: Use `User.Identity` and `ClaimsIdentity`

## 🚨 Important Notes

### Security
- ⚠️ **Never commit secrets** to source control
- ✅ Use **HTTPS only** in production
- ✅ Configure **proper token validation**
- ✅ Set appropriate **session timeouts**

### Production Deployment
- Use **Azure Key Vault** for secrets
- Configure **environment-specific** settings
- Enable **application logging**
- Set up **monitoring** and **health checks**

## 🔧 Configuration Reference

### Required Azure AD Settings
```xml
<appSettings>
  <add key="ida:ClientId" value="..." />           <!-- App Registration ID -->
  <add key="ida:ClientSecret" value="..." />       <!-- Client Secret -->
  <add key="ida:TenantId" value="..." />            <!-- Directory ID -->
  <add key="ida:Domain" value="..." />              <!-- AD Domain -->
  <add key="ida:AADInstance" value="https://login.microsoftonline.com/" />
  <add key="ida:PostLogoutRedirectUri" value="https://localhost:44300/" />
  <add key="ida:RedirectUri" value="https://localhost:44300/" />
</appSettings>
```

### Required NuGet Packages
- Microsoft.Owin (4.2.0)
- Microsoft.Owin.Host.SystemWeb (4.2.0)
- Microsoft.Owin.Security.OpenIdConnect (4.2.0)
- Microsoft.Owin.Security.Cookies (4.2.0)
- System.IdentityModel.Tokens.Jwt (6.8.0)

## 🆘 Troubleshooting

### Common Issues
| Problem | Solution |
|---------|----------|
| Redirect URI mismatch | Check Azure AD app registration URLs |
| Invalid client secret | Verify secret and expiration |
| OWIN startup errors | Check assembly binding redirects |
| Claims not appearing | Verify token validation settings |

### Debug Tips
1. **Check browser dev tools** for redirect errors
2. **Enable trace logging** in Web.config
3. **Verify configuration** values are loaded
4. **Test with incognito** browser mode

## 📚 Next Steps

### Extend the Application
- Add **role-based authorization**
- Implement **custom claims processing**
- Create **Web API endpoints**
- Add **Microsoft Graph** integration
- Implement **multi-tenant** support

### Production Readiness
- Set up **Azure Key Vault**
- Configure **Application Insights**
- Implement **health checks**
- Add **comprehensive logging**
- Create **deployment pipeline**

## 🔗 Resources

- [Full README.md](./README.md) - Complete documentation
- [Azure AD Documentation](https://docs.microsoft.com/azure/active-directory/)
- [ASP.NET Framework Auth](https://docs.microsoft.com/aspnet/mvc/overview/security/)
- [OWIN Documentation](https://docs.microsoft.com/aspnet/aspnet/overview/owin-and-katana/)

---

**Ready to run!** Just configure Azure AD and start the application. 🎉