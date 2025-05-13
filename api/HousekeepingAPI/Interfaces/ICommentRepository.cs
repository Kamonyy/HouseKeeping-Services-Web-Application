using System.Collections.Generic;
using System.Threading.Tasks;
using HousekeepingAPI.Models;
using HousekeepingAPI.Dto.Comment;

namespace HousekeepingAPI.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetByIdAsync(int id);
        Task<ICollection<Comment>> GetAllByServiceIdAsync(int serviceId);
        Task<Comment> CreateAsync(CommentCreateDto commentDto, string userId);
        Task<bool> UpdateAsync(CommentUpdateDto commentDto);
        Task<bool> DeleteAsync(int id);
        Task<double> GetAverageRatingForServiceAsync(int serviceId);
    }
}
