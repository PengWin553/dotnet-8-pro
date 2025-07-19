using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; // add this manually
using api.Data; // add this manually
using api.Models; // add this manually
using api.Mappers; // add this manually
using api.Dto.Comment; // add this manually
using api.Interfaces; // add this manually

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        // THIS IS DEPENDENCY INJECTION - constructor injection
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(c => c.ToCommentDto());
            
            return Ok(commentDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            if(comment == null){
                return NotFound();
            }

            return Ok(comment.ToCommentDto()); // return one
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto) // 1
        {
            if(!await _stockRepo.StockExists(stockId))  // 2
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = commentDto.ToCommentFromCreate(stockId); // 1
            await _commentRepo.CreateAsync(commentModel);  // 3 and 4
            return CreatedAtAction(nameof(GetById) , new { id = commentModel}, commentModel.ToCommentDto()); // 5
        }
    }
}