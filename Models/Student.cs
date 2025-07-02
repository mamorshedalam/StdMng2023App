namespace StdMng2023App.Models
{
    public class Student
    {
        public string stuID { get; set; } = "";
        public string name { get; set; } = "";
        public string sex { get; set; } = "";
        public string depno { get; set; } = "";
        public string depname { get; set; } = "";
        public string grade { get; set; } = "";
        public string @class { get; set; } = "";
        public string tel { get; set; } = "";
        public string addr { get; set; } = "";
        public override string ToString()
        {
            return $"{stuID} - {name}";
        }
    }
}
