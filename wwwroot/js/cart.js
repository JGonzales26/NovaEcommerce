const cartStorage = {
    getCart() {
        try {
            const data = localStorage.getItem('nova_cart');
            return data ? JSON.parse(data) : [];
        } catch {
            return [];
        }
    },

    saveCart(items) {
        localStorage.setItem('nova_cart', JSON.stringify(items));
    },

    addItem(productId, name, price, imageUrl, quantity, stock) {
        const items = this.getCart();
        const existing = items.find(i => i.productId === productId);
        const currentQty = existing ? existing.quantity : 0;
        const maxStock = parseInt(stock) || 0;

        if (currentQty + quantity > maxStock) {
            this.showToast('Stock insuficiente. Solo hay ' + maxStock + ' disponibles.', 'error');
            return false;
        }

        if (existing) {
            existing.quantity += quantity;
        } else {
            items.push({ productId, productName: name, unitPrice: parseFloat(price), imageUrl, quantity, stock: maxStock });
        }

        this.saveCart(items);
        this.updateBadge();
        this.renderSidebar();
        this.showToast(name + ' agregado al carrito', 'success');
        return true;
    },

    updateQuantity(productId, quantity) {
        const items = this.getCart();
        const item = items.find(i => i.productId === productId);
        if (!item) return items;

        if (quantity > item.stock) {
            this.showToast('Stock insuficiente. Maximo ' + item.stock + ' disponibles.', 'error');
            return items;
        }

        if (quantity <= 0) {
            const result = this.removeItem(productId);
            this.renderSidebar();
            return result;
        }

        item.quantity = quantity;
        this.saveCart(items);
        this.renderSidebar();
        this.updateBadge();
        return items;
    },

    removeItem(productId) {
        const items = this.getCart().filter(i => i.productId !== productId);
        this.saveCart(items);
        this.updateBadge();
        this.renderSidebar();
        return items;
    },

    clearCart() {
        localStorage.removeItem('nova_cart');
        this.updateBadge();
        this.renderSidebar();
    },

    getCount() {
        return this.getCart().reduce((sum, i) => sum + i.quantity, 0);
    },

    getTotal() {
        return this.getCart().reduce((sum, i) => sum + (i.unitPrice * i.quantity), 0);
    },

    updateBadge() {
        const count = this.getCount();
        const badges = document.querySelectorAll('[data-cart-badge]');
        badges.forEach(badge => {
            badge.textContent = count;
            badge.style.display = count > 0 ? '' : 'none';
        });
    },

    showToast(message, type) {
        const existing = document.querySelector('.cart-toast');
        if (existing) existing.remove();

        const toast = document.createElement('div');
        const bgColor = type === 'error' ? 'bg-red-600' : 'bg-emerald-600';
        toast.className = 'cart-toast fixed bottom-6 right-6 z-[9999] transform translate-y-4 opacity-0 transition-all duration-300 rounded-lg px-5 py-3 text-sm font-medium text-white shadow-lg ' + bgColor;
        toast.textContent = message;
        document.body.appendChild(toast);

        requestAnimationFrame(() => {
            toast.classList.remove('translate-y-4', 'opacity-0');
        });

        setTimeout(() => {
            toast.classList.add('translate-y-4', 'opacity-0');
            setTimeout(() => toast.remove(), 300);
        }, 3000);
    },

    openSidebar() {
        const sidebar = document.getElementById('cart-sidebar');
        const overlay = document.getElementById('cart-overlay');
        if (sidebar) sidebar.classList.add('open');
        if (overlay) overlay.classList.add('open');
        document.body.classList.add('overflow-hidden');
    },

    closeSidebar() {
        const sidebar = document.getElementById('cart-sidebar');
        const overlay = document.getElementById('cart-overlay');
        if (sidebar) sidebar.classList.remove('open');
        if (overlay) overlay.classList.remove('open');
        document.body.classList.remove('overflow-hidden');
    },

    toggleSidebar() {
        const sidebar = document.getElementById('cart-sidebar');
        if (sidebar && sidebar.classList.contains('open')) {
            this.closeSidebar();
        } else {
            this.openSidebar();
        }
    },

    renderSidebar() {
        const items = this.getCart();
        const container = document.getElementById('cart-sidebar-items');
        const empty = document.getElementById('cart-sidebar-empty');
        const content = document.getElementById('cart-sidebar-content');
        const footer = document.getElementById('cart-sidebar-footer');
        const count = document.getElementById('cart-sidebar-count');
        const total = document.getElementById('cart-sidebar-total');
        const headerCount = document.getElementById('cart-sidebar-header-count');

        if (!container) return;

        if (headerCount) {
            const n = this.getCount();
            headerCount.textContent = n + (n === 1 ? ' producto' : ' productos');
        }

        if (items.length === 0) {
            empty.style.display = '';
            content.style.display = 'none';
            footer.style.display = 'none';
            return;
        }

        empty.style.display = 'none';
        content.style.display = '';
        footer.style.display = '';

        container.innerHTML = items.map(item => {
            const subtotal = (item.unitPrice * item.quantity).toFixed(2);
            return `
                <div class="flex gap-3 border-b border-zinc-100 pb-3">
                    <img class="h-16 w-16 flex-shrink-0 rounded-md object-cover" src="${item.imageUrl}" alt="${item.productName}" />
                    <div class="flex flex-1 flex-col justify-between min-w-0">
                        <div>
                            <p class="truncate text-sm font-medium">${item.productName}</p>
                            <p class="text-xs text-zinc-500">S/ ${item.unitPrice.toFixed(2)}</p>
                        </div>
                        <div class="flex items-center gap-2">
                            <div class="flex items-center rounded-md border border-zinc-200">
                                <button onclick="cartStorage.updateQuantity(${item.productId}, ${item.quantity - 1});" class="px-2 py-0.5 text-sm hover:bg-zinc-100">-</button>
                                <span class="px-2 py-0.5 text-sm">${item.quantity}</span>
                                <button onclick="cartStorage.updateQuantity(${item.productId}, ${item.quantity + 1});" class="px-2 py-0.5 text-sm hover:bg-zinc-100">+</button>
                            </div>
                            <span class="ml-auto text-xs font-medium">S/ ${subtotal}</span>
                        </div>
                    </div>
                </div>`;
        }).join('');

        if (count) count.textContent = this.getCount();
        if (total) total.textContent = 'S/ ' + this.getTotal().toFixed(2);
    }
};

document.addEventListener('DOMContentLoaded', () => {
    cartStorage.updateBadge();
    cartStorage.renderSidebar();

    const overlay = document.getElementById('cart-overlay');
    if (overlay) {
        overlay.addEventListener('click', () => cartStorage.closeSidebar());
    }

    const cartLink = document.querySelector('[data-cart-toggle]');
    if (cartLink) {
        cartLink.addEventListener('click', (e) => {
            e.preventDefault();
            cartStorage.renderSidebar();
            cartStorage.toggleSidebar();
        });
    }
});
