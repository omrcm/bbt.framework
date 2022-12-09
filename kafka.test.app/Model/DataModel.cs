using System;

namespace kafka.test.app
{
    //public class DataModel
    //{
    //    public long Id { get; set; }
    //    public string Email { get; set; }

    //    public override string ToString()
    //    {
    //        return $"Id: {Id} Email: {Email}";
    //    }
    //}

    public class AutoProssesDto
    {
        public int Id { get; set; }
    }
    public class DataModel
    {
        public string lat { get; set; }
        public string lon { get; set; }
        public Int64 id { get; set; }
        public double distance { get; set; }
        public string altitude { get; set; }
        public string hr { get; set; }
        public string cadence { get; set; }
        public DateTime time { get; set; }
        public string rawTime { get; set; }
    }

}