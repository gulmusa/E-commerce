using Ecommerce.WebUI.App_Classes;
using Ecommerce.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.WebUI.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Urunler()
        {
            return View(Context.Baglanti.Urun.ToList());
        }

        public ActionResult UrunEkle()
        {
            ViewBag.Kategoriler = Context.Baglanti.Kategori.ToList();
            ViewBag.Markalar = Context.Baglanti.Marka.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult UrunEkle(Urun urn)
        {
            Context.Baglanti.Urun.Add(urn);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");
        }
        public ActionResult Markalar()
        {
            return View(Context.Baglanti.Marka.ToList());
        }

        public ActionResult MarkaEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MarkaEkle(Marka mrk, HttpPostedFileBase fileUpload)
        {
            int resimId = -1;
            if (fileUpload != null)
            {

                Image img = Image.FromStream(fileUpload.InputStream);
                int width = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaResimWidth"].ToString()); //Web Config de <add key="DegerİsmiYazılır" value="Deger Atanır" şeklinde eklediğimiz width ve height leri burak ConfigurationManager.AppSettings ile ulaşarak int değer içine atadık/>
                int height = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaResimHeight"].ToString());//Web Config de <add key="DegerİsmiYazılır" value="Deger Atanır" şeklinde eklediğimiz width ve height leri burak ConfigurationManager.AppSettings ile ulaşarak int değer içine atadık/>

                // Guid 24 basamaklı eşi benzeri olmayan bir string ifade verir
                string name = "/Content/MarkaResim/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);

                Bitmap bm = new Bitmap(img, width, height);
                bm.Save(Server.MapPath(name));

                Resim rsm = new Resim();

                rsm.OrtaYol = name;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
                if (rsm.Id != null)
                    resimId = rsm.Id;
            }
            if (resimId != -1)
            {
                mrk.ResimID = resimId;
            }
            Context.Baglanti.Marka.Add(mrk);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Markalar");
        }

        public ActionResult UrunResimEkle(int id)
        {
            return View(id);
        }
        [HttpPost]
        public ActionResult UrunResimEkle(int id, HttpPostedFileBase fileupload)
        {
            if (fileupload != null)
            {
                Image img = Image.FromStream(fileupload.InputStream);
                int UrunOrtaWidth = Convert.ToInt32(ConfigurationManager.AppSettings["UrunOrtaWidth"].ToString());
                int UrunOrtaHeight = Convert.ToInt32(ConfigurationManager.AppSettings["UrunOrtaHeight"].ToString());

                int UrunBuyukWidth = Convert.ToInt32(ConfigurationManager.AppSettings["UrunBuyukWidth"].ToString());
                int UrunBuyukHeight = Convert.ToInt32(ConfigurationManager.AppSettings["UrunBuyukHeight"].ToString());

 

                string ortaYol = "/Content/UrunResim/Orta/" + Guid.NewGuid() + Path.GetExtension(fileupload.FileName);
                string buyukYol = "/Content/UrunResim/Buyuk/" + Guid.NewGuid() + Path.GetExtension(fileupload.FileName);
                string kucukYol = "/Content/UrunResim/Orta/" + Guid.NewGuid() + Path.GetExtension(fileupload.FileName);

                Bitmap bmOrtaResim = new Bitmap(img, UrunOrtaWidth, UrunOrtaHeight);
                Bitmap bmBuyukResim = new Bitmap(img, UrunBuyukWidth, UrunBuyukHeight);


                bmOrtaResim.Save(Server.MapPath(ortaYol));
                bmBuyukResim.Save(Server.MapPath(buyukYol));

                Resim rsm = new Resim();
                rsm.BuyukYol = buyukYol;
                rsm.OrtaYol = ortaYol;
                rsm.UrunID = id;
                if (Context.Baglanti.Resim.FirstOrDefault(x => x.UrunID == id && x.Varsayilan == false) != null)
                {
                    rsm.Varsayilan = true;
                }
                else
                {
                    rsm.Varsayilan = false;
                }
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
                return View(id);
            }
            return View(id);
        } 
        public ActionResult Kategoriler()
        {
            return View(Context.Baglanti.Kategori.ToList());
        }

        public ActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KategoriEkle(Kategori ktg)
        {
            Context.Baglanti.Kategori.Add(ktg);
            Context.Baglanti.SaveChanges(); 
            return RedirectToAction("Kategoriler");
        }

        public ActionResult OzellikTipleri()
        { 
            return View(Context.Baglanti.OzellikTip.ToList());
        }

        public ActionResult OzellikTipEkle()
        {
            return View(Context.Baglanti.Kategori.ToList());
        }
        [HttpPost]
        public ActionResult OzellikTipEkle(OzellikTip ot)
        {
            Context.Baglanti.OzellikTip.Add(ot);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikTipleri");
        }

        public ActionResult OzellikDegerleri()
        { 
            return View(Context.Baglanti.OzellikDeger.ToList());
        }
         
        public ActionResult OzellikDegerEkle()
        { 
            return View(Context.Baglanti.OzellikTip.ToList());
        }
        [HttpPost]
        public ActionResult OzellikDegerEkle(OzellikDeger od)
        {
            Context.Baglanti.OzellikDeger.Add(od);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikDegerleri");
        }

        public ActionResult UrunOzellikleri()
        {
            return View(Context.Baglanti.UrunOzellik.ToList());
        }

        public ActionResult UrunOzellikSil(int urunId, int tipId, int degerId)
        {
            UrunOzellik uo = Context.Baglanti.UrunOzellik.FirstOrDefault(x => x.UrunID == urunId && x.OzellikTipID == tipId && x.OzellikDegerID == degerId);
            Context.Baglanti.UrunOzellik.Remove(uo);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("UrunOzellikleri");
        } 
        public ActionResult UrunOzellikEkle()
        { 
            return View(Context.Baglanti.Urun.ToList());
        }
        [HttpPost]
        public ActionResult UrunOzellikEkle(UrunOzellik uo)
        {
            Context.Baglanti.UrunOzellik.Add(uo);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("UrunOzellikleri");
        }

        public PartialViewResult UrunOzellikTipWidget(int? katId)
        {
            if (katId != null)
            {
                var data = Context.Baglanti.OzellikTip.Where(x => x.KategoriID == katId).ToList();
                return PartialView(data);
            }
            else
            {
                var data = Context.Baglanti.OzellikTip.ToList();
                return PartialView(data);
            } 
        }

        public PartialViewResult UrunOzellikDegerWidget(int? tipId)
        {
            if (tipId != null)
            {
                var data = Context.Baglanti.OzellikDeger.Where(x => x.OzellikTipID == tipId).ToList();
                return PartialView(data);
            }
            else
            {
                var data = Context.Baglanti.OzellikDeger.ToList();
                return PartialView(data);
            }
        } 

        public ActionResult SliderResimleri()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SliderResimEkle(HttpPostedFileBase fileupload)
        {
            if (fileupload != null)
            {
                Image img = Image.FromStream(fileupload.InputStream);
                Bitmap bmp = new Bitmap(img, Settings.SliderResimBoyut);
                string yol = "/Content/SliderResim/" + Guid.NewGuid() + Path.GetExtension(fileupload.FileName);
                bmp.Save(Server.MapPath(yol));

                Resim rsm = new Resim();
                rsm.BuyukYol = yol;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
            }
            return RedirectToAction("SliderResimleri");
        }

        public ActionResult UrunSil(Urun urn)
        {
            var silinecekurun = Context.Baglanti.Urun.FirstOrDefault(x => x.Id == urn.Id);
            Context.Baglanti.Urun.Remove(silinecekurun);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");
        } 

        public ActionResult UrunGuncelle(int id)
        {
            var guncellenecekurun = Context.Baglanti.Urun.FirstOrDefault(x=>x.Id==id);

            ViewBag.Kategoriler = Context.Baglanti.Kategori.ToList();
            ViewBag.Markalar = Context.Baglanti.Marka.ToList();
            return View(guncellenecekurun);
        }
        [HttpPost]
        public ActionResult UrunGuncelle(Urun urn)
        {
            var guncellenecekurun = Context.Baglanti.Urun.FirstOrDefault(x => x.Id == urn.Id);
             
            return RedirectToAction("Urunler");
        }

    }
}