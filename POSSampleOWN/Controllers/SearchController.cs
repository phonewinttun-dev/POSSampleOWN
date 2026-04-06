using Microsoft.AspNetCore.Mvc;
using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;
using POSSampleOWN.Services;

namespace POSSampleOWN.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }


        // GET : api/search?term=searchTerm
        [HttpGet]
        public async Task<IActionResult> ExecuteSearch([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Search term cannot be empty.",
                });
            }
            try
            {
                var results = await _searchService.GeneralSearchAsync(term);
                return Ok(ApiResponse<List<SearchDTO>>.Success(results, "Successfully Retrieved"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SearchDTO>.Fail($"An error occurred while retrieving: {ex.Message}"));
            }
        }



    }
}
