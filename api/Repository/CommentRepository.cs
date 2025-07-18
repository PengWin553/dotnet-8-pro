using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces; // add this manually if shortcut does not work
using api.Models; // add this manually if shortcut does not work
using Microsoft.EntityFrameworkCore; // add this manually
using api.Data; // add this manually
using api.Dto.Comment; // add this manually

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        // THIS IS DEPENDENCY INJECTION - constructor injection
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }
    }
}