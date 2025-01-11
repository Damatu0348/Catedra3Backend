using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Src.Data;
using api.Src.Interfaces;
using api.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Src.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDBContext _context;

        public PostRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddNewPostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.Include(p => p.Usuario).ToListAsync();
        }
    }
}
