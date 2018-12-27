﻿using MyDAL.Test.Enums;
using System;
using System.Collections.Generic;

namespace MyDAL.Test.Options
{
    public class AgentQueryOption : PagingQueryOption
    {
        public Guid? Id { get; set; }

        [XQuery(Name = "Name", Compare = CompareEnum.Like)]
        public string Name { get; set; }

        [XQuery(Name = "CreatedOn", Compare = CompareEnum.GreaterThanOrEqual)]
        public DateTime StartTime { get; set; }

        [XQuery(Name = "CreatedOn", Compare = CompareEnum.LessThanOrEqual)]
        public DateTime EndTime { get; set; }

        public AgentLevel AgentLevel { get; set; }

        [XQuery(Name = "AgentLevel", Compare = CompareEnum.In)]
        public List<AgentLevel> EnumListIn { get; set; }

        [XQuery(Name = "AgentLevel", Compare = CompareEnum.NotIn)]
        public List<AgentLevel> EnumListNotIn { get; set; }
    }
}
