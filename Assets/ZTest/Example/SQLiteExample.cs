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
            //GFҲ����Start��ʼ������ֹ��GFδ��ʼ��Ϊnull
            //����ʹ���߼�Ӧ��GF����ģ��������Բ���Ҫ�����������
            yield return null;

            //��һ�����ݿ⣬������ݿⲻ���ھʹ���
            //db�ļ��� Application.streamingAssetsPath/DB Ŀ¼;Android������Application.persistentDataPath/DB
            var db = GameEntry.SQLite.OpenDB("SQLiteExample");

            //������ ��һ����ʹ�����һ�������ݿ��б���������һ�£�����ʹ�������Զ������
            db.CreateTable<Stock>();

            //��
            //�������ݣ��������ʹ�����Ҫ���ĸ�����������
            var stock = new Stock() { Symbol = "�㽶" };
            db.Insert(stock);
            //db.InsertAll();
            //�ῴ�ܲ����ҵ�����������ҵ��˾��޸ģ��Ҳ����Ͳ���
            db.InsertOrReplace(stock);
            //ɾ
            db.Delete(stock);
            //��
            db.Update(stock);
            //��
            var stocks = db.Table<Stock>()
                .Where(v => v.Symbol == "�㽶")
                .ToList();

            foreach (var item in stocks)
            {
                Debug.Log(item.Id + " " + item.Symbol);
            }



        }

        
    }


    //���
    //����˵�� https://github.com/praeclarum/sqlite-net/wiki/Features
    [SQLite.Table(nameof(Stock))]
    public class Stock
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Symbol { get; set; }
    }
    //��ֵ
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