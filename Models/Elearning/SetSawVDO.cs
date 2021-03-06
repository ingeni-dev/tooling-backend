namespace webAPI.Models.Elearning
{
    public class RequestSetSawVDO
    {
        public string token { get; set; }
        public string courseDocID { get; set; }
        public string queryID { get; set; }
        public string currTime { get; set; }
        public int openTime { get; set; }
        public string stateVDO { get; set; }
    }

    public class ReturnSetSawVDO
    {
        public bool stateError { get; set; }
        public string messageError { get; set; }
        public int currTime { get; set; }
    }

}