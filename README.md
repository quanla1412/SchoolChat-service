Migrate database
1. Change connection string in Program.cs
   builder.Services.AddDbContext<ChatDbContext>(
    options => options.UseSqlServer("..."));
2. Open cmd in run this
    dotnet ef database update --context ChatDbContext

Update database
1. Update file model
2. dotnet ef migrations add {Description} --context ChatDbContext
3. dotnet ef database update {Description} --context ChatDbContext