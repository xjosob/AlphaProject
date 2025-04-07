﻿using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories
{
    public interface IClientRepository : IBaseRepository<ClientEntity, Client> { }

    public class ClientRepository(AppDbContext context)
        : BaseRepository<ClientEntity, Client>(context),
            IClientRepository { }
}
