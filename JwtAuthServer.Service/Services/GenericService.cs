using JwtAuthServer.Core.Repositories;
using JwtAuthServer.Core.Services;
using JwtAuthServer.Core.UnitOfWork;
using JwtAuthServer.Service.Mapping.AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthServer.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class, new()
        where TDto : class, new()
    {
        private readonly IGenericRepository<TEntity> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GenericService(IGenericRepository<TEntity> genericRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _genericRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var data = await _genericRepository.GetAllAsync();
            return Response<IEnumerable<TDto>>.Success( ObjectMapper.Mapper.Map<List<TDto>>(data),200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var data = await _genericRepository.GetByIdAsync(id);
            if (data==null)
            {
                return Response<TDto>.Fail("UserNotFound", 404, true);
            }
            return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(data), 200);
        }

        public void Remove(TEntity entity)
        {
            _genericRepository.Remove(entity);
            _unitOfWork.Commit();
        }

        public Response<NoDataDto> Update(TEntity entity)
        {
            _genericRepository.Update(entity);
            _unitOfWork.Commit();
            return Response<NoDataDto>.Success(200);
        }

        public async Task< Response<IEnumerable<TDto>> >Where(Expression<Func<TEntity, bool>> expression)
        {
            var queryList =  _genericRepository.Where(expression);
          
            return  Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<List<TDto>>(await queryList.ToListAsync()), 200);
        }
    }
}
