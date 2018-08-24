﻿using EasyDAL.Exchange.AdoNet;
using EasyDAL.Exchange.Base;
using EasyDAL.Exchange.DynamicParameter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EasyDAL.Exchange.Core
{
    public class SelectOperation<M> : DbOperation
    {
        public SelectOperation(IDbConnection conn)
            : base(conn)
        {
        }

        public SelectOperation<M> Where(Expression<Func<M, bool>> func)
        {
            var field = EH.GetFieldName(func);
            Conditions.Add(field);
            return this;
        }

        public async Task<M> QueryFirstOrDefaultAsync()
        {
            TryGetTableName<M>(out var tableName);

            if(!Conditions.Any())
            {
                throw new Exception("没有设置任何查询条件!");
            }

            var wherePart = string.Join(" and ", GetWheres());
            var sql = $"SELECT * FROM `{tableName}` WHERE {wherePart} ; ";
            var paras = new DynamicParameters();
            foreach (var item in Conditions)
            {
                paras.Add(item.key, item.Value);
            }

            return await SqlMapper.QueryFirstOrDefaultAsync<M>(Conn, sql, paras);
        }

    }
}