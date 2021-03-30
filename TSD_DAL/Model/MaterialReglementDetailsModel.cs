namespace TSD_DAL.Model
{
    public class MaterialReglementDetailsModel
    {
        public int ID { get; set; }
        public int ReglementID { get; set; }
        public string OwnerName { get; set; }
        public string VoucherNumber { get; set; }
        public double? Ammount { get; set; }

    }
}
