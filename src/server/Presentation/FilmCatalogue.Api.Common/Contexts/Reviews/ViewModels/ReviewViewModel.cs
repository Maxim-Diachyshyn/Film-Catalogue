using System;
using FilmCatalogue.Api.Common.Contexts.Films.ViewModels;
using FilmCatalogue.Domain.DataTypes.Reviews;

namespace FilmCatalogue.Api.Common.Contexts.Reviews.ViewModels
{
    public class ReviewViewModel
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public DateTime AddedAt { get; set; }
        public int Rate { get; set; }

        public ReviewViewModel(Review review)
        {
            Id = review.Id;
            Comment = review.Comment;
            AddedAt = review.AddedAt;
            Rate = review.Rate;
        }
    }
}