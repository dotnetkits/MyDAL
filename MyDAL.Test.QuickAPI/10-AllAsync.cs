﻿using MyDAL.Test.Entities.EasyDal_Exchange;
using MyDAL.Test.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyDAL.Test.QuickAPI
{
    public class _10_AllAsync:TestBase
    {
        [Fact]
        public async Task test()
        {
            var xx1 = "";

            var res1 = await Conn.AllAsync<Agent>();
            Assert.True(res1.Count == 28620);

            var tuple1 = (XDebug.SQL, XDebug.Parameters, XDebug.SqlWithParams);

            /***************************************************************************************************************************/

            var xx2 = "";

            var res2 = await Conn.AllAsync<Agent,AgentVM>();
            Assert.True(res2.Count == 28620);

            var tuple2 = (XDebug.SQL, XDebug.Parameters, XDebug.SqlWithParams);

            /***************************************************************************************************************************/

            var xx3 = "";

            var res3 = await Conn.AllAsync<Agent, Guid>(it => it.Id);
            Assert.True(res3.Count == 28620);

            var tuple3 = (XDebug.SQL, XDebug.Parameters, XDebug.SqlWithParams);

            /***************************************************************************************************************************/

            var xx4 = "";

            var res4 = await Conn.AllAsync<Agent, AgentVM>(it => new AgentVM
            {
                XXXX=it.Name,
                YYYY=it.PathId
            });
            Assert.True(res4.Count == 28620);

            var tuple4 = (XDebug.SQL, XDebug.Parameters, XDebug.SqlWithParams);

            /***************************************************************************************************************************/

            var xx = "";
        }
    }
}
