namespace WebApiDemo1.Models
{
    public class ServiceEntity
    {
        public string IP{ get; set; }
        public int Port{ get; set; }
        public string Name{ get; set; }
        public string ConsulIP{ get; set; }
        public int ConsulPort { get; set; }
        public Check Check { get; set; }
    }

    public class Check
    {
        public string Path { get; set; }
        public int Interval { get; set; }
        public int Timeout { get; set; }
    }
}
