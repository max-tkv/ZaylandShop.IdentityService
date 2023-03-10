// ef commands templates for the Rider's terminal

cd ZaylandShop.IdentityService.Storage
dotnet restore
dotnet ef -h
dotnet ef migrations add Initial --verbose --project ../ZaylandShop.IdentityService.Storage --startup-project ../ZaylandShop.IdentityService.Web
dotnet ef database update --verbose --project ../ZaylandShop.IdentityService.Storage --startup-project ../ZaylandShop.IdentityService.Web