﻿using FilmCatalogue.Domain.DataTypes;
using FilmCatalogue.Domain.UseCases.Film.Models;
using MediatR;
using System;

namespace FilmCatalogue.Domain.UseCases.Film.Commands
{
    public class UpdateFilmCommand : IRequest<FilmModel>
    {       
        public Id FilmId { get; set; }
        public string Name { get; set; }
        public DateTime ShowedDate { get; set; }
        public Blob Photo { get; set; }
    }
}
