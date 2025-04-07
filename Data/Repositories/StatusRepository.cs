﻿using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories
{
    public interface IStatusRepository : IBaseRepository<StatusEntity, Status> { }

    public class StatusRepository(AppDbContext context)
        : BaseRepository<StatusEntity, Status>(context),
            IStatusRepository { }
}
