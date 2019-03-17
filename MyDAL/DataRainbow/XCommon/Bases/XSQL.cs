﻿using MyDAL.Core;
using MyDAL.Core.Bases;
using MyDAL.Core.Enums;
using System.Text;

namespace MyDAL.DataRainbow.XCommon.Bases
{
    internal abstract class XSQL
    {
        protected static void Spacing(StringBuilder sb)
        {
            sb.Append(' ');
        }
        protected static void EscapeChar(StringBuilder sb)
        {
            sb.Append('/');
        }
        protected static char EscapeChar()
        {
            return '/';
        }
        protected static void Percent(StringBuilder sb)
        {
            sb.Append('%');
        }
        protected static char Percent()
        {
            return '%';
        }
        protected static void SingleQuote(StringBuilder sb)
        {
            sb.Append('\'');
        }
        protected static void At(StringBuilder sb)
        {
            sb.Append('@');
        }
        internal static void Dot(StringBuilder sb)
        {
            sb.Append('.');
        }
        protected static void Star(StringBuilder sb)
        {
            sb.Append('*');
        }
        protected static void Comma(StringBuilder sb)
        {
            sb.Append(',');
        }
        protected static void CRLF(StringBuilder sb)
        {
            sb.Append("\r\n");
        }
        protected static void Tab(StringBuilder sb)
        {
            sb.Append("\t");
        }
        protected static void LeftRoundBracket(StringBuilder sb)
        {
            sb.Append('(');
        }
        protected static void RightRoundBracket(StringBuilder sb)
        {
            sb.Append(')');
        }
        protected static void Equal(StringBuilder sb)
        {
            sb.Append('=');
        }

        /****************************************************************************************************************/

        protected static void Action(ActionEnum action, StringBuilder sb, Context dc)
        {
            switch (action)
            {
                case ActionEnum.None:
                case ActionEnum.Insert:
                case ActionEnum.Update:
                case ActionEnum.Select:
                case ActionEnum.From:
                case ActionEnum.OrderBy:
                    return;
                case ActionEnum.InnerJoin:
                    Inner(sb); Spacing(sb); Join(sb);
                    return;
                case ActionEnum.LeftJoin:
                    Left(sb); Spacing(sb); Join(sb);
                    return;
                case ActionEnum.On:
                    On(sb);
                    return;
                case ActionEnum.Where:
                    Where(sb);
                    return;
                case ActionEnum.And:
                    Tab(sb); And(sb);
                    return;
                case ActionEnum.Or:
                    Tab(sb); Or(sb);
                    return;
                default:
                    throw dc.Exception(XConfig.EC._014, action.ToString());
            }
        }
        protected static void MultiAction(ActionEnum action, StringBuilder sb, Context dc)
        {
            if (action == ActionEnum.And)
            {
                Spacing(sb); sb.Append("&&"); Spacing(sb);
            }
            else if (action == ActionEnum.Or)
            {
                Spacing(sb); sb.Append("||"); Spacing(sb);
            }
            else
            {
                throw dc.Exception(XConfig.EC._010, action.ToString());
            }
        }
        protected static void Option(OptionEnum option, StringBuilder sb, Context dc)
        {
            switch (option)
            {
                case OptionEnum.None:
                case OptionEnum.Insert:
                case OptionEnum.Column:
                case OptionEnum.ColumnAs:
                case OptionEnum.Compare:
                case OptionEnum.OneEqualOne:
                    return;
                case OptionEnum.Set:
                    sb.Append("=");
                    return;
                case OptionEnum.ChangeAdd:
                    sb.Append("+");
                    return;
                case OptionEnum.ChangeMinus:
                    sb.Append("-");
                    return;
                case OptionEnum.IsNull:
                    sb.Append(" is null ");
                    return;
                case OptionEnum.IsNotNull:
                    sb.Append(" is not null ");
                    return;
                case OptionEnum.Asc:
                    sb.Append(" asc ");
                    return;
                case OptionEnum.Desc:
                    sb.Append(" desc ");
                    return;
                default:
                    throw dc.Exception(XConfig.EC._022, option.ToString());
            }
        }
        protected static void Compare(CompareXEnum compare, StringBuilder sb, Context dc)
        {
            switch (compare)
            {
                case CompareXEnum.None:
                    return;
                case CompareXEnum.Equal:
                    sb.Append("=");
                    return;
                case CompareXEnum.NotEqual:
                    sb.Append("<>");
                    return;
                case CompareXEnum.LessThan:
                    sb.Append("<");
                    return;
                case CompareXEnum.LessThanOrEqual:
                    sb.Append("<=");
                    return;
                case CompareXEnum.GreaterThan:
                    sb.Append(">");
                    return;
                case CompareXEnum.GreaterThanOrEqual:
                    sb.Append(">=");
                    return;
                case CompareXEnum.Like:
                    sb.Append(" like ");
                    return;
                case CompareXEnum.NotLike:
                    sb.Append(" not like ");
                    return;
                case CompareXEnum.In:
                    sb.Append(" in ");
                    return;
                case CompareXEnum.NotIn:
                    sb.Append(" not in ");
                    return;
                default:
                    throw dc.Exception(XConfig.EC._023, compare.ToString());
            }
        }
        protected static void Function(FuncEnum func, StringBuilder sb, Context dc)
        {
            switch (func)
            {
                case FuncEnum.None:
                    return;
                case FuncEnum.CharLength:
                    sb.Append(" char_length");
                    return;
                case FuncEnum.DateFormat:
                    sb.Append(" date_format");
                    return;
                case FuncEnum.Trim:
                    sb.Append(" trim");
                    return;
                case FuncEnum.LTrim:
                    sb.Append(" ltrim");
                    return;
                case FuncEnum.RTrim:
                    sb.Append(" rtrim");
                    return;
                case FuncEnum.Count:
                    sb.Append(" count");
                    return;
                case FuncEnum.Sum:
                    sb.Append(" sum");
                    return;
                default:
                    throw dc.Exception(XConfig.EC._008, func.ToString());
            }
        }

        /****************************************************************************************************************/

        protected static void InsertInto(StringBuilder sb)
        {
            CRLF(sb);
            sb.Append("insert into");
        }
        protected static void Delete(StringBuilder sb)
        {
            sb.Append("delete");
        }
        protected static void Update(StringBuilder sb)
        {
            sb.Append("update");
        }
        protected static void Select(StringBuilder sb)
        {
            sb.Append("select");
        }

        protected static void From(StringBuilder sb)
        {
            CRLF(sb);
            sb.Append("from");
        }
        protected static void Inner(StringBuilder sb)
        {
            sb.Append("inner");
        }
        protected static void Left(StringBuilder sb)
        {
            sb.Append("left");
        }
        protected static void Join(StringBuilder sb)
        {
            sb.Append("join");
        }
        protected static void On(StringBuilder sb)
        {
            sb.Append("on");
        }

        protected static void Where(StringBuilder sb)
        {
            sb.Append("where");
        }
        protected static void And(StringBuilder sb)
        {
            sb.Append("and");
        }
        protected static void Or(StringBuilder sb)
        {
            sb.Append("or");
        }

        protected static void Values(StringBuilder sb)
        {
            CRLF(sb);
            sb.Append("values");
        }
        protected static void Set(StringBuilder sb)
        {
            CRLF(sb);
            sb.Append("set");
        }

        protected static void Distinct(StringBuilder sb)
        {
            Spacing(sb); sb.Append("distinct"); Spacing(sb);
        }
        protected static void As(StringBuilder sb)
        {
            Spacing(sb); sb.Append("as"); Spacing(sb);
        }
        protected static void Escape(StringBuilder sb)
        {
            sb.Append("escape");
        }

    }
}
