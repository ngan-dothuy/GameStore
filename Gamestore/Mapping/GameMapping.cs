using System;
using Gamestore.Dtos;
using Gamestore.Entities;

namespace Gamestore.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDto game)
    {
        return new Game()
        {
            Name = game.Name,
            // Genre = dbContext.Genres.Find(newGame.GenreId),
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

    public static GameSummaryDto ToGameSummaryDto(this Game game)
    {
        return new (
                game.Id,
                game.Name,
                game.Genre!.Name, // ! (Null-forgiving operator) => make sure Genre is not null when retreiving data
                game.Price,
                game.ReleaseDate

            );
    }

     public static GameDetailsDto ToGameDetailsDto(this Game game)
    {
        return new (
                game.Id,
                game.Name,
                game.GenreId, // ! (Null-forgiving operator) => make sure Genre is not null when retreiving data
                game.Price,
                game.ReleaseDate

            );
    }

     public static Game ToEntity(this UpdateGameDto game, int id)
    {
        return new Game()
        {
            Id = id,
            Name = game.Name,
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

}
