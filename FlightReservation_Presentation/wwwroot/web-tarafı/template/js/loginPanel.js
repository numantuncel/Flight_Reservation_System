const btnnn = document.querySelector('.btnnn');
const loginPanel = document.querySelector('.loginPanel');

// Panelin görünümünü değiştiren fonksiyon
function toggleAdminLogin(e) {
    // Butona tıklanırsa paneli aç veya kapa
    loginPanel.classList.toggle('hidden'); 
    window.scrollTo({
        top: 0,
        behavior: 'smooth'  // Yumuşak kaydırma efekti
    });
}

// Dışarı tıklama ile paneli kapatma
document.addEventListener('click', function(event) {
    // Eğer tıklanan yer buton değilse ve panelin içi değilse
    if (!btnnn.contains(event.target) && !loginPanel.contains(event.target)) {
        loginPanel.classList.remove('hidden'); // Paneli gizle
    }
});

// Butona tıklama olayı
btnnn.addEventListener('click', toggleAdminLogin);
document.querySelector('.img__btn').addEventListener('click', function() {
    document.querySelector('.cont').classList.toggle('s--signup');
});

function swapSelectValues() {
    const select1 = document.getElementById("sehir1");
    const select2 = document.getElementById("sehir2");
  
    // Seçili indexleri geçici değişkenlerde saklayarak takas et
    const tempIndex = select1.selectedIndex;
    select1.selectedIndex = select2.selectedIndex;
    select2.selectedIndex = tempIndex;
}


$(document).ready(function(){
    $('#datetimepicker1, #datetimepicker2').datetimepicker({
        format: 'd-m-Y', // Tarih formatı (Yıl-ay-gün)
        timepicker: false, // Sadece tarih seçimi için saat seçici devre dışı
        i18n: {
            tr: { // Türkçe çeviriler
                months: [
                    'Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran',
                    'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'
                ],
                dayOfWeek: [
                    "Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"
                ]
            }
        },
        lang: 'tr' // Dil ayarını Türkçe olarak belirle
    });
});


document.addEventListener("touchstart", function(){}, true);


