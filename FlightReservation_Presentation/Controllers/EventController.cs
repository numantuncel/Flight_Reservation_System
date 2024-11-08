using BusinessLayer.Concrete;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightReservationSystem_Presentation.Controllers
{
    [AllowAnonymous]
    public class EventController : Controller
    {
        private readonly EventMenager _eventMenager;

        public EventController(EventMenager eventMenager)
        {
            _eventMenager = eventMenager;
        }

        public IActionResult EtkinlikListele()
        {
            var values = _eventMenager.GetListAll();
            return View(values);
        }


        [HttpGet]
        public IActionResult AddEvent()
        {
            return View("AddEvent");
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent(Event _event, IFormFile eventPhoto)
        {

            try
            {
                // Fotoğraf dosyası yükleniyor mu?
                if (eventPhoto != null && eventPhoto.Length > 0)
                {
                    var fileName = Path.GetFileName(eventPhoto.FileName); // Dosya adını al
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName); // Dosya yolu oluştur

                    // Fotoğrafı yükle
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await eventPhoto.CopyToAsync(stream); // Dosyayı belirtilen yola kopyala
                    }

                    // Yüklenen fotoğrafın URL'sini oluştur
                    _event.EventPhoto = "/images/" + fileName; // Model'deki 'EventPhoto' alanına URL'yi ekliyoruz.
                }

                // Yeni etkinlik nesnesi oluştur
                Event values = new Event()
                {
                    Id = _event.Id,
                    EventName = _event.EventName,
                    EventDescription = _event.EventDescription,
                    EventPhoto = _event.EventPhoto, // URL'sini buraya atıyoruz
                    StartDate = _event.StartDate.ToUniversalTime(),
                    EndDate = _event.EndDate.ToUniversalTime(),
                    Capacity = _event.Capacity,
                    Tags = _event.Tags,
                    IsAvailable = _event.IsAvailable,
                };

                // Veritabanına ekleme işlemi yapılabilir
                _eventMenager.Insert(values);
                return RedirectToAction("EtkinlikListele"); // Başarıyla ekleme yaptıktan sonra yönlendirme

            }
            catch (Exception error)
            {
                ModelState.AddModelError("", "Bir hata oluştu: " + error.Message);

            }


            return View("AddEvent", _event); // Hata varsa aynı sayfayı döndür
        }

        [HttpGet]
        public IActionResult EditEvent(int id)
        {
            var values = _eventMenager.GetById(id);
            return View(values);
        }


        [HttpPost]
        public IActionResult EditEvent(Event _event)
        {
            if (_event != null)
            {
                try
                {
                    _eventMenager.Update(_event);
                    return RedirectToAction("EtkinlikListele");
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                return View();
            }
        }
    }
}
