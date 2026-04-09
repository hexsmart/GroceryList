// Index page - Grocery List management
const cart = JSON.parse(localStorage.getItem('groceryCart') || '{}');
const catCollapsed = JSON.parse(localStorage.getItem('catCollapsed') || '{}');
let catOrder = JSON.parse(localStorage.getItem('catOrder') || '[]');
let flatMode = localStorage.getItem('flatMode') === 'true';

function applyViewMode() {
    const catView = document.getElementById('category-container');
    const flatView = document.getElementById('flat-list');
    const toggleBtn = document.getElementById('view-toggle-btn');
    
    if (!catView || !flatView || !toggleBtn) {
        return;
    }
    
    if (flatMode) {
        catView.style.display = 'none';
        flatView.style.display = '';
        toggleBtn.textContent = '🗂️ Category View';
    } else {
        catView.style.display = '';
        flatView.style.display = 'none';
        toggleBtn.textContent = '📋 Flat View';
    }
}

function toggleViewMode() {
    flatMode = !flatMode;
    localStorage.setItem('flatMode', flatMode);
    applyViewMode();
}

function toggleCollapse(catId) {
    const items = document.getElementById('items-' + catId);
    const chevron = document.getElementById('chevron-' + catId);
    const isCollapsed = items.classList.toggle('collapsed');
    chevron.textContent = isCollapsed ? '▸' : '▾';
    catCollapsed[catId] = isCollapsed;
    localStorage.setItem('catCollapsed', JSON.stringify(catCollapsed));
}

function toggleRow(row) {
    const checkbox = row.querySelector('.row-checkbox');
    const id = row.dataset.id;
    const name = row.dataset.name;
    const isSelected = row.classList.toggle('selected');
    
    if (checkbox) {
        checkbox.checked = isSelected;
    }
    
    if (isSelected) {
        cart[id] = name;
    } else {
        delete cart[id];
    }
    localStorage.setItem('groceryCart', JSON.stringify(cart));
}

function removeFromCart(id, name) {
    delete cart[id];
    localStorage.setItem('groceryCart', JSON.stringify(cart));
    
    // Uncheck both the category and flat checkboxes
    const catCb = document.querySelector(`#category-container input[data-id="${id}"]`);
    const flatCb = document.querySelector(`#flat-list input[data-id="${id}"]`);
    if (catCb) catCb.checked = false;
    if (flatCb) flatCb.checked = false;
}

function deselectAll() {
    Object.keys(cart).forEach(id => delete cart[id]);
    localStorage.setItem('groceryCart', JSON.stringify(cart));
    document.querySelectorAll('input[type="checkbox"]').forEach(cb => cb.checked = false);
}

function selectStaples() {
    document.querySelectorAll('.grocery-row[data-category="Staple"]').forEach(row => {
        const id = row.dataset.id;
        const name = row.dataset.name;
        const cb = row.querySelector('.row-checkbox');
        if (cb) {
            cb.checked = true;
            row.classList.add('selected');
            cart[id] = name;
        }
    });
    localStorage.setItem('groceryCart', JSON.stringify(cart));
}

function updateCategory(formId) {
    const form = document.getElementById(formId);
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    fetch(form.action, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    }).then(r => {
        if (r.ok) {
            // Update the data-category attribute for all rows with this ID
            const rowId = data.id;
            const newCat = data.category;
            document.querySelectorAll(`tr[data-id="${rowId}"]`).forEach(row => {
                row.dataset.category = newCat;
            });
        }
    });
}

// Wait for DOM to be ready before initializing
document.addEventListener('DOMContentLoaded', function() {
    // Initialize view mode
    applyViewMode();

    // Restore checkboxes and selections from cart
    Object.keys(cart).forEach(id => {
        const rows = document.querySelectorAll(`.grocery-row[data-id="${id}"]`);
        rows.forEach(row => {
            const cb = row.querySelector('.row-checkbox');
            if (cb) {
                cb.checked = true;
                row.classList.add('selected');
            }
        });
    });

    // Restore collapsed state
    document.querySelectorAll('[id^="items-"]').forEach(el => {
        const catId = el.id.replace('items-', '');
        if (catCollapsed[catId]) {
            el.classList.add('collapsed');
            const chevron = document.getElementById('chevron-' + catId);
            if (chevron) chevron.textContent = '▸';
        }
    });
});
