// Store page - Item selection and category reordering
const storeCatCollapsed = JSON.parse(localStorage.getItem('storeCatCollapsed') || '{}');

// Restore collapsed state
document.querySelectorAll('[id^="items-"]').forEach(el => {
    const catId = el.id.replace('items-', '');
    if (storeCatCollapsed[catId]) {
        el.classList.add('collapsed');
        const chevron = document.getElementById('chevron-' + catId);
        if (chevron) chevron.textContent = '▸';
    }
});

// Sync button label to current state
const anyExpanded = Array.from(document.querySelectorAll('.category-items')).some(el => !el.classList.contains('collapsed'));
document.getElementById('expand-collapse-label').textContent = anyExpanded ? '🔽 Collapse All' : '🔼 Expand All';

// Drag-to-reorder
Sortable.create(document.getElementById('store-container'), {
    handle: '.drag-handle',
    animation: 150,
    onEnd: function () {
        const order = Array.from(document.querySelectorAll('#store-container > [data-category]'))
            .map(el => el.dataset.category);
        
        // Save to server
        fetch('/Home/SaveCategoryOrder', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(order)
        });
        
        // Update localStorage so other pages reflect the new order
        localStorage.setItem('catOrder', JSON.stringify(order));
    }
});

function expandCollapseAll() {
    const allCollapsed = Array.from(document.querySelectorAll('.category-items')).every(el => el.classList.contains('collapsed'));
    document.querySelectorAll('.category-items').forEach(el => {
        const catId = el.id.replace('items-', '');
        const chevron = document.getElementById('chevron-' + catId);
        if (allCollapsed) {
            el.classList.remove('collapsed');
            if (chevron) chevron.textContent = '▾';
            storeCatCollapsed[catId] = false;
        } else {
            el.classList.add('collapsed');
            if (chevron) chevron.textContent = '▸';
            storeCatCollapsed[catId] = true;
        }
    });
    localStorage.setItem('storeCatCollapsed', JSON.stringify(storeCatCollapsed));
    document.getElementById('expand-collapse-label').textContent = allCollapsed ? '🔽 Collapse All' : '🔼 Expand All';
}

function toggleCollapse(catId) {
    const items = document.getElementById('items-' + catId);
    const chevron = document.getElementById('chevron-' + catId);
    const isCollapsed = items.classList.toggle('collapsed');
    chevron.textContent = isCollapsed ? '▸' : '▾';
    storeCatCollapsed[catId] = isCollapsed;
    localStorage.setItem('storeCatCollapsed', JSON.stringify(storeCatCollapsed));
}

function addItem(btn, name) {
    btn.disabled = true;
    fetch('/Home/AddItem', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(name)
    }).then(r => {
        if (r.ok) {
            btn.outerHTML = `<span class="badge bg-success">✅ Added</span>`;
        } else {
            btn.disabled = false;
        }
    });
}
