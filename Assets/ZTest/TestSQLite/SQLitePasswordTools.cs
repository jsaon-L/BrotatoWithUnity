using SQLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;
using System.Security.Cryptography;
using System.Text;

public class SQLitePasswordTools : MonoBehaviour
{


    public string AccountText;
    public string PasswordText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var c = CreateAccount(AccountText, PasswordText);
            Debug.Log("创建账号："+c);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            var c = Login(AccountText, PasswordText);
            Debug.Log("登录："+c);
        }
    }


    public bool CreateAccount(string account, string password)
    {
        GameEntry.SQLite.InitDBTables();

        if (HasAccount(account))
        {
            Debug.LogError("账户已存在：" + account);
            return false;
        }
        var db = GameEntry.SQLite.GetOrCreateDB();
        db.CreateTable<UserTable>();
        string salt = Guid.NewGuid().ToString("N");
        UserTable userTable = new UserTable();
        userTable.Account = account;
        userTable.Password = CreateMD5(password + salt);
        userTable.Salt = salt;


        db.Insert(userTable);
        return true;
    }

    public bool Login(string account, string password)
    {
        if (!HasAccount(account))
        {
            Debug.LogError("账户不存在：" + account);
            return false;
        }
        var db = GameEntry.SQLite.GetOrCreateDB();

        var table = db.Table<UserTable>();
        var userData = table.Where(x => x.Account == account).First();

        return userData.Password == CreateMD5(password + userData.Salt);
    }

    public bool HasAccount(string account)
    {
        var db = GameEntry.SQLite.GetOrCreateDB();
        var query = db.Table<UserTable>()
            .Where(x => x.Account == account);

        return query.Count() > 0;
    }

    public static string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            //return Convert.ToHexString(hashBytes); // .NET 5 +

            // Convert the byte array to hexadecimal string prior to .NET 5
            StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    [SQLite.Table("UserData")]
    public class UserTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [SQLite.MaxLength(32)]
        public string Account { get; set; }
        [SQLite.MaxLength(32)]
        public string Password { get; set; }
        [SQLite.MaxLength(32)]
        public string Salt { get; set; }
    }

}
