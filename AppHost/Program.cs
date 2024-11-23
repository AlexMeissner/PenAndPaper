var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Server>("API");

builder.AddProject<Projects.Website>("Website").WithReference(api);

builder.Build().Run();
