using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PlantManager_WPF
{
    public class Shape
    {
        public Shape(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public string Name
        {
            get { return GetShapeName(); }
        }

        private string GetShapeName()
        {
            if (Id == -1)
            {
                return Constants.StrNone;
            }

            DataRow shape = Db.QueryFirst("SELECT ShapeName FROM Shapes WHERE ShapeID = ?",
                Id.ToString());

            if (shape == null)
                return null;
            return shape["ShapeName"].ToString();
        }

        public static Shape[] GetAllShapes()
        {
            DataTable dtShapes = Db.Query("SELECT * FROM Shapes");
            List<Shape> lstShapes = new List<Shape>();

            lstShapes.Add(GetDefaultShape());

            lstShapes.AddRange(from DataRow row in dtShapes.Rows
                select new Shape(Convert.ToInt32(row["ShapeID"])));
            return lstShapes.ToArray();
        }

        public static void AddShape(string name)
        {
            Db.Execute("INSERT INTO Shapes (ShapeName) VALUES (?)", name);
        }

        public static Shape GetDefaultShape()
        {
            return new Shape(-1);
        }

        public override string ToString()
        {
            return Name;
        }

        public static Shape GetShapeByPlantId(int plantId)
        {
            DataRow item = Db.QueryFirst(
                "SELECT * FROM Plants INNER JOIN Shapes On PlantShapeID = ShapeID WHERE PlantID = ?",
                plantId.ToString());

            return item == null
                ? GetDefaultShape()
                : new Shape(Convert.ToInt32(item["ShapeID"]));
        }

        public static void DeleteShapeById(int id)
        {
            if (id != -1)
                Db.Execute("DELETE FROM Shapes WHERE ShapeID = ?", id.ToString());
        }
    }
}