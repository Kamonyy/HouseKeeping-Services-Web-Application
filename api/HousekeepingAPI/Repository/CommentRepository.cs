using System.Collections.Generic;
using System.Threading.Tasks;
using HousekeepingAPI.Data;
using HousekeepingAPI.Interfaces;
using HousekeepingAPI.Models;
using Microsoft.EntityFrameworkCore;
using HousekeepingAPI.Dto.Comment;

namespace HousekeepingAPI.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Service)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ICollection<Comment>> GetAllByServiceIdAsync(int serviceId)
        {
            return await _context.Comments
                .Where(c => c.ServiceId == serviceId)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<Comment> CreateAsync(CommentCreateDto commentDto, string userId)
        {
            var comment = new Comment
            {
                Content = commentDto.Content,
                UserId = userId,
                ServiceId = commentDto.ServiceId,
                CreatedDate = DateTime.UtcNow
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> UpdateAsync(CommentUpdateDto commentDto)
        {
            var comment = await _context.Comments.FindAsync(commentDto.Id);
            if (comment == null) return false;

            comment.Content = commentDto.Content;
            comment.UserId = commentDto.UserId;
            comment.ServiceId = commentDto.ServiceId;

            _context.Comments.Update(comment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
