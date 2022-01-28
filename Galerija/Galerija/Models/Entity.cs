using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Galerija.Models
{
    public class Entity
    {
        public int Key { get; set; }
        public Slika Value;
        public bool Obrisan { get; set; }

        public Entity()
        {
            
        }

        public Entity(Slika value)
        {
            this.Key = value.Id;
            this.Value = value;
        }
    }
}