var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Patients_API>("patients-api");

builder.Build().Run();
