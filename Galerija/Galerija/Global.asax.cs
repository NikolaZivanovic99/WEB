using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Serialization;
using Galerija.Models;

namespace Galerija
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            using (var fs = new FileStream(Server.MapPath(@".\App_Data\KorisniciData.xml"), FileMode.Open))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<Korisnik>));
                var korisnici = (List<Korisnik>)xmlSerializer.Deserialize(fs);

                Application["korisnici"] = korisnici;
            }
            
            using (var fs = new FileStream(Server.MapPath(@".\App_Data\Galerije.xml"), FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(List<Models.Galerija>));
                var galerije = (List<Models.Galerija>)serializer.Deserialize(fs);

                Application["galerije"] = galerije;
                List<Entity> slike = new List<Entity>();
                int max = galerije[0].Slike[0].Key;
                foreach (var galerija in galerije)
                {
                    foreach (var slika in galerija.Slike)
                    {
                        slike.Add(slika);
                        if(slika.Key > max)
                        {
                            max = slika.Key;
                        }
                    }
                }

                Slika._id = max;
                Application["slike"] = slike;
            }

            using (var fs = new FileStream(Server.MapPath(@".\App_Data\Kupovine.xml"), FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(List<Kupovina>));
                List <Kupovina> kupovine = serializer.Deserialize(fs) as List<Kupovina>;
                if(kupovine == null)
                {
                    kupovine = new List<Kupovina>();
                }
                Application["istorijaKupovine"] = kupovine;
            }
            
        }
    }
}
