using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils.Calendar.Populate;

public static class ColorUtil
{
    internal static void AddBelongsToBasedOnColor2(string color2, DateTime d)
    {
        string query;
        switch (color2)
        {
            case "FFFF0000": //festività
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 1);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFC5DFB3": //esame di profitto
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 3);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFAC75D4": //laurea Magistrale
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 4);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case null: //lezione
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 5);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFD9D9D9": //sabato
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 6);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFFFFF00": //laureee di 1 livello
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 7);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFFFC000": //vacanze
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 8);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFBCD5ED": //altre attività
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 9);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFFAE3D4": //prove in itinere
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 10);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
        }
    }

    internal static void AddBelongsToBasedOnColor(string color, DateTime d)
    {
        string query;
        switch (color)
        {
            case "FFFF0000": //festività
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 1);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFC5DFB3": //esame di profitto
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 3);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFAC75D4":
            case "FF9E5ECE": //laurea Magistrale
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 4);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case null: //lezione
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 5);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFD9D9D9": //sabato
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 6);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFFFFF00": //laureee di 1 livello
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 7);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFFFC000": //vacanze
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 8);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFBCD5ED": //altre attività
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 9);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "FFFAE3D4": //prove in itinere
            {
                query = "INSERT IGNORE INTO belongsTo VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 10);";
                PoliNetwork.Db.Utils.Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
        }
    }
}