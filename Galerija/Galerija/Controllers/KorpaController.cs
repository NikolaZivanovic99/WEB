using Galerija.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Galerija.Controllers
{
    public class KorpaController : Controller
    {
        // GET: Korpa
        public ActionResult Index()
        {
            var user = Session["user"] as Korisnik;
            if(user == null || user.Uloga != Uloga.KUPAC)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Kupovina> kupovine = new List<Kupovina>();
            foreach(var kup in HttpContext.Application["istorijaKupovine"] as List<Kupovina>)
            {
                if(kup.Kupac.Username == user.Username)
                {
                    kupovine.Add(kup);
                }
            }
            return View(kupovine);
        }

        [HttpPost]
        public ActionResult Dodaj(int id)
        {
            var user = Session["user"] as Korisnik;
            if (user == null || user.Uloga != Uloga.KUPAC)
            {
                return RedirectToAction("Index", "Home");
            }

            var slike = HttpContext.Application["slike"] as List<Entity>;
            var uKorpi = HttpContext.Application["korpa"] as List<Entity>;
            if(uKorpi == null)
            {
                uKorpi = new List<Entity>();
            }
            foreach(var slika in uKorpi)
            {
                if(slika.Key == id)
                {
                   TempData["error"] = "Slika " + slika.Value.Naziv + " je vec u korpi!";
                    return RedirectToAction("Index", "Home");
                }
            }
            foreach(var slika in slike)
            {
                if(slika.Key == id)
                {
                    uKorpi.Add(slika);
                    HttpContext.Application["korpa"] = uKorpi;
                    break;
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Korpa()
        {
            var user = Session["user"] as Korisnik;
            if (user == null || user.Uloga != Uloga.KUPAC)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Entity> korpa = HttpContext.Application["korpa"] as List<Entity>;
            if(korpa == null)
            {
                korpa = new List<Entity>();
            }

            return View(korpa);
        }

        [HttpPost]
        public ActionResult Ukloni(string id)
        {
            List<Entity> korpa = HttpContext.Application["korpa"] as List<Entity>;
            foreach(var slika in korpa)
            {
                if(slika.Key == int.Parse(id))
                {
                    korpa.Remove(slika);
                    HttpContext.Application["korpa"] = korpa;
                    break;
                }
            }

            return RedirectToAction("Korpa");
        }

        [HttpPost]
        public ActionResult Kupi(string uk)
        {
            List<Entity> korpa = HttpContext.Application["korpa"] as List<Entity>;
            if (korpa == null || korpa.Count == 0)
            {
                ViewBag.Error = "Korpa je prazna!";
                return View("Korpa", korpa);
            }

            var kupovine = HttpContext.Application["istorijaKupovine"] as List<Kupovina>;
            var kupovina = new Kupovina();
            kupovina.Kupac = Session["user"] as Korisnik;
            kupovina.DatumKupovine = DateTime.Now;
            kupovina.UkupnoCena = uint.Parse(uk);
            var slike = HttpContext.Application["slike"] as List<Entity>;
            foreach (var slika in slike)
            {
                foreach (var item in korpa)
                {
                    if (item.Key == slika.Key)
                    {
                        slika.Obrisan = true;
                        kupovina.Slike.Add(item);
                    }
                }
            }

            kupovine.Add(kupovina);
            HttpContext.Application["korpa"] = new List<Entity>();
            HttpContext.Application["istorijaKupovine"] = kupovine;
            HttpContext.Application["slike"] = slike;
            Kupovina.Save();
            Models.Galerija.Save();

            return RedirectToAction("Index");
        }

    }
}