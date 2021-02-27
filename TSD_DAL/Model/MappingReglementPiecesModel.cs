namespace TSD_DAL.Model
{
    public class MappingReglementPiecesModel
    {
        public int ID { get; set; }
        public int ReglementID { get; set; }
        public string PieceID { get; set; }
        public double Montant { get; set; }
        public bool RAS { get; set; }
    }
}
