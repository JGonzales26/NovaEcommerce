const button = document.querySelector('[data-menu-button]');
const mobileMenu = document.querySelector('[data-mobile-menu]');

button?.addEventListener('click', () => {
  mobileMenu?.classList.toggle('hidden');
});

document.querySelectorAll('[data-confirm]').forEach((element) => {
  element.addEventListener('click', (event) => {
    if (!window.confirm(element.getAttribute('data-confirm'))) {
      event.preventDefault();
    }
  });
});
