using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VoucherManagementApi.Dto;
using VoucherManagementApi.RepositoryContract;

namespace VoucherManagementApi.Repository
{
    public abstract class BaseRepository<TContext, TEntity, TDto, TDtoType> : IBaseRepository<TDto>
        where TContext : DbContext, new()
        where TEntity : class, new()
        where TDto : BaseDto<TDtoType>, new()
    {
        protected BaseRepository(TContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        protected IMapper Mapper { get; }

        protected TContext Context { get; }

        public virtual async Task<TDto> InsertAsync(TDto dto)
        {
            try
            {
                var entity = new TEntity();
                DtoToEntity(dto, entity);

                var dbSet = this.Context.Set<TEntity>();
                dbSet.Add(entity);
                var numObj = await this.Context.SaveChangesAsync();
                if (numObj > 0)
                {
                    var type = entity.GetType();
                    var prop = type.GetProperty("Id");
                    dto.Id = (TDtoType)Convert.ChangeType(prop.GetValue(entity).ToString(), typeof(TDtoType));
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dto;
        }

        public virtual async Task<TDto> ReadAsync(object primaryKey)
        {
            var dbSet = this.Context.Set<TEntity>();
            var entity = await dbSet.FindAsync(primaryKey);
            if (entity == null) return null;

            var dto = new TDto();
            EntityToDto(entity, dto);
            return dto;
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            var dbSet = this.Context.Set<TEntity>();

            var entity = await dbSet.FindAsync(dto.Id);
            if (entity == null) return null;

            DtoToEntity(dto, entity);
            dbSet.Update(entity);

            await this.Context.SaveChangesAsync();

            return dto;
        }
        
        public virtual TDto Update(TDto dto)
        {
            var dbSet = this.Context.Set<TEntity>();

            var entity = dbSet.Find(dto.Id);
            if (entity == null) return null;

            DtoToEntity(dto, entity);
            dbSet.Update(entity);

            this.Context.SaveChangesAsync();

            return dto;
        }

        public virtual async Task<TDto> DeleteAsync(object primaryKey)
        {
            var dbSet = this.Context.Set<TEntity>();

            var entity = await dbSet.FindAsync(primaryKey);
            if (entity == null) return null;

            var dto = new TDto();
            EntityToDto(entity, dto);

            dbSet.Remove(entity);
            await this.Context.SaveChangesAsync();

            return dto;
        }

        public virtual TDto Delete(object primaryKey)
        {
            var dbSet = this.Context.Set<TEntity>();

            var entity = dbSet.Find(primaryKey);
            if (entity == null) return null;

            var dto = new TDto();
            EntityToDto(entity, dto);

            dbSet.Remove(entity);
            this.Context.SaveChanges();

            return dto;
        }

        protected virtual void DtoToEntity(TDto dto, TEntity entity)
        {
            Mapper.Map(dto, entity);
        }

        protected virtual void EntityToDto(TEntity entity, TDto dto)
        {
            Mapper.Map(entity, dto);
        }

        protected virtual void EntityToDtoWithRelation(TEntity entity, TDto dto)
        {
            Mapper.Map(entity, dto);
        }

        protected bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            var dbSet = this.Context.Set<TEntity>();
            var result = dbSet.Any(predicate);

            return result;
        }
    }
}
