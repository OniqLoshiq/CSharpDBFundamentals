﻿using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10___3CDD.Dtos
{
    [XmlType("part")]
    public class Q4_PartDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
