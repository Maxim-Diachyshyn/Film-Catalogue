﻿using FilmCatalogue.Domain.UseCases.Film.Commands.DeleteFilm;
using FilmCatalogue.Persistence.EntityFramework.Contexts.Film.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FilmCatalogue.Persistence.EntityFramework.Contexts.Film.Commands
{
    public class DeleteFilmHandler : IRequestHandler<DeleteFilmCommand>
    {
        private readonly FilmDbContext _context;

        public DeleteFilmHandler(FilmDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteFilmCommand request, CancellationToken cancellationToken)
        {
            _context.Films.Remove(new FilmEntity { Id = request.FilmId });
            await _context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
