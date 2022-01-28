using Galerija.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Galerija.Controllers
{
    public class UpravljanjeController : Controller
    {
        // GET: Upravljanje
        public ActionResult Index()
        {
            Korisnik usr = (Korisnik)Session["user"];
            if(usr == null || usr.Uloga != Uloga.ADMINISTRATOR)
            {
                return RedirectToAction("Index", "Autorizacija");
            }

            ViewBag.Slika = new Entity();
            
            return View();
        }

        [HttpPost]
        public ActionResult ObrisiSliku(int id)
        {
            Korisnik usr = (Korisnik)Session["user"];
            if (usr == null || usr.Uloga != Uloga.ADMINISTRATOR)
            {
                return RedirectToAction("Index", "Autorizacija");
            }
            List<Entity> slike = (List<Entity>)HttpContext.Application["slike"];
            foreach (var slika in slike)
            {
                if (slika.Value.Id == id)
                {
                    slika.Obrisan = true;
                    Models.Galerija.Save();
                }
            }

            HttpContext.Application["slike"] = slike;

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult IzmeniSliku(int id)
        {
            Korisnik usr = (Korisnik)Session["user"];
            if (usr == null || usr.Uloga != Uloga.ADMINISTRATOR)
            {
                return RedirectToAction("Index", "Autorizacija");
            }

            List<Entity> slike = (List<Entity>)HttpContext.Application["slike"];
            Entity ret = new Entity() ;
            foreach(var slika in slike)
            {
                if (slika.Value.Id == id)
                {
                    ret = slika;
                }
            }

            ViewBag.Slika = ret;
            return View("Index");
        }

        [HttpPost]
        public ActionResult Izmeni(Slika slika)
        {
            List<Entity> slike = (List<Entity>)HttpContext.Application["slike"];
            foreach(var s in slike)
            {
                if (s.Value.Id == slika.Id)
                {
                    if(slika.GodinaIzrade != null)
                    {
                        s.Value.GodinaIzrade = slika.GodinaIzrade;
                    }
                    if (slika.Naziv != null)
                    {
                        s.Value.Naziv = slika.Naziv;
                    }
                    if(slika.Autor.Ime != null)
                    {
                        s.Value.Autor.Ime = slika.Autor.Ime;
                    }
                    if(slika.Autor.Prezime != null)
                    {
                        s.Value.Autor.Prezime = slika.Autor.Prezime;
                    }
                    if(slika.Tehnika != null)
                    {
                        s.Value.Tehnika = slika.Tehnika;
                    }
                    if(slika.Opis != null)
                    {
                        s.Value.Opis = slika.Opis;
                    }
                    if(slika.Cena != null)
                    {
                        s.Value.Cena = slika.Cena;
                    }
                    if(slika.NaProdaju != s.Value.NaProdaju)
                    {
                        s.Value.NaProdaju = slika.NaProdaju;
                    }
                    HttpContext.Application["slike"] = slike;
                    Models.Galerija.Save();
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult DodajSliku()
        {
            Korisnik usr = (Korisnik)Session["user"];
            if (usr == null || usr.Uloga != Uloga.ADMINISTRATOR)
            {
                return RedirectToAction("Index", "Autorizacija");
            }
            
            
            return View("DodajSliku");
        }

        [HttpPost]
        public ActionResult DodajSliku(Slika slika, string galerija)
        {
            Korisnik usr = (Korisnik)Session["user"];
            if (usr == null || usr.Uloga != Uloga.ADMINISTRATOR)
            {
                return RedirectToAction("Index", "Autorizacija");
            }
            if (slika.Naziv == null)
            {
                ViewBag.Naziv = "min 1 karakter";
                return View("DodajSliku");
            }else if((slika.Cena != null) && (slika.Cena < 0 || slika.Cena > 100000000)){
                ViewBag.Cena = "0 < cena < 100 000 000";
                return View("DodajSliku");
            }
            List<Models.Galerija> galerije = (List<Models.Galerija>)HttpContext.Application["galerije"];
            slika.Id = Slika._id;
            Slika._id++;
            foreach (var gal in galerije)
            {
                if(gal.Naziv == galerija)
                {
                    gal.Slike.Add(new Entity(slika));
                    Models.Galerija.Save();
                    HttpContext.Application["galerije"] = galerije;
                }
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ObrisiGaleriju(string ime)
        {
            Korisnik usr = (Korisnik)Session["user"];
            if (usr == null || usr.Uloga != Uloga.ADMINISTRATOR)
            {
                return RedirectToAction("Index", "Autorizacija");
            }
            List<Models.Galerija> galerije = (List<Models.Galerija>)HttpContext.Application["galerije"];

            foreach (var gal in galerije)
            {
                if (gal.Naziv == ime)
                {
                    foreach (var slika in gal.Slike)
                    {
                        slika.Obrisan = true;
                    }
                    gal.Obrisan = true;
                    Models.Galerija.Save();
                }
            }

            HttpContext.Application["galerije"] = galerije;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult DodajGaleriju()
        {
            Korisnik usr = (Korisnik)Session["user"];
            if (usr == null || usr.Uloga != Uloga.ADMINISTRATOR)
            {
                return RedirectToAction("Index", "Autorizacija");
            }

            return View();
        }

        [HttpPost]
        public ActionResult DodajGaleriju(string naziv, string ulica, string broj, string grad)
        {
            Korisnik usr = (Korisnik)Session["user"];
            if (usr == null || usr.Uloga != Uloga.ADMINISTRATOR)
            {
                return RedirectToAction("Index", "Autorizacija");
            }
            if(naziv == string.Empty)
            {
                ViewBag.Naziv = "Naziv je obavezan";
                return View();
            }
            if(ulica == string.Empty)
            {
                ViewBag.Ulica = "Ulica je obavezna";
                return View();
            }
            if(broj == string.Empty)
            {
                ViewBag.Broj = "Broj je obavezan";
                return View();
            }
            else
            {
                int tmp;
                if (!Int32.TryParse(broj, out tmp))
                {
                    ViewBag.Broj = "Broj mora biti cifra";
                    return View();
                }
            }

            List<Models.Galerija> galerije = HttpContext.Application["galerije"] as List<Models.Galerija>;
            foreach(var gal in galerije)
            {
                if(naziv == gal.Naziv)
                {
                    if (!gal.Obrisan)
                    {
                        ViewBag.Error = "Galerija sa ovim naziv vec postoji";
                        return View();
                    }
                    gal.Adresa = ulica + ", " + broj + ", " + grad;
                    gal.Obrisan = false;
                    HttpContext.Application["galerije"] = galerije;
                    Models.Galerija.Save();

                    return RedirectToAction("Index", "Home");
                }
            }
            galerije.Add(new Models.Galerija(naziv, ulica + ", " + broj + ", " + grad) { Obrisan = false });
            HttpContext.Application["galerije"] = galerije;
            Models.Galerija.Save();

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult ObrisiKorisnika(string username)
        {
            Korisnik usr = (Korisnik)Session["user"];
            if (usr == null || usr.Uloga != Uloga.ADMINISTRATOR)
            {
                return RedirectToAction("Index", "Autorizacija");
            }
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            foreach (var kor in korisnici)
            {
                if (kor.Username == username)
                {
                    kor.Obrisan = true;
                    Korisnik tmp = (Korisnik)Session["user"];
                    if (tmp.Username == username)
                    {
                        Session["user"] = null;
                    }
                    HttpContext.Application["korisnici"] = korisnici;
                    Korisnik.Sacuvaj();
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}