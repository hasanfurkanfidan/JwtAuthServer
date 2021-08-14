using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthServer.Core.Services
{
    public interface IGenericService<TEntity, TDto> where TEntity : class, new() where TDto : class, new()
    {
        Task<Response<TDto>> GetByIdAsync(int id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        Response<TDto> Update(TEntity entity);
        Response<IEnumerable<TDto>> Where(Expression<Func<TEntity, bool>> expression);
    }
}
