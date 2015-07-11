using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PlantManager_WPF
{
    public class SoilType
    {
        public SoilType(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public string Name
        {
            get { return GetSoilTypeName(); }
        }

        private string GetSoilTypeName()
        {
            if (Id == -1)
            {
                return Constants.StrNone;
            }

            DataRow soilType = Db.QueryFirst("SELECT SoilTypeName FROM SoilTypes WHERE SoilTypeID = ?",
                Id.ToString());
            return soilType["SoilTypeName"].ToString();
        }

        public static SoilType[] GetAllSoilTypes()
        {
            DataTable dtSoilTypes = Db.Query("SELECT * FROM SoilTypes");
            List<SoilType> lstSoilTypes = new List<SoilType>();

            lstSoilTypes.Add(GetDefaultSoilType());

            lstSoilTypes.AddRange(from DataRow row in dtSoilTypes.Rows
                select new SoilType(Convert.ToInt32(row["SoilTypeID"])));
            return lstSoilTypes.ToArray();
        }

        public static void AddSoilType(string name)
        {
            Db.Execute("INSERT INTO SoilTypes (SoilTypeName) VALUES (?)", name);
        }

        public static SoilType GetDefaultSoilType()
        {
            return new SoilType(-1);
        }

        public override string ToString()
        {
            return Name;
        }

        public static SoilType GetSoilTypeByPlantId(int plantId)
        {
            DataRow item = Db.QueryFirst(
                "SELECT * FROM Plants INNER JOIN SoilTypes On PlantSoilTypeID = SoilTypeID WHERE PlantID = ?",
                plantId.ToString());

            return item == null
                ? GetDefaultSoilType()
                : new SoilType(Convert.ToInt32(item["SoilTypeID"]));
        }

        public static void DeleteSoilTypeById(int id)
        {
            if (id != -1)
                Db.Execute("DELETE FROM SoilTypes WHERE SoilTypeID = ?", id.ToString());
        }
    }
}