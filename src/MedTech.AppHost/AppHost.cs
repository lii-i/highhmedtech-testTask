var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

var db = postgres.AddDatabase("medtech");

builder.AddProject<Projects.api>("api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
