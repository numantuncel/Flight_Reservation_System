const numanLogo = document.querySelector('.numanLogo');
const tıklama = document.querySelector('.navbar-toggler');

tıklama.addEventListener('click', function () {
    numanLogo.classList.toggle('numanLogoActive');
});
