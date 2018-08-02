using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10.Dtos
{
    [XmlRoot("users")]
    public class Q4_UsersDto
    {
        [XmlAttribute("count")]
        public int UsersCount { get; set; }

        [XmlElement("user")]
        public Q4_UserDto[] Users { get; set; }
    }
}
