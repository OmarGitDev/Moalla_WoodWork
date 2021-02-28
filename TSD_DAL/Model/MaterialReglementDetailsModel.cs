namespace TSD_DAL.Model
{
    public class MaterialReglementDetailsModel
    {
        public int ID { get; set; }
        public int ReglementID { get; set; }
        public int ProductID { get; set; }
        public double Amount { get; set; }

        public string ProductName { get; set; }
    }
}
