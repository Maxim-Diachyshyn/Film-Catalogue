﻿using System;
using FilmCatalogue.Domain.UseCases.Film.Commands;

namespace FilmCatalogue.Api.Web.Rest.Controllers.Film.Commands.Create
{
    public class CreateModel
    {
        public string Name { get; set; }
        public DateTime ShowedDate { get; set; }
    }
}