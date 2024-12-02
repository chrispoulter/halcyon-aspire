var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var postgresdb = postgres.AddDatabase("postgresdb");

var rabbitmq = builder
    .AddRabbitMQ("messaging")
    .WithDataVolume(isReadOnly: false)
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

var cache = builder
    .AddRedis("cache")
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var api = builder
    .AddProject<Projects.Halcyon_Api>("api")
    .WithExternalHttpEndpoints()
    .WithReference(postgresdb)
    .WithReference(cache)
    .WithReference(rabbitmq);

builder
    .AddNpmApp("web", "../halcyon-web", scriptName: "dev")
    .WithEnvironment("VITE_API_URL", api.GetEndpoint("https"))
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
