using IFICamarAPI.Application.Pagings;
using System.Xml.Linq;
using IFICamarAPI.Application.Requests.PlaceOfPosting.Commands;
using IFICamarAPI.Domain.Entities.PostingPlace;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IFICamarAPI.Application.Requests.PlaceOfPosting.Queries;

namespace IFICamarAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostingPlaceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostingPlaceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdatePostingPlace(PostingPlace command)
        {
            try
            {
                var result = await _mediator.Send(new CreateOrUpdatePostingPlace(command));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPostingPlaces(int? id, string? placeName, string? isActive, string? getAll, int currentPage, int itemsPerPage)
        {
            try
            {
                var result = await _mediator.Send(new GetPostingPlaces(id, placeName, isActive, getAll, currentPage, itemsPerPage));

                PaginationHeader.Add(Response, result.CurrentPage, result.ItemsPerPage, result.TotalPages, result.TotalItems);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
