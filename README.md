Migrate database
1. Change connection string in Program.cs
   builder.Services.AddDbContext<ChatDbContext>(
    options => options.UseSqlServer("..."));
2. Open cmd in run this
    dotnet ef database update Initial --context ChatDbContext   
