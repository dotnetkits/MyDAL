﻿using MyDAL.Core.Bases;
using MyDAL.Core.Enums;
using MyDAL.Core.Extensions;
using MyDAL.Impls.Base;
using MyDAL.Interfaces.IAsyncs;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyDAL.Impls.ImplAsyncs
{
    internal sealed class QueryOneAsyncImpl<M>
         : Impler
         , IQueryOneAsync<M>
     where M : class
    {
        internal QueryOneAsyncImpl(Context dc)
            : base(dc)
        { }

        public async Task<M> QueryOneAsync()
        {
            return (await new TopAsyncImpl<M>(DC).TopAsync(1)).FirstOrDefault();
        }
        public async Task<VM> QueryOneAsync<VM>()
            where VM : class
        {
            return (await new TopAsyncImpl<M>(DC).TopAsync<VM>(1)).FirstOrDefault();
        }
        public async Task<T> QueryOneAsync<T>(Expression<Func<M, T>> columnMapFunc)
        {
            return (await new TopAsyncImpl<M>(DC).TopAsync(1, columnMapFunc)).FirstOrDefault();
        }

    }

    internal sealed class QueryOneXAsyncImpl
         : Impler
         , IQueryOneXAsync
    {
        internal QueryOneXAsyncImpl(Context dc)
            : base(dc)
        { }

        public async Task<M> QueryOneAsync<M>()
            where M : class
        {
            return (await new TopXAsyncImpl(DC).TopAsync<M>(1)).FirstOrDefault();
        }
        public async Task<T> QueryOneAsync<T>(Expression<Func<T>> columnMapFunc)
        {
            return (await new TopXAsyncImpl(DC).TopAsync(1, columnMapFunc)).FirstOrDefault();
        }

    }

    internal sealed class QueryOneSQLAsyncImpl
        : ImplerAsync
        , IQueryOneSQLAsync
    {
        public QueryOneSQLAsyncImpl(Context dc)
            : base(dc)
        { }

        public async Task<T> QueryOneAsync<T>()
        {
            DC.Method = UiMethodEnum.QueryOneAsync;
            if (typeof(T).IsSingleColumn())
            {
                return (await DSA.ExecuteReaderSingleColumnAsync<T>()).FirstOrDefault();
            }
            else
            {
                return (await DSA.ExecuteReaderMultiRowAsync<T>()).FirstOrDefault();
            }
        }

    }
}
