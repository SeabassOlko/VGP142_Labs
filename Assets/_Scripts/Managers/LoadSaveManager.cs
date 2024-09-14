using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;
using System.Text;


public class LoadSaveManager : Singleton<LoadSaveManager>
{
    private const string password = "PotatoSO";

    [XmlRoot("GameData")]
    public class GameStateData
    {
        public struct DataTransform
        {
            public float posX;
            public float posY;
            public float posZ;
            public float rotX;
            public float rotY;
            public float rotZ;
            public float scaleX;
            public float scaleY;
            public float ScaleZ;
        }

        // Data for enemy
        public class DataEnemy
        {
            //Enemy Transform Data
            public DataTransform posRotScale;

            //Enemy ID
            public int enemyID;

            //Health
            public int health;

        }

        // Data for player
        public class DataPlayer
        {
            //checkpoint Data
            public DataTransform checkpointPosRotScale;

            //Has Collected weapons?
            public bool hasSword;
            public bool hasAxe;

            public int health;
            public int lives;
        }

        // Instance variables
        public List<DataEnemy> enemies = new List<DataEnemy>();
        public DataPlayer player = new DataPlayer();
    }

    // Game data to save/load
    public GameStateData gameStateData = new GameStateData();
    
    public void Save(string filename = "GameData.xml")
    {
        // Save game data
        XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
        FileStream stream = new FileStream(filename, FileMode.Create);    

        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        DES.Key = ASCIIEncoding.ASCII.GetBytes(password);
        DES.IV = ASCIIEncoding.ASCII.GetBytes(password);

        ICryptoTransform desencrypt = DES.CreateEncryptor();

        using (CryptoStream cStream = new CryptoStream(stream, desencrypt, CryptoStreamMode.Write))
        {
            serializer.Serialize(cStream, gameStateData);
        }

        stream.Close();
        stream.Dispose();
    }

    public void Load(string filename = "GameData.xml") 
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
        FileStream stream = new FileStream(filename, FileMode.Open);

        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        DES.Key = ASCIIEncoding.ASCII.GetBytes(password);
        DES.IV = ASCIIEncoding.ASCII.GetBytes(password);

        ICryptoTransform desDecrypt = DES.CreateDecryptor();

        using (CryptoStream cStream = new CryptoStream(stream, desDecrypt, CryptoStreamMode.Read))
        {
            gameStateData = serializer.Deserialize(cStream) as GameStateData;
        }

        stream.Close();
        stream.Dispose();
    }
}
