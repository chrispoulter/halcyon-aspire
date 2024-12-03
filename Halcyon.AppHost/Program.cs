var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("database");

var rabbitmq = builder
    .AddRabbitMQ("rabbitmq")
    .WithDataVolume(isReadOnly: false)
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent);

var redis = builder
    .AddRedis("redis")
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var api = builder
    .AddProject<Projects.Halcyon_Api>("api")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WithReference(rabbitmq)
    .WithReference(redis);

builder
    .AddNpmApp("web", "../halcyon-web", scriptName: "dev")
    .WithEnvironment("VITE_API_URL", api.GetEndpoint("https"))
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
