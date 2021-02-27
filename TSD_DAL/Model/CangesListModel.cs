namespace TSD_DAL.Model
{
    public class CongesListModel
    {
        public string title { get; set; }
        public string motif { get; set; }
        public int starty { get; set; }
        public int startM { get; set; }
        public int startd { get; set; }

        public int endy { get; set; }
        public int endM { get; set; }
        public int endd { get; set; }

        public CongesListModel()
        { }
        public CongesListModel(string personName, int Starty, int StartM, int Startd, int Endy, int EndM, int Endd, string Motif)
        {
            title = personName;
            motif = Motif;


            starty = Starty;
            startM = StartM;
            startd = Startd;

            endy = Endy;
            endM = EndM;
            endd = Endd;
        }
    }
}
