const headerProfileBtn = document.querySelector(".header__profile-btn")
const headerProfileNav = document.querySelector(".header__popup-window")

headerProfileBtn.addEventListener("click", () => {
    headerProfileNav.classList.toggle("active")
})