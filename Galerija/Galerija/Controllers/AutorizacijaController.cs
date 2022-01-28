using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Galerija.Models;

namespace Galerija.Controllers
{
    public class AutorizacijaController : Controller
    {
        // GET: Autorizacija
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registruj()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registruj(Korisnik korisnik)
        {
            if(korisnik.Username.Length < 3)
            {
                ViewBag.Username = "minimalno 3 karaktera";
                return View();
            }else if(korisnik.Password.Length < 8)
            {
                ViewBag.Pass = "minimalno 8 karaktera";
                return View();
            }
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            foreach(var kor in korisnici)
            {
                if(kor.Username == korisnik.Username)
                {
                    ViewBag.Username = "Korisnicko ime vec postoji";
                    return View();
                }
            }
            korisnik.Uloga = Uloga.KUPAC;
            Korisnik.Sacuvaj(korisnik);

            return View("Index");
        }

        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            if(username.Equals(string.Empty) || password.Equals(string.Empty))
            {
                return View("Index");
            }

            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            foreach(var korisnik in korisnici)
            {
                if(korisnik.Username.Equals(username) && korisnik.Password.Equals(password))
                {
                    if (!korisnik.Obrisan)
                    {
                        Session["user"] = korisnik;

                        return RedirectToAction("Index", "Home");
                    }

                }
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}