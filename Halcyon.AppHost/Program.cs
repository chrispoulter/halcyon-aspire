var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Halcyon_ApiService>("apiservice").WithReference(cache);

builder
    .AddViteApp("webfrontend", "../halcyon-web")
    .WithNpmPackageInstallation()
    .WithEnvironment("VITE_API_URL", apiService.GetEndpoint("https"));

builder.Build().Run();
