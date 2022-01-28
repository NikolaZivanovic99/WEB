using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Galerija.Models
{

    public class Slika
    {
        public static int _id = 0;
        public string Naziv { get; set; }
        public Osoba Autor { get; set; }
        public string GodinaIzrade { get; set; }
        public string Tehnika { get; set; }
        public string Opis { get; set; }
        public uint Cena { get; set; }
        public bool NaProdaju { get; set; }
        public int Id { get; set; }

        public Slika()
        {

        }

        public Slika(string naz, Osoba autor, string godina, string tehnika, string opis, uint cena, bool naProdaju)
        {
            this.Naziv = naz;
            this.Autor = autor;
            this.GodinaIzrade = godina;
            this.Tehnika = tehnika;
            this.Opis = opis;
            this.Cena = cena;
            this.NaProdaju = naProdaju;
            this.Id = ++_id;
        }

    }
}