﻿using MyDAL.Test.Entities.EasyDal_Exchange;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Yunyong.DataExchange;

namespace MyDAL.Test.JoinQueryM
{
    public class _06_TopAsync:TestBase
    {
        [Fact]
        public async Task test()
        {

            /*******************************************************************************************************************************/

            var xx8 = "";

            var res8 = await Conn
                .Joiner<Agent, AgentInventoryRecord>(out var agent8, out var record8)
                .From(() => agent8)
                    .InnerJoin(() => record8)
                        .On(() => agent8.Id == record8.AgentId)
                .Where(() => record8.CreatedOn >= WhereTest.CreatedOn)
                .TopAsync<Agent>(25);
            Assert.True(res8.Count == 25);

            var tuple8 = (XDebug.SQL, XDebug.Parameters);

            /*******************************************************************************************************************************/

            var xx10 = "";

            var res10 = await Conn
                .Joiner<Agent, AgentInventoryRecord>(out var agent10, out var record10)
                .From(() => agent10)
                    .InnerJoin(() => record10)
                        .On(() => agent10.Id == record10.AgentId)
                .Where(() => record10.CreatedOn >= WhereTest.CreatedOn)
                .ListAsync<Agent>(25);
            Assert.True(res10.Count == 25);

            var tuple10 = (XDebug.SQL, XDebug.Parameters, XDebug.SqlWithParams);

        }
    }
}