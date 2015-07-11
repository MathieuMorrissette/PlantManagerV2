using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PlantManager_WPF
{
    public class HardinessZone
    {
        public HardinessZone(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public string Name
        {
            get { return GetHardinessZoneName(); }
        }

        private string GetHardinessZoneName()
        {
            if (Id == -1)
            {
                return Constants.StrNone;
            }

            DataRow plant = Db.QueryFirst("SELECT HardinessZoneName FROM HardinessZones WHERE HardinessZoneID = ?",
                Id.ToString());
            return plant["HardinessZoneName"].ToString();
        }

        public static HardinessZone[] GetAllHardinessZones()
        {
            DataTable dtGenus = Db.Query("SELECT * FROM HardinessZones");
            List<HardinessZone> lstHardinessZones = new List<HardinessZone>();

            lstHardinessZones.Add(GetDefaultHardinessZone());

            lstHardinessZones.AddRange(from DataRow row in dtGenus.Rows
                select new HardinessZone(Convert.ToInt32(row["HardinessZoneID"])));
            return lstHardinessZones.ToArray();
        }

        public static void AddHardinessZone(string name)
        {
            Db.Execute("INSERT INTO HardinessZones (HardinessZoneName) VALUES (?)", name);
        }

        public static HardinessZone GetDefaultHardinessZone()
        {
            return new HardinessZone(-1);
        }

        public override string ToString()
        {
            return Name;
        }

        public static HardinessZone GetHardinessZoneByPlantId(int plantId)
        {
            DataRow item = Db.QueryFirst(
                "SELECT * FROM Plants INNER JOIN HardinessZones On PlantHardinessZoneID = HardinessZoneID WHERE PlantID = ?",
                plantId.ToString());

            return item == null
                ? GetDefaultHardinessZone()
                : new HardinessZone(Convert.ToInt32(item["HardinessZoneID"]));
        }

        public static void DeleteHardinessZoneById(int id)
        {
            if (id != -1)
                Db.Execute("DELETE FROM HardinessZones WHERE HardinessZoneID = ?", id.ToString());
        }
    }
}