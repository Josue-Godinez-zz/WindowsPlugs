namespace IIS_TEST.Models
{


    public class Site
    {
        public int Id { get; set; }
        public String SiteName { get; set; }
        //En veremos
        public String Schema { get; set; }
        public Boolean ServerAutoStart { get; set; }
        public String State { get; set; }
        public String LogFileDirectory { get; set; }
    }

    public class Pool
    {
        public Boolean AutoStart { get; set; }
        public String PoolName { get; set; }
        //En veremos
        public String State { get; set; }
        public String Schema { get; set; }
        public Boolean IsLocallyStored { get; set; }
        //Enable 32 bit on Windows x64
        public Boolean Enable32Bit { get; set; }
        // Managed Runtime Version
        public String Version { get; set; }
    }

    public class TaskSchedule
    {
        public String Name { get; set; }
        public String Path { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime LastRunTime { get; set; }
        public DateTime NextRunTime { get; set; }
        public int LastTimeResult { get; set; }
        public Boolean Enabled { get; set; }
        public String State { get; set; }
        public Boolean ReadOnly { get; set; }
        public String Description { get; set; }
        public String Autor { get; set; }
        public DateTime DateCreation { get; set; }
    }

}
