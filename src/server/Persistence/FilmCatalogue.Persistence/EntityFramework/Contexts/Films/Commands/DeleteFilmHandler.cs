﻿using FilmCatalogue.Application.UseCases.Films.Commands;
using FilmCatalogue.Persistence.EntityFramework.Contexts.Films.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FilmCatalogue.Persistence.EntityFramework.Contexts.Films.Commands
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
            _context.Remove(new FilmEntity { Id = request.FilmId });
            await _context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
