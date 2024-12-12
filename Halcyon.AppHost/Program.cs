var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("pgPassword", secret: true);

var postgres = builder
    .AddPostgres("postgres", password: postgresPassword, port: 5432)
    .WithLifetime(ContainerLifetime.Persistent);

if (builder.ExecutionContext.IsRunMode)
{
    postgres.WithDataVolume(isReadOnly: true);
}

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

var maildevPassword = builder.AddParameter("mdPassword", secret: true);

var maildev = builder
    .AddMailDev("mail", password: maildevPassword, httpPort: 1080, smtpPort: 1025)
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
    .WithReference(maildev)
    .WaitFor(maildev);

var web = builder
    .AddNpmApp("web", "../halcyon-web", scriptName: "dev")
    .WithReference(api)
    .WithEnvironment("BROWSER", "none")
    .WithHttpEndpoint(port: 5173, env: "VITE_PORT", isProxied: false)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

api.WithEnvironment("Email__CdnUrl", web.GetEndpoint("http"));

builder.Build().Run();
