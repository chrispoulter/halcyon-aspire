var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Halcyon_ApiService>("apiservice").WithReference(cache);

builder
    .AddNpmApp("webfrontend", "../halcyon-web", scriptName: "dev")
    .WithReference(apiService)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
