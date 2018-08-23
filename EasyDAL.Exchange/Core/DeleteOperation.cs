﻿using EasyDAL.Exchange.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using EasyDAL.Exchange.AdoNet;

namespace EasyDAL.Exchange.Core
{
    public class DeleteOperation<M> : DbOperation
    {
        public DeleteOperation(IDbConnection conn)
            : base(conn)
        {
        }

        public DeleteOperation<M> Where<T>(Expression<Func<M, T>> func)
        {
            var field = EH.GetFieldName(func);
            Conditions.Add(field);
            return this;
        }

        public async Task<int> DeleteAsync(M m)
        {

            TryGetTableName(m, out var tableName);

            if (!Conditions.Any())
            {
                throw new Exception("没有设置任何删除条件!");
            }
            var whereFields = " where " + string.Join(" and ", Conditions.Select(p => $"`{p}`=@{p}"));
            var sql = $" delete from `{tableName}`{whereFields} ; ";

            return await SqlMapper.ExecuteAsync(Conn, sql, m);

        }

    }
}
