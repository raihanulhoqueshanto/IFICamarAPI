using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Application.Pagings;
using MediatR;

namespace IFICamarAPI.Application.Requests.PlaceOfPosting.Queries
{
    public class GetPostingPlaces : PageParameters, IRequest<PagedList<PostingPlaceVM>>
    {
        public int? Id { get; set; }
        public string? PlaceName { get; set; }
        public string? IsActive { get; set; }
        public string? GetAll { get; set; }

        public GetPostingPlaces(int? id, string? placeName, string? isActive, string? getAll, int currentPage, int itemsPerPage) : base(currentPage, itemsPerPage)
        {
            Id = id;
            PlaceName = placeName;
            IsActive = isActive;
            GetAll = getAll;
        }
    }

    public class GetPostingPlacesHandler : IRequestHandler<GetPostingPlaces, PagedList<PostingPlaceVM>>
    {
        private readonly IPostingPlaceService _postingPlaceService;

        public GetPostingPlacesHandler(IPostingPlaceService postingPlaceService)
        {
            _postingPlaceService = postingPlaceService ?? throw new ArgumentNullException(nameof(postingPlaceService));
        }

        public async Task<PagedList<PostingPlaceVM>> Handle(GetPostingPlaces request, CancellationToken cancellationToken)
        {
            return await _postingPlaceService.GetPostingPlaces(request);
        }
    }
}
