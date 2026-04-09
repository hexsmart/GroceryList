// Index page - Grocery List management
const cart = JSON.parse(localStorage.getItem('groceryCart') || '{}');
const catCollapsed = JSON.parse(localStorage.getItem('catCollapsed') || '{}');
let catOrder = JSON.parse(localStorage.getItem('catOrder') || '[]');
let flatMode = localStorage.getItem('flatMode') === 'true';

function applyViewMode() {
    const catView = document.getElementById('category-container');
    const flatView = document.getElementById('flat-list');
    const toggleBtn = document.getElementById('view-toggle-btn');
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

function toggleRow(checkbox) {
    const row = checkbox.closest('tr');
    const id = row.dataset.id;
    const name = row.dataset.name;
    if (checkbox.checked) {
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
    document.querySelectorAll('tr[data-category="Staple"]').forEach(row => {
        const id = row.dataset.id;
        const name = row.dataset.name;
        const cb = row.querySelector('input[type="checkbox"]');
        if (cb) {
            cb.checked = true;
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

// Initialize view mode
applyViewMode();

// Restore checkboxes from cart
Object.keys(cart).forEach(id => {
    const cbs = document.querySelectorAll(`input[data-id="${id}"]`);
    cbs.forEach(cb => cb.checked = true);
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

// Apply category order from localStorage (if available)
if (catOrder.length > 0) {
    const container = document.getElementById('category-container');
    const sections = Array.from(container.querySelectorAll('[data-category]'));
    sections.sort((a, b) => {
        const catA = a.dataset.category;
        const catB = b.dataset.category;
        const indexA = catOrder.indexOf(catA);
        const indexB = catOrder.indexOf(catB);
        if (indexA === -1 && indexB === -1) return catA.localeCompare(catB);
        if (indexA === -1) return 1;
        if (indexB === -1) return -1;
        return indexA - indexB;
    });
    sections.forEach(s => container.appendChild(s));
}
