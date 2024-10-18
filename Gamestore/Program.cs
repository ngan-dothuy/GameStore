using Gamestore.Data;
using Gamestore.Dtos;
using Gamestore.Endpoints;

// builder is an object WebApplicationBuilder in ASP.NET which allows to config application
var builder = WebApplication.CreateBuilder(args); // builder object to register services

var connString = "Data Source = GameStore.db";  // connect to DB

/*AddSqlite: use to register "GameStoreContext" with EntityFramework 
to indicate that the application will use SQLite as the database.*/
builder.Services.AddSqlite<GameStoreContext>(connString); 


var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndpoints();
await app.MigrateDbAsync();

app.Run();
