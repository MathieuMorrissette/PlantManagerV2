using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PlantManager_WPF
{
    public class PlantType
    {
        public PlantType(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public string Name
        {
            get { return GetPlantTypeName(); }
        }

        private string GetPlantTypeName()
        {
            if (Id == -1)
            {
                return Constants.StrNone;
            }

            DataRow plantType = Db.QueryFirst("SELECT PlantTypeName FROM PlantTypes WHERE PlantTypeID = ?",
                Id.ToString());

            if (plantType == null)
            {
                return null;
            }

            return plantType["PlantTypeName"].ToString();
        }

        public static PlantType[] GetAllPlantTypes()
        {
            DataTable dtPlantTypes = Db.Query("SELECT * FROM PlantTypes");
            List<PlantType> lstPlantTypes = new List<PlantType>();

            lstPlantTypes.Add(GetDefaultPlantType());

            lstPlantTypes.AddRange(from DataRow row in dtPlantTypes.Rows
                select new PlantType(Convert.ToInt32(row["PlantTypeID"])));
            return lstPlantTypes.ToArray();
        }

        public static void AddPlantType(string name)
        {
            Db.Execute("INSERT INTO PlantTypes (PlantTypeName) VALUES (?)", name);
        }

        public static PlantType GetDefaultPlantType()
        {
            return new PlantType(-1);
        }

        public override string ToString()
        {
            return Name;
        }

        public static PlantType GetPlantTypeByPlantId(int plantId)
        {
            DataRow item = Db.QueryFirst(
                "SELECT * FROM Plants INNER JOIN PlantTypes On PlantPlantTypeID = PlantTypeID WHERE PlantID = ?",
                plantId.ToString());

            return item == null
                ? GetDefaultPlantType()
                : new PlantType(Convert.ToInt32(item["PlantTypeID"]));
        }

        public static void DeletePlantTypeById(int id)
        {
            if (id != -1)
                Db.Execute("DELETE FROM PlantTypes WHERE PlantTypeID = ?", id.ToString());
        }
    }
}