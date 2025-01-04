using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
namespace GFExample
{

    public class SQLiteExample : MonoBehaviour
    {
        //https://github.com/praeclarum/sqlite-net/wiki/Synchronous-API
        IEnumerator Start()
        {
            //GF也是在Start初始化，防止在GF未初始化为null
            //正常使用逻辑应被GF流程模块调起所以不需要考虑这个问题
            yield return null;

            //打开一个数据库，如果数据库不存在就创建
            //db文件在 Application.streamingAssetsPath/DB 目录;Android例外在Application.persistentDataPath/DB
            var db = GameEntry.SQLite.OpenDB("SQLiteExample");

            //创建表 这一个类就代表了一个表，数据库中表名与类名一致，可以使用特性自定义表名
            db.CreateTable<Stock>();

            //增
            //创建数据，数据类型代表了要在哪个表增加数据
            var stock = new Stock() { Symbol = "香蕉" };
            db.Insert(stock);
            //db.InsertAll();
            //会看能不能找到主键，如果找到了就修改，找不到就插入
            db.InsertOrReplace(stock);
            //删
            db.Delete(stock);
            //改
            db.Update(stock);
            //查
            var stocks = db.Table<Stock>()
                .Where(v => v.Symbol == "香蕉")
                .ToList();

            foreach (var item in stocks)
            {
                Debug.Log(item.Id + " " + item.Symbol);
            }



        }

        
    }


    //库存
    //特性说明 https://github.com/praeclarum/sqlite-net/wiki/Features
    [SQLite.Table(nameof(Stock))]
    public class Stock
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Symbol { get; set; }
    }
    //价值
    [SQLite.Table(nameof(Valuation))]
    public class Valuation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int StockId { get; set; }
        public DateTime Time { get; set; }
        public decimal Price { get; set; }
        [Ignore]
        public string IgnoreField { get; set; }
    }
}