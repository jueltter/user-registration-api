using AutoMapper;
using user_registration_api.DefaultDomain.Data;
using user_registration_api.DefaultDomain.Models;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Exceptions;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Models;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace user_registration_api.DefaultDomain.Repositories.Impl;

public class UserRepository(
    IDbContextFactory<SqlServerDataContext> contextFactory,
    IMapper mapper,
    ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<Page<User>> SearchAsync(UserSearch? search)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            IQueryable<User> query = context.Users;

            if (search is { IdUser: not null })
            {
                query = query.Where(x => x.IdUser == search.IdUser);
            }

            if (search is { Sort: not null, Order: not null })
            {
                // TODO: implement sorting
            }
            else
            {
                query = DefaultOrderBy(query);
            }

            var totalItems = await query.CountAsync();

            var pagination = new PagePagination.Builder()
                .SetPage(search?.Page)
                .SetPageSize(search?.PageSize)
                .SetTotalItems(totalItems)
                .Build();

            if (search is { Page: not null, PageSize: not null })
            {
                query = query
                    .Skip(pagination.GetSkip())
                    .Take(pagination.PageSize);
            }

            var entities = await query.ToListAsync();
            logger.Log(LogLevel.Trace, "read entities {}", entities.Count);

            return Page<User>.Create(entities, pagination);
        }
        catch (Exception ex)
        {
            throw RepositoryException.ReadError(ex);
        }
    }

    public async Task<Option<User>> FindByIdAsync(long id)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var entity = await context.Users.FindAsync(id);
            var entityAsOption = Optional(entity);
            entityAsOption.Match(
                Some: _ => logger.Log(LogLevel.Trace, "read entity id: {}", id),
                None: () => logger.Log(LogLevel.Trace, "entity not found id: {}", id)
            );
            return entityAsOption;
        }
        catch (Exception ex)
        {
            throw RepositoryException.ReadError(ex);
        }
    }

    public async Task<User> SaveAsync(User model)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var entityEntry = await context.Users.AddAsync(model);
            await context.SaveChangesAsync();
            logger.Log(LogLevel.Trace, "created entity id: {}", entityEntry.Entity.IdUser);
            return entityEntry.Entity;
        }
        catch (Exception ex)
        {
            throw RepositoryException.CreateError(ex);
        }
    }

    public async Task<User> UpdateAsync(User model)
    {
        if (!model.IdUser.HasValue) throw new InvalidOperationException("El campo IdUser es nulo.");

        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var entityAsOption = await FindByIdAsync(model.IdUser.Value);
            var entity = entityAsOption.IfNone(() => throw new InvalidOperationException("El campo IdUser no corresponde a ningún registro."));
            mapper.Map(model, entity);
            var entityEntry = context.Users.Update(entity);
            await context.SaveChangesAsync();
            logger.Log(LogLevel.Trace, "updated entity id: {}", entityEntry.Entity.IdUser);
            return entityEntry.Entity;
        }
        catch (Exception ex)
        {
            throw RepositoryException.UpdateError(ex);
        }
    }

    public async Task<User> DeleteAsync(long id)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var entityAsOption = await FindByIdAsync(id);
            var entity = entityAsOption.IfNone(() => throw new InvalidOperationException("El campo IdUser no corresponde a ningún registro."));
            var entityEntry = context.Users.Remove(entity);
            await context.SaveChangesAsync();
            logger.Log(LogLevel.Trace, "deleted entity id: {}", entityEntry.Entity.IdUser);
            return entityEntry.Entity;
        }
        catch (Exception ex)
        {
            throw RepositoryException.DeleteError(ex);
        }
    }

    public async Task<int> CountAsync(UserSearch? search)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            IQueryable<User> query = context.Users;

            if (search is { IdUser: not null })
            {
                query = query.Where(x => x.IdUser == search.IdUser);
            }

            var totalItems = await query.CountAsync();
            logger.Log(LogLevel.Trace, "counted entities {}", totalItems);
            return totalItems;
        }
        catch (Exception ex)
        {
            throw RepositoryException.ReadError(ex);
        }
    }

    private static IOrderedQueryable<User> DefaultOrderBy(IQueryable<User> q)
    {
        return q.OrderBy(e => e.IdUser);
    }
}