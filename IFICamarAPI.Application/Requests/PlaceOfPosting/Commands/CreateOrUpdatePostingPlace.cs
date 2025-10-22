using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Application.Common.Models;
using IFICamarAPI.Domain.Entities.PostingPlace;
using MediatR;

namespace IFICamarAPI.Application.Requests.PlaceOfPosting.Commands
{
    public class CreateOrUpdatePostingPlace : IRequest<Result>
    {
        public PostingPlace PostingPlace { get; set; }

        public CreateOrUpdatePostingPlace(PostingPlace postingPlace)
        {
            PostingPlace = postingPlace;
        }
    }

    public class CreateOrUpdatePostingPlaceHandler : IRequestHandler<CreateOrUpdatePostingPlace, Result>
    {
        private readonly IPostingPlaceService _postingPlaceService;

        public CreateOrUpdatePostingPlaceHandler(IPostingPlaceService postingPlaceService)
        {
            _postingPlaceService = postingPlaceService ?? throw new ArgumentNullException(nameof(postingPlaceService));
        }

        public async Task<Result> Handle(CreateOrUpdatePostingPlace request, CancellationToken cancellationToken)
        {
            var result = await _postingPlaceService.CreateOrUpdatePostingPlace(request.PostingPlace);
            return result;
        }
    }
}
