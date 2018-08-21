﻿using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EasyDAL.Exchange.DynamicParameter;
using Xunit;

namespace EasyDAL.Exchange.Tests
{
    public class ProcedureTests : TestBase
    {
        [Fact]
        public void TestProcWithOutParameter()
        {
            connection.Execute(
            @"CREATE PROCEDURE #TestProcWithOutParameter
                @ID int output,
                @Foo varchar(100),
                @Bar int
            AS
                SET @ID = @Bar + LEN(@Foo)");
            var obj = new
            {
                ID = 0,
                Foo = "abc",
                Bar = 4
            };
            var args = new DynamicParameters(obj);
            args.Add("ID", 0, direction: ParameterDirection.Output);
            connection.Execute("#TestProcWithOutParameter", args, commandType: CommandType.StoredProcedure);
            Assert.Equal(7, args.Get<int>("ID"));
        }

        [Fact]
        public void TestProcWithOutAndReturnParameter()
        {
            connection.Execute(
            @"CREATE PROCEDURE #TestProcWithOutAndReturnParameter
                @ID int output,
                @Foo varchar(100),
                @Bar int
            AS
                SET @ID = @Bar + LEN(@Foo)
                RETURN 42");
            var obj = new
            {
                ID = 0,
                Foo = "abc",
                Bar = 4
            };
            var args = new DynamicParameters(obj);
            args.Add("ID", 0, direction: ParameterDirection.Output);
            args.Add("result", 0, direction: ParameterDirection.ReturnValue);
            connection.Execute("#TestProcWithOutAndReturnParameter", args, commandType: CommandType.StoredProcedure);
            Assert.Equal(7, args.Get<int>("ID"));
            Assert.Equal(42, args.Get<int>("result"));
        }


        [Fact]
        public void SO24605346_ProcsAndStrings()
        {
            connection.Execute(
            @"create proc #GetPracticeRebateOrderByInvoiceNumber 
                @TaxInvoiceNumber nvarchar(20) 
            as
                select @TaxInvoiceNumber as [fTaxInvoiceNumber]");
            const string InvoiceNumber = "INV0000000028PPN";
            var result = connection.Query<PracticeRebateOrders>("#GetPracticeRebateOrderByInvoiceNumber", new
            {
                TaxInvoiceNumber = InvoiceNumber
            }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            Assert.Equal("INV0000000028PPN", result.TaxInvoiceNumber);
        }

        private class PracticeRebateOrders
        {
            public string fTaxInvoiceNumber;
#if !NETCOREAPP1_0
            [System.Xml.Serialization.XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
#endif
            public string TaxInvoiceNumber
            {
                get { return fTaxInvoiceNumber; }
                set { fTaxInvoiceNumber = value; }
            }
        }
        
        private class Issue327_Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class Issue327_Magic
        {
            public string Creature { get; set; }
            public string SpiritAnimal { get; set; }
            public string Location { get; set; }
        }

        [Fact]
        public void TestProcSupport()
        {
            var p = new DynamicParameters();
            p.Add("a", 11);
            p.Add("b", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            connection.Execute(@"
            create proc #TestProc 
	            @a int,
	            @b int output
            as 
            begin
	            set @b = 999
	            select 1111
	            return @a
            end");
            Assert.Equal(1111, connection.Query<int>("#TestProc", p, commandType: CommandType.StoredProcedure).First());

            Assert.Equal(11, p.Get<int>("c"));
            Assert.Equal(999, p.Get<int>("b"));
        }

        // https://stackoverflow.com/q/8593871
        [Fact]
        public void TestListOfAnsiStrings()
        {
            var results = connection.Query<string>("select * from (select 'a' str union select 'b' union select 'c') X where str in @strings",
                new
                {
                    strings = new[] {
                    new DbString { IsAnsi = true, Value = "a" },
                    new DbString { IsAnsi = true, Value = "b" }
                }
                }).ToList();

            Assert.Equal(2, results.Count);
            results.Sort();
            Assert.Equal("a", results[0]);
            Assert.Equal("b", results[1]);
        }

        [Fact]
        public void TestDateTime2PrecisionPreservedInDynamicParameters()
        {
            const string tempSPName = "#" + nameof(TestDateTime2PrecisionPreservedInDynamicParameters);

            DateTime datetimeDefault = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime datetime2 = datetimeDefault.AddTicks(1); // Add 100 ns

            Assert.True(datetimeDefault < datetime2);

            connection.Execute(
            $@"create proc {tempSPName} 
	            @a datetime2,
	            @b datetime2 output
            as 
            begin
	            set @b = @a
	            select DATEADD(ns, -100, @b)
            end");

            var p = new DynamicParameters();
            // Note: parameters declared as DateTime2
            p.Add("a", datetime2, dbType: DbType.DateTime2, direction: ParameterDirection.Input);
            p.Add("b", dbType: DbType.DateTime2, direction: ParameterDirection.Output);

            DateTime fromSelect = connection.Query<DateTime>(tempSPName, p, commandType: CommandType.StoredProcedure).First();

            Assert.Equal(datetimeDefault, fromSelect);

            Assert.Equal(datetime2, p.Get<DateTime>("b"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(DbType.DateTime)]
        public void TestDateTime2LosePrecisionInDynamicParameters(DbType? dbType)
        {
            const string tempSPName = "#" + nameof(TestDateTime2LosePrecisionInDynamicParameters);

            DateTime datetimeDefault = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime datetime2 = datetimeDefault.AddTicks(1); // Add 100 ns

            Assert.True(datetimeDefault < datetime2);

            connection.Execute(
            $@"create proc {tempSPName}
	            @a datetime2,
	            @b datetime2 output
            as 
            begin
	            set @b = DATEADD(ns, 100, @a)
	            select @b
            end");

            var p = new DynamicParameters();
            // Note: input parameter declared as DateTime (or implicitly as this) but SP has DateTime2
            p.Add("a", datetime2, dbType: dbType, direction: ParameterDirection.Input);
            p.Add("b", dbType: DbType.DateTime, direction: ParameterDirection.Output);

            DateTime fromSelect = connection.Query<DateTime>(tempSPName, p, commandType: CommandType.StoredProcedure).First();

            // @a truncates to datetimeDefault when passed into SP by DynamicParameters, add 100ns and it comes out as DateTime2
            Assert.Equal(datetime2, fromSelect);

            // @b gets set to datetime2 value but is truncated back to DbType.DateTime by DynamicParameter's Output declaration
            Assert.Equal(datetimeDefault, p.Get<DateTime>("b"));
        }


  
    }
}