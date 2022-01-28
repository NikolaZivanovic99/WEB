using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Galerija.Models
{
    public class Kupovina
    {
        public Korisnik Kupac { get; set; }
        public List<Entity> Slike = new List<Entity>();
        public DateTime DatumKupovine { get; set; }
        public uint UkupnoCena { get; set; }

        public Kupovina()
        {

        }

        public static void Save()
        {
            List<Kupovina> kupovine = (List<Kupovina>)HttpContext.Current.Application["istorijaKupovine"];
            using (var fs = new FileStream(HttpContext.Current.Server.MapPath(@"..\App_Data\Kupovine.xml"), FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(List<Kupovina>));
                serializer.Serialize(fs, kupovine);
            }
        }
    }
}