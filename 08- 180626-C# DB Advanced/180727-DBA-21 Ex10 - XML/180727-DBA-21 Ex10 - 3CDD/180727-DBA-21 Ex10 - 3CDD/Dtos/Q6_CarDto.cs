﻿using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10___3CDD.Dtos
{
    [XmlType("car")]
    public class Q6_CarDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }
    }
}