function openModal() {
    const modal = new bootstrap.Modal(document.getElementById('categoryModal'));
    modal.show();
}

function selectCategory(name) {
    document.getElementById('categoryNameInput').value = name;
    const modal = bootstrap.Modal.getInstance(document.getElementById('categoryModal'));
    modal.hide();
}