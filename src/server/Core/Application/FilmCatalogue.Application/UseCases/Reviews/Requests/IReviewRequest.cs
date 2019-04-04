using System.Collections.Generic;
using FilmCatalogue.Domain.DataTypes.Common;

namespace FilmCatalogue.Application.UseCases.Reviews.Requests
{
    public interface IReviewRequest
    {
        IList<Id> SpecifiedIds { get; }
        Id FilmId { get; }
    }
}