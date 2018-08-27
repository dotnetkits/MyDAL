﻿using EasyDAL.Exchange.Attributes;
using EasyDAL.Exchange.Common;
using EasyDAL.Exchange.Enums;
using EasyDAL.Exchange.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EasyDAL.Exchange.Extensions;
using EasyDAL.Exchange.DynamicParameter;
using EasyDAL.Exchange.Core.Sql;

namespace EasyDAL.Exchange.Base
{
    public abstract partial class DbOperation
    {
        protected AttributeHelper AH { get; private set; }

        protected GenericHelper GH { get; private set; }

        protected ExpressionHelper EH { get; private set; }

        protected DbContext DC { get; private set; }

        protected List<string> GetProperties<M>(M m)
        {
            if (m == null)
            {
                return new List<string>();
            }

            if (m is ExpandoObject)
            {
                return ((IDictionary<string, object>)m).Keys.ToList();
            }


            var props = default(List<PropertyInfo>);
            if (!DC.ModelPropertiesCache.TryGetValue(m.GetType(), out props))
            {
                props = GH.GetPropertyInfos(m);
                DC.ModelPropertiesCache[m.GetType()] = props;
            }

            return props.Select(x => x.Name).ToList();
        }

        protected string GetWheres()
        {
            if (!DC.Conditions.Any(it => it.Action == ActionEnum.Where)
                && !DC.Conditions.Any(it => it.Action == ActionEnum.And)
                && !DC.Conditions.Any(it => it.Action == ActionEnum.Or))
            {
                throw new Exception("没有设置任何条件!");
            }

            var str = string.Empty;

            foreach (var item in DC.Conditions)
            {
                switch (item.Action)
                {
                    case ActionEnum.Where:
                    case ActionEnum.And:
                    case ActionEnum.Or:
                        switch (item.Option)
                        {
                            case OptionEnum.Equal:
                            case OptionEnum.LessThan:
                            case OptionEnum.LessThanOrEqual:
                            case OptionEnum.GreaterThan:
                            case OptionEnum.GreaterThanOrEqual:
                                str += $" {item.Action.ToEnumDesc<ActionEnum>()} `{item.key}`{item.Option.ToEnumDesc<OptionEnum>()}@{item.key} ";
                                break;
                            case OptionEnum.Like:
                                str += $" {item.Action.ToEnumDesc<ActionEnum>()} `{item.key}`{item.Option.ToEnumDesc<OptionEnum>()}CONCAT('%',@{item.key},'%') ";
                                break;
                        }
                        break;
                }
            }

            return str;
        }

        protected string GetUpdates()
        {
            if (!DC.Conditions.Any(it => it.Action == ActionEnum.Set)
                && !DC.Conditions.Any(it => it.Action == ActionEnum.Change))
            {
                throw new Exception("没有设置任何要更新的字段!");
            }

            var list = new List<string>();

            foreach (var item in DC.Conditions)
            {
                switch (item.Action)
                {
                    case ActionEnum.Set:
                    case ActionEnum.Change:
                        switch (item.Option)
                        {
                            case OptionEnum.ChangeAdd:
                            case OptionEnum.ChangeMinus:
                                list.Add($" `{item.key}`=`{item.key}`{item.Option.ToEnumDesc<OptionEnum>()}@{item.key} ");
                                break;
                            case OptionEnum.Set:
                                list.Add($" `{item.key}`{item.Option.ToEnumDesc<OptionEnum>()}@{item.key} ");
                                break;
                        }
                        break;
                }
            }

            return string.Join(",", list);
        }

        protected bool TryGetTableName<M>(M m, out string tableName)
        {

            tableName = AH.GetPropertyValue<M, TableAttribute>(m, a => a.Name);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("DB Entity 缺少 TableAttribute 指定的表名!");
            }

            return true;

        }
        protected bool TryGetTableName<M>(out string tableName)
        {
            tableName = AH.GetPropertyValue<M, TableAttribute>(Activator.CreateInstance<M>(), a => a.Name);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("DB Entity 缺少 TableAttribute 指定的表名!");
            }

            return true;

        }

        protected OptionEnum GetChangeOption(ChangeEnum change)
        {
            switch (change)
            {
                case ChangeEnum.Add:
                    return OptionEnum.ChangeAdd;
                case ChangeEnum.Minus:
                    return OptionEnum.ChangeMinus;
                default:
                    return OptionEnum.ChangeAdd;
            }
        }

        protected DynamicParameters GetParameters()
        {
            var paras = new DynamicParameters();
            foreach (var item in DC.Conditions)
            {
                switch (item.Action)
                {
                    case ActionEnum.Set:
                    case ActionEnum.Change:
                    case ActionEnum.Where:
                    case ActionEnum.And:
                    case ActionEnum.Or:
                        paras.Add(item.key, item.Value);
                        break;
                }
            }
            return paras;
        }
    }
}
