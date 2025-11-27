namespace MedicalAPI.Miscellaneous
{
    public class GlobalResponse
    {
        public long Response_Code { get; set; }
        public string Response_Message { get; set; }
        public object ReponseData { get; set; }
    }
}
