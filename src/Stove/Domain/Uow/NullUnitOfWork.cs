﻿using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Stove.Domain.Uow
{
    /// <summary>
    ///     Null implementation of unit of work.
    ///     It's used if no component registered for <see cref="IUnitOfWork" />.
    ///     This ensures working Stove without a database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class NullUnitOfWork : UnitOfWorkBase
    {
        public NullUnitOfWork(
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter
        ) : base(
            connectionStringResolver,
            defaultOptions,
            filterExecuter)
        {
        }

        public override void SaveChanges()
        {
        }

        public override Task SaveChangesAsync()
        {
            return Task.FromResult(0);
        }

        protected override void BeginUow()
        {
        }

        protected override void CompleteUow()
        {
        }

        protected override Task CompleteUowAsync()
        {
            return Task.FromResult(0);
        }

        protected override void DisposeUow()
        {
        }
    }
}
