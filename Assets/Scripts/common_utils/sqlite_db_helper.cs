using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

//this script is used for sqllite connection in editor and android

namespace cmn_infrastructure
{
    static class sqlite_db_helper
    {
        private const string fileName = "game.bytes"; //define file name
        private static string DBPath;
        private static SqliteConnection connection;
        private static SqliteCommand command;

        static sqlite_db_helper() // get db path in constructor
        {
            DBPath = GetDatabasePath();
        }

        private static string GetDatabasePath() //Depending of platform, return patch to db. For android we copy db to app path so it can be modified.
        {
            //Debug.Log(Path.Combine(Application.streamingAssetsPath, fileName));
#if UNITY_EDITOR
            return Path.Combine(Application.streamingAssetsPath, fileName);
#endif

#if UNITY_ANDROID
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(filePath)) UnpackDatabase(filePath);
            UnpackDatabase(filePath); //we perform overwrite on each run due to debug reasons
            return filePath;
#endif
        }

        private static void UnpackDatabase(string toPath) //unpack db to local folder at android
        {
            string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);
            WWW reader = new WWW(fromPath);
            while (!reader.isDone) { }
            File.WriteAllBytes(toPath, reader.bytes);
        }

        public static void OpenConnection() // open connection to DB
        {
            connection = new SqliteConnection("Data Source=" + DBPath);
            command = new SqliteCommand(connection);
            connection.Open();
        }

        public static DataTable GetTable(string query) // return first table on select statment
        {
            OpenConnection();
            SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);
            DataSet DS = new DataSet();
            adapter.Fill(DS);
            adapter.Dispose();
            CloseConnection();
            return DS.Tables[0];
        }

        public static void ExecuteQueryWithoutAnswer(string query) // execute insert\delete\update
        {
            OpenConnection();
            command.CommandText = query;
            command.ExecuteNonQuery();
            CloseConnection();
        }

        public static void CloseConnection() //close connection to db
        {
            connection.Close();
            command.Dispose();
        }
    }
}


