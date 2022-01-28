using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Galerija.Models
{
    public enum Pol { MUSKI, ZENSKI };
    public enum Uloga { ADMINISTRATOR, KUPAC };

    public class Korisnik : Osoba
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Pol Pol{ get; set; }
        public string Email { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public Uloga Uloga { get; set; }

        public bool Obrisan { get; set; }

        public Korisnik()
        {

        }

        public Korisnik(string usr, string pass, string ime, string prezime, Pol pol, string email, DateTime rodj) : base(ime, prezime)
        {
            this.Username = usr;
            this.Password = pass;
            this.Pol = pol;
            this.Email = email;
            this.DatumRodjenja = rodj;
        }

        public static void Sacuvaj(Korisnik korisnik)
        {
            var korisnici = (List<Korisnik>)HttpContext.Current.Application["korisnici"];
            korisnici.Add(korisnik);
            using (var fs = new FileStream(HttpContext.Current.Server.MapPath(@"..\App_Data\KorisniciData.xml"), FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(List<Korisnik>));
                serializer.Serialize(fs, korisnici);
            }
        }

        public static void Sacuvaj()
        {
            var korisnici = (List<Korisnik>)HttpContext.Current.Application["korisnici"];
            using (var fs = new FileStream(HttpContext.Current.Server.MapPath(@"..\App_Data\KorisniciData.xml"), FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(List<Korisnik>));
                serializer.Serialize(fs, korisnici);
            }
        }
    }
}