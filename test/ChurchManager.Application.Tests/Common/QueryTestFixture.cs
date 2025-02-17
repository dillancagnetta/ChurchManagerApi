﻿using System;
using AutoMapper;
using ChurchManager.Features.People.Infrastructure.Mapper;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Xunit;

namespace ChurchManager.Application.Tests.Common
{
    public class QueryTestFixture : IDisposable
    {
        public ChurchManagerDbContext Context { get; private set; }
        public IMapper Mapper { get; private set; }

        public QueryTestFixture()
        {
            Context = DbContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PeopleMappingProfile>();
            });

            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            DbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
