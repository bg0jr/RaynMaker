﻿using System;
using System.Data.Entity;
using RaynMaker.Entities.Figures;

namespace RaynMaker.Entities
{
    public interface IAssetsContext
    {
        IDbSet<Currency> Currencies { get; }

        IDbSet<Company> Companies { get; }

        IDbSet<Stock> Stocks { get; }

        IDbSet<Tag> Tags { get; }

        int SaveChanges();
    }
}
