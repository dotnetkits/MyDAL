﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yunyong.DataExchange.Core.Bases;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.Interfaces;

namespace Yunyong.DataExchange.Impls
{
    internal class TopImpl<M>
        : Impler, ITop<M>
        where M:class
    {
        internal TopImpl(Context dc) 
            : base(dc)
        {
        }

        public async Task<List<M>> TopAsync(int count)
        {
            return (await DC.DS.ExecuteReaderMultiRowAsync<M>(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(UiMethodEnum.TopAsync,0,count)[0],
                DC.SqlProvider.GetParameters())).ToList();
        }

        public async Task<List<VM>> TopAsync<VM>(int count) 
            where VM : class
        {
            SelectMHandle<M, VM>();
            DC.DH.UiToDbCopy();
            return (await DC.DS.ExecuteReaderMultiRowAsync<VM>(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(UiMethodEnum.TopAsync,0,count)[0],
                DC.SqlProvider.GetParameters())).ToList();
        }

        public async Task<List<VM>> TopAsync<VM>(int count,Expression<Func<M, VM>> columnMapFunc) 
            where VM : class
        {
            SelectMHandle(columnMapFunc);
            DC.DH.UiToDbCopy();
            return (await DC.DS.ExecuteReaderMultiRowAsync<VM>(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(UiMethodEnum.TopAsync,0,count)[0],
                DC.SqlProvider.GetParameters())).ToList();
        }
    }
}
