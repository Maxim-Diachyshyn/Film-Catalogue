﻿using System;
using FilmCatalogue.Domain.DataTypes;
using FilmCatalogue.Domain.UseCases.Film.Commands;
using FilmCatalogue.Domain.UseCases.Film.Models;
using GraphQL.Types;

namespace FilmCatalogue.Api.GraphQL.Mutations
{
    public class AddFilmCommandInput
    {
        public string Name { get; set; }
        public DateTime ShowedDate { get; set; }
        public string Photo { get; set; }
    }

    public class AddFilmInput : InputObjectGraphType<AddFilmCommandInput>
    {
        public AddFilmInput()
        {
            Name = "AddFilmInput";
            Field(x => x.Name);
            Field(x => x.ShowedDate);
            Field(x => x.Photo);
        }
    }
}
