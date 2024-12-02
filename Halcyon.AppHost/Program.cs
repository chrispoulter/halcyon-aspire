var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Halcyon_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(cache);

builder
    .AddNpmApp("webfrontend", "../halcyon-web", scriptName: "dev")
    .WithEnvironment("VITE_API_URL", apiService.GetEndpoint("https"))
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
