const dropArea = document.getElementById('drop-area');
const fileInput = document.getElementById('image');

fileInput.addEventListener('change', handleFiles);

function handleFiles(event) {
    const files = event.target.files;

    if (files.length > 0) {
        const reader = new FileReader();

        reader.onload = function (e) {
            const img = new Image();

            img.onload = function () {
                dropArea.style.backgroundImage = `url(${e.target.result})`;
                dropArea.style.backgroundSize = 'contain';
                dropArea.style.backgroundPosition = 'center';
                dropArea.style.backgroundRepeat = 'no-repeat';
                dropArea.style.backgroundColor = '#e6f7ff';
            };

            img.src = e.target.result;
        };

        reader.readAsDataURL(files[0]);
    }
}
