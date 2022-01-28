using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Galerija.Models
{
    public class Galerija
    {
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public List<Entity> Slike = new List<Entity>();
        public bool Obrisan { get; set; }

        public Galerija()
        {
            
        }

        public Galerija(string naziv, string adresa)
        {
            this.Naziv = naziv;
            this.Adresa = adresa;
        }

        public static void Save()
        {
            List<Galerija> galerije = (List<Galerija>)HttpContext.Current.Application["galerije"];
            using (var fs = new FileStream(HttpContext.Current.Server.MapPath(@"..\App_Data\Galerije.xml"), FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(List<Galerija>));
                serializer.Serialize(fs, galerije);
            }
        }
    }
}