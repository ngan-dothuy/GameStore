using System;
using Gamestore.Data;
using Gamestore.Dtos;
using Gamestore.Entities;
using Gamestore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.Endpoints;

public static class GamesEndpoint
{
    const string GetGameEndpointName = "GetGame";



    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // Get /games
        group.MapGet("/", async (GameStoreContext dbContext) =>
        await dbContext.Games
        .Include(game => game.Genre)
        .Select(game => game.ToGameSummaryDto())
        .AsNoTracking()
        .ToListAsync());

        // get /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ?
            Results.NoContent() : Results.Ok(game.ToGameDetailsDto());

        }).WithName(GetGameEndpointName);



        // Post /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            //game.Genre = dbContext.Genres.Find(newGame.GenreId);

            /*
                        Game game = new()
                        {
                            Name = newGame.Name,
                            Genre = dbContext.Genres.Find(newGame.GenreId),
                            GenreId = newGame.GenreId,
                            Price = newGame.Price,
                            ReleaseDate = newGame.ReleaseDate,
                        };*/

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync(); // transform all of the changes that have been tracked by EF - add the new entity
            /*
                        GameDto gameDto = new(
                            game.Id,
                            game.Name,
                            game.Genre!.Name, // ! (Null-forgiving operator) => make sure Genre is not null when retreiving data
                            game.Price,
                            game.ReleaseDate

                        ); // contract of Dto when responding data*/

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());

        });

        // Put /games
        group.MapPut("/{id}", async(int id, UpdateGameDto updateGame, GameStoreContext dbContext) =>
        {
            //var index = games.FindIndex(game => game.Id == id);
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }
            dbContext.Entry(existingGame).CurrentValues.SetValues(updateGame.ToEntity(id));

            await dbContext.SaveChangesAsync();


            return Results.NoContent();

        });

        // delete /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            //games.RemoveAll(game => game.Id == id);
            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
            
            return Results.NoContent();
        });

        return group;
    }




}
