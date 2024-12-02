var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Halcyon_ApiService>("apiservice").WithReference(cache);

builder.AddViteApp("halcyon-web").WithNpmPackageInstallation().WithExternalHttpEndpoints().WithReference(apiService);

builder.Build().Run();
