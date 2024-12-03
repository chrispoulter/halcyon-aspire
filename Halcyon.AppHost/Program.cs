var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres(
        "postgres",
        builder.AddParameter("pgUsername", secret: true),
        builder.AddParameter("pgPassword", secret: true),
        port: 5432
    )
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("database", databaseName: "halcyon");

var rabbitmq = builder
    .AddRabbitMQ(
        "rabbitmq",
        builder.AddParameter("rmqUsername", secret: true),
        builder.AddParameter("rmqPassword", secret: true),
        port: 5672
    )
    .WithDataVolume(isReadOnly: false)
    .WithManagementPlugin(port: 15672)
    .WithLifetime(ContainerLifetime.Persistent);

var redis = builder
    .AddRedis("redis", port: 6379)
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var mailhog = builder
    .AddContainer("mailhog", "mailhog/mailhog")
    .WithEndpoint(port: 1025, targetPort: 1025, name: "smtp")
    .WithHttpEndpoint(port: 8025, targetPort: 8025)
    .WithExternalHttpEndpoints()
    .WithLifetime(ContainerLifetime.Persistent);

var api = builder
    .AddProject<Projects.Halcyon_Api>("api")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitFor(database)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(redis)
    .WaitFor(redis)
    .WaitFor(mailhog);

builder
    .AddNpmApp("web", "../halcyon-web", scriptName: "dev")
    .WithEnvironment("VITE_API_URL", api.GetEndpoint("https"))
    .WithHttpEndpoint(port: 3000, env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile()
    .WaitFor(api);

builder.Build().Run();
