using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Galerija.Models
{
    public class Osoba
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }

        public Osoba()
        {
            
        }

        public Osoba(string ime, string prezime)
        {
            this.Ime = ime;
            this.Prezime = prezime;
        }

        public override string ToString()
        {
            return Ime + " " + Prezime;
        }
    }
}