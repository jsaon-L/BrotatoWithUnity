using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using System.IO;
using System;
using UnityGameFramework.Runtime;

public class SQLiteComponent : GameFrameworkComponent
{
    public const string DefaultDBName = "DefaultDB";
    private Dictionary<string, SQLiteConnection> _dbConnections = new Dictionary<string, SQLiteConnection>();

    //Windows 建议只用同步api加快开发速度
    //移动端或者to c项目应该用异步api防止卡顿，优化优先

    public void InitDBTables(List<Type> tableTypes,string dbName = DefaultDBName)
    {
        var db = GetOrCreateDB(dbName);

        if (tableTypes == null|| tableTypes.Count <= 0)
        {
            return;
        }

        foreach (var tableType in tableTypes)
        {
            if (tableType!=null)
            {
                db.CreateTable(tableType);
            }
        }
    }
    public void InitDBTables(string dbName = DefaultDBName)
    {
        List<Type> tableTypes = new List<Type>();

        foreach (var item in GameFramework.Utility.Assembly.GetTypes())
        {
            if (item.IsDefined(typeof(SQLite.TableAttribute), false))
            {
                tableTypes.Add(item);
            }
        }

        InitDBTables(tableTypes, dbName);
    }

    public SQLiteConnection OpenDB(string dbName = DefaultDBName)
    {
        if (_dbConnections.ContainsKey(dbName))
        {
            return _dbConnections[dbName];
        }

        string dbFullName = GetDBFullName(dbName);
        string dbDirName = Path.GetDirectoryName(dbFullName);
        if (!Directory.Exists(dbDirName))
        {
            Directory.CreateDirectory(dbDirName);
        }
        var db = new SQLiteConnection(dbFullName);
        _dbConnections.Add(dbName, db);

        return db;
    }
    public void CloseDB(string dbName = DefaultDBName)
    {
        if (_dbConnections.ContainsKey(dbName))
        {
            var db = _dbConnections[dbName];
            _dbConnections.Remove(dbName);
            db.Close();
        }
    }
    public void CloseAllDB()
    {
        foreach (var db in _dbConnections)
        {
            db.Value.Close();
        }
        _dbConnections.Clear();
    }
   

    public string GetDBFullName(string dbName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
string dbPath = Path.Combine(Application.persistentDataPath, dbName);
#else
        string dbPath = Path.Combine(Application.streamingAssetsPath,"DB", dbName+".db");
#endif
        return dbPath;
    }
    
    public SQLiteConnection GetOrCreateDB(string dbName = DefaultDBName)
    {
        return OpenDB(dbName);
    }



    private void OnDestroy()
    {
        CloseAllDB();
    }



    //private void Test()
    //{
    //    var db = OpenDB("Test.db");

    //     db.CreateTable<Stock>();
    //     db.CreateTable<Valuation>();

    //    var stock = new Stock() {Id = 1, Symbol = "苹果" };
    //    db.InsertOrReplace(stock);
    //    Debug.Log(stock.Id + "  " + stock.Symbol);

    //    //var stock2 = new Stock() { Symbol = "香蕉" };
    //    // db.InsertOrReplace(stock2);
    //    //Debug.Log(stock2.Id + "  " + stock2.Symbol);


    //    //var queryList =  db.Table<Stock>()
    //    //    .Where(v => v.Symbol.StartsWith("q"))
    //    //    .ToList();
    //    foreach (var query in db.Table<Stock>())
    //    {
    //        Debug.Log(query.Id + "--" + query.Symbol);
    //    }
    //}


}
