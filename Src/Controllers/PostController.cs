using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Src.Data;
using api.Src.Dtos;
using api.Src.Interfaces;
using api.Src.Mappers;
using api.Src.Models;
using api.Src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ImageService _imageService;

        public PostController(IPostRepository postRepository, ImageService imageService)
        {
            _postRepository = postRepository;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            var result = posts.Select(PostMapper.ToDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] PostCreateDto dtoPost)
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);
            var post = PostMapper.ToModel(dtoPost, userId);
            await _postRepository.AddNewPostAsync(post);
            return CreatedAtAction(nameof(CreatePost), new { id = post.Id }, post);
        }
    }
}
