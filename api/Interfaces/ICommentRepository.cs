using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models; // add this manually
using api.Dto.Comment; // add this manually

namespace api.Interfaces
{
   public interface ICommentRepository
   {
        Task<List<Comment>> GetAllAsync();
   }
}