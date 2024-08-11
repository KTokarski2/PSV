function submitDeletePhotoForm(photoElement) {
    const form = photoElement.nextElementSibling;
    if (form && form.classList.contains('delete-photo-form')) {
        const confirmed = confirm('Czy chcesz usunąć to zdjęcie?')
        if (confirmed) {
            form.submit();
        }
    }
}

function submitUploadPhotoForm() {
    const fileInput = document.getElementById('fileInput');
    const form = document.getElementById('uploadForm');
    
    if (fileInput.files.length > 0) {
        form.submit();
    }
}