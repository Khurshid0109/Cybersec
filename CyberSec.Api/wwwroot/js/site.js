// For comment feauture
document.getElementById('comment-btn').addEventListener("click", function () {
    document.getElementsByClassName('comments')[0].classList.toggle('active')
})
document.addEventListener("DOMContentLoaded", function () {
    var descriptionElements = document.querySelectorAll('.text-block');
    descriptionElements.forEach(function (element) {
        var descriptionText = element.innerHTML;
        element.innerHTML = descriptionText.replace(/\./g, '.<br><br>');
    });
});


