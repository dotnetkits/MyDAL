using MyDAL.Test.Entities.EasyDal_Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Yunyong.DataExchange;

namespace MyDAL.Test.WhereEdge
{
    public class _04_WhereMethodParam:TestBase
    {

        [Fact]
        public async Task MethodParam()
        {
            var res = await Conn
                .Selecter<Agent>()
                .Where(it => it.Id == Guid.Parse("000a9465-8665-40bf-90e3-0165442d9120"))
                .FirstOrDefaultAsync();
            Assert.NotNull(res);

            await xxx(res.Id);
            var id = Guid.Parse("000a9465-8665-40bf-90e3-0165442d9120");
            await xxx(id);
        }
        private async Task xxx(Guid id)
        {
            var xx1 = "";

            // where method parameter 
            var res1 = await Conn
                .Selecter<Agent>()
                .Where(it => it.Id == id)
                .FirstOrDefaultAsync();
            Assert.NotNull(res1);

            var tuple1 = (XDebug.SQL, XDebug.Parameters);

            var xxR1 = "";

            // where method parameter 
            var resR1 = await Conn
                .Selecter<Agent>()
                .Where(it => id == it.Id)
                .FirstOrDefaultAsync();
            Assert.NotNull(resR1);

            var tupleR1 = (XDebug.SQL, XDebug.Parameters);

            Assert.True(res1.Id.Equals(Guid.Parse("000a9465-8665-40bf-90e3-0165442d9120")));
            Assert.True(resR1.Id.Equals(Guid.Parse("000a9465-8665-40bf-90e3-0165442d9120")));

            var xx = "";
        }


        [Fact]
        public async Task MethodListParam()
        {
            var list = new List<Guid>();
            list.Add(Guid.Parse("00079c84-a511-418b-bd5b-0165442eb30a"));
            list.Add(Guid.Parse("000cecd5-56dc-4085-804b-0165443bdf5d"));

            await yyy(list);
            await yyy(list.ToArray());
        }
        private async Task yyy(List<Guid> list)
        {
            var xx = "";

            var res = await Conn
                .Selecter<Agent>()
                .Where(it => list.Contains(it.Id))
                .ListAsync();
            Assert.True(res.Count == 2);

            var xxx = "";
        }
        private async Task yyy(Guid[] arrays)
        {
            var xx = "";

            var res = await Conn
                .Selecter<Agent>()
                .Where(it => arrays.Contains(it.Id))
                .ListAsync();
            Assert.True(res.Count == 2);

            var xxx = "";
        }


    }
}
