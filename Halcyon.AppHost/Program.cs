var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("pgPassword", secret: true);

var postgres = builder
    .AddPostgres("postgres", password: postgresPassword, port: 5432)
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("database", databaseName: "halcyon");

var rabbitMqPassword = builder.AddParameter("rmqPassword", secret: true);

var rabbitmq = builder
    .AddRabbitMQ("rabbitmq", password: rabbitMqPassword, port: 5672)
    .WithDataVolume(isReadOnly: false)
    .WithManagementPlugin(port: 15672)
    .WithLifetime(ContainerLifetime.Persistent);

var redis = builder
    .AddRedis("redis", port: 6379)
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var maildev = builder
    .AddMailDev("maildev", httpPort: 1080, smtpPort: 1025)
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
    //.WithReference(maildev)
    .WaitFor(maildev);

builder
    .AddNpmApp("web", "../halcyon-web", scriptName: "dev")
    .WithEnvironment("VITE_API_URL", api.GetEndpoint("https"))
    .WithHttpEndpoint(port: 3000, env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile()
    .WaitFor(api);

builder.Build().Run();
