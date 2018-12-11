﻿using MyDAL.Test.Entities;
using MyDAL.Test.Enums;
using MyDAL.Test.TestModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyDAL.Test
{
    public abstract class TestBase
    {
        protected WhereTestModel WhereTest
        {
            get
            {
                return new WhereTestModel
                {
                    CreatedOn = Convert.ToDateTime("2018-08-23 13:36:58").AddDays(-30),
                    StartTime = Convert.ToDateTime("2018-08-23 13:36:58").AddDays(-30),
                    EndTime = DateTime.Now,
                    AgentLevelXX = AgentLevel.DistiAgent,
                    AgentLevelNull = null,
                    ContainStr = "~00-d-3-1-",
                    ContainStr2= "~00-d-3-",
                    In_List_枚举 = new List<AgentLevel?>
                    {
                        AgentLevel.CityAgent,
                        AgentLevel.DistiAgent
                    },
                    In_Array_枚举 = new AgentLevel?[]
                    {
                        AgentLevel.CityAgent,
                        AgentLevel.DistiAgent
                    },
                    In_List_String = new List<string>
                    {
                        "黄银凤",
                        "刘建芬"
                    },
                    In_Array_String = new string[]
                    {
                        "黄银凤",
                        "刘建芬"
                    }
                };
            }
        }

        protected LikeTestModel LikeTest
        {
            get
            {
                return new LikeTestModel
                {
                    无通配符 = "陈",
                    百分号 = "陈%",
                    下划线 = "王_",
                    百分号转义 = "刘/%_",
                    下划线转义 = "何/__"
                };
            }
        }

        protected IDbConnection Conn
        {
            /*
             * CREATE DATABASE `MyDAL_TestDB`;
             */
            get
            {
                return GetOpenConnection("MyDAL_TestDB");
            }
        }
        
        //protected IDbConnection Conn3
        //{
        //    /*
        //     * CREATE DATABASE `EasyDal_Exchange2` 
        //     */
        //    get { return GetOpenConnection3("EasyDal_Exchange2"); }
        //}

        private static IDbConnection GetOpenConnection(string name)
        {
            /*
             * 
            */
            return 
                new MySqlConnection($"Server=localhost; Database={name}; Uid=SkyUser; Pwd=Sky@4321;SslMode=none;")
                .OpenDebug()  // 全局 debug 配置, 生产环境不要开启 
                //.OpenDB()  // 建议 每次新实例并打开,以获得更好的性能体验, 但是 用完要注意手动释放, 防止 连接池 资源耗尽!!!
                ;
        }
        //private static IDbConnection GetOpenConnection3(string name)
        //{
        //    /*
        //     * 
        //    */
        //    var conn =
        //        new MySqlConnection($"Server=localhost; Database={name}; Uid=SkyUser; Pwd=Sky@4321;SslMode=none;")
        //        .OpenCodeFirst("MyDAL.Test.Entities.EasyDal_Exchange")  // 开启 CodeFirst 模式
        //        .OpenDebug()  // 全局 debug 配置, 生产环境不要开启 
        //        .OpenDB();  // 建议 每次新实例并打开,以获得更好的性能体验
        //    return conn;
        //}

    }

}
