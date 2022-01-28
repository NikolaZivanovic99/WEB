using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Galerija.Models;

namespace Galerija.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<Entity> slike = (List<Entity>)HttpContext.Application["slike"];
            Korisnik user = (Korisnik)Session["user"];
            if(user != null && user.Uloga == Uloga.ADMINISTRATOR)
            {
                return View("Admin", slike);
            }else if(user != null && user.Uloga == Uloga.KUPAC)
            {
                return View("Kupac", slike);
            }

            return View(slike);
        }

        [HttpPost]
        public ActionResult Sortiraj()
        {
            string nacin = HttpContext.Request["nacin"];
            string smer = HttpContext.Request["smer"];
            
            List<Entity> slike = (List<Entity>)HttpContext.Application["slike"];
          
                if(nacin == "naziv")
                {
                    slike = _SortirajNaziv(smer, slike);
                }else if(nacin == "tehnika")
                {
                    slike = _SortirajTehnika(smer, slike);
                }else if(nacin == "cena")
                {
                    slike = _SortirajCena(smer, slike);
                }

            Korisnik user = (Korisnik)Session["user"];
            if (user != null && user.Uloga == Uloga.ADMINISTRATOR)
            {
                return View("Admin", slike);
            }
            if(user != null && user.Uloga == Uloga.KUPAC)
            {
                return View("Kupac", slike);
            }

            return View("Index", slike);
        }


        [HttpPost]
        public ActionResult Pretraga()
        {
            List<Entity> slike = (List<Entity>)HttpContext.Application["slike"];
            List<Entity> ret = slike;
            int od = -1, Do = -1;
            
            bool cena = (Int32.TryParse(HttpContext.Request["od"], out od) && Int32.TryParse(HttpContext.Request["do"], out Do));
            string naziv = HttpContext.Request["naziv"] != null ? HttpContext.Request["naziv"] : string.Empty;
            string tehnika = HttpContext.Request["tehnika"] != null ? HttpContext.Request["naziv"] : string.Empty;

            if (cena)
            {
                ret = _PretragaCene(od, Do, slike);
            }
            else if (naziv != string.Empty)
            {
                ret = _PretragaNaziv(naziv, slike);
            }
            else if (tehnika != string.Empty)
            {
                ret = _PretragaTehnika(tehnika, slike);
            }

            Korisnik user = (Korisnik)Session["user"];
            if (user != null && user.Uloga == Uloga.ADMINISTRATOR)
            {
                return View("Admin", ret);
            }
            if (user != null && user.Uloga == Uloga.KUPAC)
            {
                return View("Kupac", slike);
            }

            return View("Index", ret);
        }

        public ActionResult Ocisti()
        {
            List<Entity> slike = (List<Entity>)HttpContext.Application["slike"];


            Korisnik user = (Korisnik)Session["user"];
            if (user != null && user.Uloga == Uloga.ADMINISTRATOR)
            {
                return View("Admin", slike);
            }
            if (user != null && user.Uloga == Uloga.KUPAC)
            {
                return View("Kupac", slike);
            }
            return View("Index", slike);
        }

        [HttpPost]
        public ActionResult PoGal(string naziv)
        {
            var user = Session["user"] as Korisnik;
            if(user.Uloga != Uloga.KUPAC || user == null)
            {
                return RedirectToAction("Index");
            }

            var slike = new List<Entity>();

            var galerije = HttpContext.Application["galerije"] as List<Models.Galerija>;
            foreach(var gal in galerije)
            {
                if(gal.Naziv == naziv)
                {
                    slike = gal.Slike;
                }
            }
            ViewBag.Flag = "flag";
            return View("Kupac", slike);
        }

        private List<Entity> _SortirajNaziv(string smer, List<Entity> slike)
        {
            List<Entity> ret = new List<Entity>();

            if(smer == "rastuce")
            {
                ret = slike.OrderBy(s => s.Value.Naziv).ToList();
            }
            else
            {
                ret = slike.OrderByDescending(s => s.Value.Naziv).ToList();
            }

            return ret;
        }

        private List<Entity> _SortirajTehnika(string smer, List<Entity> slike)
        {
            List<Entity> ret = new List<Entity>();

            if (smer == "rastuce")
            {
                ret = slike.OrderBy(s => s.Value.Tehnika).ToList();
            }
            else
            {
                ret = slike.OrderByDescending(s => s.Value.Tehnika).ToList();
            }

            return ret;
        }

        private List<Entity> _SortirajCena(string smer, List<Entity> slike)
        {
            List<Entity> ret = new List<Entity>();

            if (smer == "rastuce")
            {
                ret = slike.OrderBy(s => s.Value.Cena).ToList();
            }
            else
            {
                ret = slike.OrderByDescending(s => s.Value.Cena).ToList();
            }

            return ret;
        }

        private List<Entity> _PretragaCene(int od , int Do, List<Entity> slike)
        {
            List<Entity> ret = new List<Entity>();
            foreach(var slika in slike)
            {
                if(od <= slika.Value.Cena && slika.Value.Cena <= Do)
                {
                    ret.Add(slika);
                }
            }

            return ret;
        }

        private List<Entity> _PretragaNaziv(string naziv, List<Entity> slike)
        {
            List<Entity> ret = new List<Entity>();
            foreach (var slika in slike)
            {
                if (slika.Value.Naziv.IndexOf(naziv, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ret.Add(slika);
                }
            }

            return ret;
        }

        private List<Entity> _PretragaTehnika(string tehnika, List<Entity> slike)
        {
            List<Entity> ret = new List<Entity>();
            foreach (var slika in slike)
            {
                if (slika.Value.Tehnika.IndexOf(tehnika, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ret.Add(slika);
                }
            }
            return ret;
        }
    }
}