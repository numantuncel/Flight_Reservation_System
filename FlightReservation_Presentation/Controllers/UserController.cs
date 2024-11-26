using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete.EntityFramework;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using FlightReservation_Presentation.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using BusinessLayer.Sifrelemeler;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace FlightReservationSystem_Presentation.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {


        private readonly UserMenager _userMenager;
        private readonly IHttpClientFactory _httpClientFactory;


        public UserController(UserMenager userMenager, IHttpClientFactory httpClientFactory)
        {
            _userMenager = userMenager;
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }


        // USER LİSTELEME, EKLEME ,GÜNCELLEME VE SİLME İŞLEMLERİ BAŞLANGIÇ 
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var values = _userMenager.GetListAll();
            return View(values);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddUser()
        {
            return View("AddUser");
        }



        [HttpPost]
        public async Task<IActionResult> AddUser(User _user, IFormFile userPhoto)
        {
            // Doğrulama kurallarını kontrol et
            UserValidator validationRules = new UserValidator();
            ValidationResult result = validationRules.Validate(_user);

            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View("AddUser", _user); // Hata varsa aynı sayfayı döndür
            }

            try
            {
                if (userPhoto != null && userPhoto.Length > 0)
                {
                    var fileName = Path.GetFileName(userPhoto.FileName); // Dosya adını al
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/template/", fileName); // Dosya yolu oluştur

                    // Fotoğrafı yükle
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await userPhoto.CopyToAsync(stream); // Dosyayı belirtilen yola kopyala
                    }

                    // Yüklenen fotoğrafın URL'sini oluştur
                    _user.ProfilePicture = "images/" + fileName; // Model'deki 'EventPhoto' alanına URL'yi ekliyoruz.
                }
                // Yeni etkinlik nesnesi oluştur
                User values = new User()
                {
                    FirstName = _user.FirstName,
                    LastName = _user.LastName,

                    ProfilePicture = _user.ProfilePicture,
                    PhoneNumber = _user.PhoneNumber,
                    PasswordHash = Md5Sifreleme.ComputeMD5(_user.PasswordHash),
                    Email = _user.Email,
                    IsEmailVerified = _user.IsEmailVerified,
                    CreatedAt = _user.CreatedAt.ToUniversalTime(),
                    //UpdatedAt = _user.UpdatedAt.HasValue ? _user.UpdatedAt.Value.ToUniversalTime() : (DateTime?)null

                    RoleUrl = RandomUrl_.GenerateComplexRoleId(),
                    Role = "User"
                };

                // Veritabanına ekleme işlemi yapılabilir
                _userMenager.Insert(values);
                return RedirectToAction("Index"); // Başarıyla ekleme yaptıktan sonra yönlendirme

            }
            catch (Exception error)
            {
                // Loglama veya hata mesajı gösterme işlemi
                ModelState.AddModelError("", "Bir hata oluştu: " + error.Message);

            }
            return View("AddUser", _user); // Hata varsa aynı sayfayı döndür
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            var values = _userMenager.GetById(id);
            _userMenager.Delete(values);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _userMenager.GetById(id); // Kullanıcıyı al
            if (user == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döndür
            }
            return View(user); // Kullanıcı bilgilerini view'a gönder
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User _user, IFormFile userPhoto)
        {
            UserValidator validationRules = new UserValidator();
            ValidationResult result = validationRules.Validate(_user);

            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View("EditUser", _user); // Hata varsa aynı sayfayı döndür
            }

            try
            {
                if (userPhoto != null && userPhoto.Length > 0)
                {
                    var fileName = Path.GetFileName(userPhoto.FileName); // Dosya adını al
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/template/", fileName); // Dosya yolu oluştur

                    // Fotoğrafı yükle
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await userPhoto.CopyToAsync(stream); // Dosyayı belirtilen yola kopyala
                    }

                    // Yüklenen fotoğrafın URL'sini oluştur
                    _user.ProfilePicture = "images/" + fileName; // Model'deki 'EventPhoto' alanına URL'yi ekliyoruz.
                }
                // Yeni etkinlik nesnesi oluştur



                User values = new User()
                {

                    Id = _user.Id,
                    FirstName = _user.FirstName,
                    LastName = _user.LastName,

                    ProfilePicture = _user.ProfilePicture,
                    PhoneNumber = _user.PhoneNumber,
                    PasswordHash = Md5Sifreleme.ComputeMD5(_user.PasswordHash),
                    Email = _user.Email,
                    IsEmailVerified = _user.IsEmailVerified,
                    CreatedAt = _user.CreatedAt.AddHours(3).ToUniversalTime(),
                    UpdatedAt = _user.UpdatedAt.HasValue ? _user.UpdatedAt.Value.ToUniversalTime() : (DateTime?)null,

                    RoleUrl = RandomUrl_.GenerateComplexRoleId(),
                    Role = "Admin"
                };

                // Veritabanına ekleme işlemi yapılabilir
                _userMenager.Update(values);
                return RedirectToAction("Index"); // Başarıyla ekleme yaptıktan sonra yönlendirme

            }
            catch (Exception error)
            {
                // Loglama veya hata mesajı gösterme işlemi
                ModelState.AddModelError("", "Bir hata oluştu: " + error.Message);

            }
            return View("EditUser", _user); // Hata varsa aynı sayfayı döndür
        }
        // USER LİSTELEME, EKLEME ,GÜNCELLEME VE SİLME İŞLEMLERİ BİTİŞ





        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(User _user)
        {

            try
            {
                User user = new User()
                {
                    FirstName = _user.FirstName,
                    LastName = _user.LastName,

                    PhoneNumber = _user.PhoneNumber,
                    ProfilePicture = "images/no-user-image.jpg",
                    Email = _user.Email,
                    PasswordHash = Md5Sifreleme.ComputeMD5(_user.PasswordHash),
                    CreatedAt = DateTime.Now.ToUniversalTime(),
                    IsEmailVerified = false, // Başlangıçta doğrulanmamış

                    RoleUrl = RandomUrl_.GenerateComplexRoleId(),
                    Role = "Admin"
                };

                _userMenager.Insert(user);

                //// E-posta doğrulama bağlantısı oluştur
                //var verificationLink = Url.Action("VerifyEmail", "Login", new { email = user.Email }, Request.Scheme);

                //// E-posta mesajı oluştur
                //var message = $"Doğrulama için lütfen <a href='{verificationLink}'>buraya tıklayın</a>.";

                //// E-posta gönder
                //await _emailService.SendAsync(user.Email, "Doğrulama E-postası", message);
                
                TempData["UserEmail"] = user.Email;
                return RedirectToAction("Web_SignIn", "User");
            }
            catch (Exception error)
            {
                ModelState.AddModelError("", "Bir hata oluştu: " + error.Message);
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Web_SignIn()
        {
            return View();
        }

        // POST: /User/SignIn


        AesSifreleme aesSifreleme = new AesSifreleme();
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Web_SignIn(string Email, string PasswordHash)
        {


            // Kullanıcıyı veritabanından e-posta ile bul
            var existingUser = _userMenager.FindByEmail(Email);

            if (existingUser != null)
            {
                // Şifre kontrolü (şifreyi hashleyerek karşılaştırmalısınız)2
                var gelenSifre = Md5Sifreleme.ComputeMD5(PasswordHash);
                var olanSifre = existingUser.PasswordHash;

                if (olanSifre == gelenSifre)
                {
                    //// Kullanıcı giriş işlemi başarılı, rolü kontrol et
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, existingUser.Email),
                        new Claim(ClaimTypes.Role, existingUser.Role) // Kullanıcının rolü
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Login");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Kimlik doğrulama
                    HttpContext.SignInAsync(claimsPrincipal);

                    HttpContext.Session.SetString("FullName", existingUser.FirstName + " " + existingUser.LastName);
                    //HttpContext.Session.SetString("RoleId", existingUser.RoleId.ToString());

                    //exiting=mevcut 
                    //< img src = "/admin/template/@profileImage" alt = "Profile" />

                    //exiting=mevcut 

                    if (existingUser.ProfilePicture==null)
                    {
                        HttpContext.Session.SetString("userProfil", "images/no-user-image.jpg");
                    }
                    else
                    {
                        HttpContext.Session.SetString("userProfil", existingUser.ProfilePicture);
                    }
                    
                    // Yönlendirme

                    var encryptedRoleId = aesSifreleme.Encrypt(existingUser.RoleUrl);

                    if (existingUser.Role == "Admin")
                    {
                        // Admin sayfasına yönlendir
                        return RedirectToAction("Index", "User", new { RoleUrl = encryptedRoleId });
                    }
                    else
                    {
                        // Kullanıcı sayfasına yönlendir
                        return RedirectToAction("UserUI", "Home", new { RoleUrl = encryptedRoleId });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
            }

            return View(); // Giriş başarısızsa aynı view'ı döndür
        }





        // BİLET İŞLEMLERİ
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SearchFlights(
     string secenek, string nereden, string nereye,
     string gidisTarihi, string dönüsTarihi,
     int yolcuSayısı, string uçuşSinifi)
        {
            string formattedGidisTarihi = DateTime.Parse(gidisTarihi).ToString("yyyy-MM-dd");
            string formattedDonusTarihi = string.IsNullOrEmpty(dönüsTarihi) ? null : DateTime.Parse(dönüsTarihi).ToString("yyyy-MM-dd");

            // Amadeus API URL'si
            var apiUrl = "https://test.api.amadeus.com/v2/shopping/flight-offers";

            // Access token alımı
            //var tokenService = new AmedusToken(); // Amadeus token alımı için bir örnek oluştur.
            //string accessToken = await tokenService.GetAccessToken(_httpClientFactory); // Token almak için asenkron bir çağrı yap.

            string accessToken = "q6RYaeMvMWxBXv9WV6Q45TAQQj8I";

            // Uçuş arama parametrelerini ayarlayın
            var queryParams = new Dictionary<string, object>
     {
         { "originLocationCode", nereden },              // Nereden
         { "destinationLocationCode", nereye },          // Nereye
         { "departureDate", formattedGidisTarihi },      // Gidiş tarihi
         { "adults", yolcuSayısı },                       // Yolcu sayısı, integer olarak tutuluyor
         { "nonStop", false },                            // Duraksız uçuşlar için, boolean olarak tutuluyor
         { "max", 250 }                                     // Maksimum uçuş teklifi, integer olarak tutuluyor
     };
            var requestUrl = $"{apiUrl}?originLocationCode={nereden}&destinationLocationCode={nereye}&departureDate={formattedGidisTarihi}&adults={yolcuSayısı}&nonStop={queryParams["nonStop"].ToString().ToLower()}&max={queryParams["max"]}";



            // API isteği için URL oluşturma
            // Dönüş tarihi, "Gidiş-Dönüş" seçeneği seçildiyse eklenir
            if (secenek == "Option2" && !string.IsNullOrEmpty(dönüsTarihi)) // Eğer dönüş tarihi varsa ve gidiş-dönüş seçeneği seçilmişse.
            {
                requestUrl += $"&returnDate={formattedDonusTarihi}"; // Dönüş tarihini sorgu parametrelerine ekle.
            }



            //using bloğu, IDisposable arayüzünü uygulayan nesneleri otomatik olarak temizler. 
            //    Örneğin, dosya veya veritabanı bağlantılarını işlem sonrası serbest bırakır.
            // API isteği
            using (var client = _httpClientFactory.CreateClient()) // HTTP istemcisi oluştur.
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken); // Authorization başlığına alınan token'ı ekle.

                var response = await client.GetAsync(requestUrl); // API'den uçuş tekliflerini almak için GET isteği yap.

                if (response.IsSuccessStatusCode) // API yanıtı başarılıysa.
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync(); // Yanıtın içeriğini JSON formatında oku.

                    // API'den dönen yanıtın yapısına göre TicketModel sınıfını kontrol edin
                    var flightResult = JsonConvert.DeserializeObject<FlightOffersResponse>(jsonResponse); // JSON verisini TicketModel sınıfına deserialize et.
                                                                                                          //var segmentCount = flightResult.segment.number.Distinct();
                    ;                    // Eğer API'den dönen sonuçlarda uçuş yoksa, kullanıcıyı bilgilendirin
                    if (flightResult?.data == null || !flightResult.data.Any()) // Eğer uçuş bilgileri yoksa.
                    {
                        ViewBag.ErrorMessage = "Uçuş bilgileri bulunamadı. Lütfen farklı kriterlerle tekrar deneyin."; // Hata mesajını ayarla.
                        return View("Web_SignIn"); // Hata sayfasına yönlendir.
                    }

                    return View("SearchFlights", flightResult); // Sonuçları "SearchFlights" görünümünde göster.
                }
                else // API yanıtı başarısızsa.
                {
                    // Hata durumu için kullanıcıya bilgilendirici bir mesaj gösterebiliriz
                    ViewBag.ErrorMessage = "Uçuş bilgileri alınamadı. Lütfen tekrar deneyin."; // Hata mesajını ayarla.
                    return View("Web_SignIn"); // Hata sayfasına yönlendir.
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Web_SignIn","User");
        }

    }


}




//public class TokenResponse
//{
//    public string AccessToken { get; set; }
//    public string TokenType { get; set; }
//    public int ExpiresIn { get; set; }
//}

