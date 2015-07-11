using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PlantManager_WPF
{
    public class SunLevel
    {
        public SunLevel(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public string Name
        {
            get { return GetSunLevelName(); }
        }

        private string GetSunLevelName()
        {
            if (Id == -1)
            {
                return Constants.StrNone;
            }

            DataRow sunLevel = Db.QueryFirst("SELECT SunLevelName FROM SunLevels WHERE SunLevelID = ?",
                Id.ToString());
            return sunLevel["SunLevelName"].ToString();
        }

        public static SunLevel[] GetAllSunLevels()
        {
            DataTable dtSunLevels = Db.Query("SELECT * FROM SunLevels");
            List<SunLevel> lstSunLevels = new List<SunLevel>();

            lstSunLevels.Add(GetDefaultSunLevel());

            lstSunLevels.AddRange(from DataRow row in dtSunLevels.Rows
                select new SunLevel(Convert.ToInt32(row["SunLevelID"])));
            return lstSunLevels.ToArray();
        }

        public static void AddSunLevel(string name)
        {
            Db.Execute("INSERT INTO SunLevels (SunLevelName) VALUES (?)", name);
        }

        public static SunLevel GetDefaultSunLevel()
        {
            return new SunLevel(-1);
        }

        public override string ToString()
        {
            return Name;
        }

        public static SunLevel GetSunLevelByPlantId(int plantId)
        {
            DataRow item = Db.QueryFirst(
                "SELECT * FROM Plants INNER JOIN SunLevels On PlantSunLevelID = SunLevelID WHERE PlantID = ?",
                plantId.ToString());

            return item == null
                ? GetDefaultSunLevel()
                : new SunLevel(Convert.ToInt32(item["SunLevelID"]));
        }

        public static void DeleteSunLevelById(int id)
        {
            if (id != -1)
                Db.Execute("DELETE FROM SunLevels WHERE SunLevelID = ?", id.ToString());
        }
    }
}