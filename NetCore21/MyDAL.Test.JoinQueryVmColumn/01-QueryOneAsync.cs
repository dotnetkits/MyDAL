﻿using MyDAL.Test.Entities.MyDAL_TestDB;
using MyDAL.Test.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyDAL.Test.JoinQueryVmColumn
{
    public class _01_QueryOneAsync
        : TestBase
    {
        [Fact]
        public async Task Test()
        {

            /*************************************************************************************************************************/

            xx = string.Empty;

            var guid2 = Guid.Parse("544b9053-322e-4857-89a0-0165443dcbef");
            var res2 = await Conn
                .Queryer(out Agent agent2, out AgentInventoryRecord record2)
                .From(() => agent2)
                    .InnerJoin(() => record2)
                        .On(() => agent2.Id == record2.AgentId)
                .Where(() => agent2.Id == guid2)
                .QueryOneAsync(() => new AgentVM
                {
                    nn = agent2.PathId,
                    yy = record2.Id,
                    xx = agent2.Id,
                    zz = agent2.Name,
                    mm = record2.LockedCount
                });

            Assert.NotNull(res2);
            Assert.Equal("夏明君", res2.zz);

            tuple = (XDebug.SQL, XDebug.Parameters,XDebug.SqlWithParams);

        }
    }
}