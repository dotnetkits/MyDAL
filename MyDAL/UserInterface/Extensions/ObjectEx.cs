﻿using MyDAL.Core.Common;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyDAL.ModelTools
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectEx
    {

        /// <summary>
        /// 深度复制 [[只要你能点出来这个方法,就可以对该对象进行深度复制~~]]
        /// </summary>
        public static T DeepClone<T>(this T obj)
            where T : class
        {
            var result = default(T);
            try
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.SurrogateSelector = new SurrogateSelector();
                formatter.SurrogateSelector.ChainSelector(new NonSerialiazableTypeSurrogateSelector());
                var ms = new MemoryStream();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                result = (T)formatter.Deserialize(ms);
            }
            catch (Exception ex)
            {
                throw new Exception($"不支持类型【{typeof(T).FullName}】的深度复制!!!", ex);
            }
            return result;
        }

    }
}
