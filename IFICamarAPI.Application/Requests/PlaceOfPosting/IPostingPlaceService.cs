using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Application.Common.Models;
using IFICamarAPI.Application.Pagings;
using IFICamarAPI.Application.Requests.PlaceOfPosting.Queries;
using IFICamarAPI.Domain.Entities.PostingPlace;

namespace IFICamarAPI.Application.Requests.PlaceOfPosting
{
    public interface IPostingPlaceService : IDisposable
    {
        Task<Result> CreateOrUpdatePostingPlace(PostingPlace request);
        Task<PagedList<PostingPlaceVM>> GetPostingPlaces(GetPostingPlaces request);
    }
}
