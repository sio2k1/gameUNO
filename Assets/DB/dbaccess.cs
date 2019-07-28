using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

static class MyDataBase
{
    private const string fileName = "game.bytes";
    private static string DBPath;
    private static SqliteConnection connection;
    private static SqliteCommand command;

    static MyDataBase()
    {
        DBPath = GetDatabasePath();
    }

    /// <summary> Depending of platform, return patch to db. For android we copy db to app path so it can be modified. </summary>
    private static string GetDatabasePath()
    {
        //Debug.Log(Path.Combine(Application.streamingAssetsPath, fileName));
#if UNITY_EDITOR
        return Path.Combine(Application.streamingAssetsPath, fileName);
#endif

#if UNITY_ANDROID
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(filePath)) UnpackDatabase(filePath);
        UnpackDatabase(filePath);
        return filePath;
#endif

    }

    /// <summary> Распаковывает базу данных в указанный путь. </summary>
    /// <param name="toPath"> Путь в который нужно распаковать базу данных. </param>
    private static void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        WWW reader = new WWW(fromPath);
        while (!reader.isDone) { }

        File.WriteAllBytes(toPath, reader.bytes);
    }

    public static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }

    public static DataTable GetTable(string query)
    {
        OpenConnection();

        SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

        DataSet DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    }

    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }
}

public class dbaccess : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(MyDataBase.GetTable("SELECT * from questions").Columns[1].ColumnName);
        //Debug.Log(Path.Combine(Application.dataPath, "DB"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
