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

    addItem(productId, name, price, imageUrl, quantity) {
        const items = this.getCart();
        const existing = items.find(i => i.productId === productId);
        if (existing) {
            existing.quantity += quantity;
        } else {
            items.push({ productId, productName: name, unitPrice: price, imageUrl, quantity });
        }
        this.saveCart(items);
        this.updateBadge();
        return items;
    },

    updateQuantity(productId, quantity) {
        const items = this.getCart();
        const item = items.find(i => i.productId === productId);
        if (!item) return items;
        if (quantity <= 0) {
            return this.removeItem(productId);
        }
        item.quantity = quantity;
        this.saveCart(items);
        return items;
    },

    removeItem(productId) {
        const items = this.getCart().filter(i => i.productId !== productId);
        this.saveCart(items);
        this.updateBadge();
        return items;
    },

    clearCart() {
        localStorage.removeItem('nova_cart');
        this.updateBadge();
    },

    getCount() {
        return this.getCart().reduce((sum, i) => sum + i.quantity, 0);
    },

    updateBadge() {
        const count = this.getCount();
        const badges = document.querySelectorAll('[data-cart-badge]');
        badges.forEach(badge => {
            badge.textContent = count;
            badge.style.display = count > 0 ? '' : 'none';
        });
    }
};

document.addEventListener('DOMContentLoaded', () => {
    cartStorage.updateBadge();
});
