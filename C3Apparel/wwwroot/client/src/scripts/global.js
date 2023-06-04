
document.addEventListener("DOMContentLoaded", function(){

    document.addEventListener('click', function (event) {

        if (event.target.id != 'modal-close') return;

        event.preventDefault();
        let container = document.querySelector(".modal");
        container.classList.remove("is-active");
    
    }, false);

})
