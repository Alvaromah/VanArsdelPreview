﻿using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace VanArsdel.Data.Services
{
    public interface IDataSource : IDisposable
    {
        DbSet<CountryCode> CountryCodes { get; }
        DbSet<PaymentType> PaymentTypes { get; }
        DbSet<TaxType> TaxTypes { get; }
        DbSet<OrderStatus> OrderStatus { get; }
        DbSet<Shipper> Shippers { get; }

        DbSet<Customer> Customers { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<Product> Products { get; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
