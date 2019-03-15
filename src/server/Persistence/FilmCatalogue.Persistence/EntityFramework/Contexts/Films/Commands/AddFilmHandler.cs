﻿using FilmCatalogue.Domain.UseCases.Films.Commands;
using FilmCatalogue.Domain.UseCases.Films.Models;
using FilmCatalogue.Persistence.EntityFramework.Contexts.Films.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FilmCatalogue.Persistence.EntityFramework.Contexts.Films.Commands
{
    public class AddFilmHandler : IRequestHandler<AddFilmCommand, Film>
    {
        private readonly FilmDbContext _context;

        public AddFilmHandler(FilmDbContext context)
        {
            _context = context;
        }

        public async Task<Film> Handle(AddFilmCommand command, CancellationToken cancellationToken)
        {
            var entry = _context.Add(
                new FilmEntity
                {
                    Name = command.Name,
                    ShowedDate = command.ShowedDate,
                    Photo = command.Photo?.Data,
                    PhotoType = command.Photo?.Type
                }
            );
            await _context.SaveChangesAsync(cancellationToken);

            return entry.Entity.ToModel();
        }
    }
}
