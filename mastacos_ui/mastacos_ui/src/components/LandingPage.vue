<template>
    <div class="landing-page">
        <!-- Hero Section -->
        <div class="hero-section text-center text-white d-flex align-items-center">
            <div class="container">
                <h1 class="display-3 mb-4">Welcome to Ma's Tacos</h1>
                <p class="lead mb-5">Authentic Mexican cuisine made with love and tradition</p>
                <router-link to="/menu" class="btn btn-light btn-lg">
                    <i class="fas fa-utensils me-2"></i>View Our Menu
                </router-link>
            </div>
        </div>

        <!-- About Section -->
        <div class="container my-5">
            <div class="row align-items-center">
                <div class="col-md-6">
                    <h2 class="mb-4">Our Story</h2>
                    <p class="lead">Ma's Tacos brings the authentic flavors of Mexico to your table. Our recipes have been passed down through generations, ensuring each dish captures the essence of traditional Mexican cuisine.</p>
                    <p>Using only the freshest ingredients and traditional cooking methods, we create dishes that not only satisfy your hunger but also tell a story of culture and tradition.</p>
                </div>
                <div class="col-md-6">
                    <div class="about-image rounded shadow-4"></div>
                </div>
            </div>
        </div>

        <!-- Features Section -->
        <div class="py-5">
            <div class="container">
                <div class="row g-4">
                    <div class="col-md-4">
                        <div class="card h-100 border-0 shadow-2">
                            <div class="card-body text-center">
                                <i class="fas fa-pepper-hot fa-3x text-primary mb-3"></i>
                                <h3 class="h4 mb-3">Fresh Ingredients</h3>
                                <p>We source the freshest ingredients daily to ensure quality and authentic taste in every dish.</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card h-100 border-0 shadow-2">
                            <div class="card-body text-center">
                                <i class="fas fa-clock fa-3x text-primary mb-3"></i>
                                <h3 class="h4 mb-3">Fast Service</h3>
                                <p>Quick service without compromising on quality. Perfect for your lunch break or family dinner.</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card h-100 border-0 shadow-2">
                            <div class="card-body text-center">
                                <i class="fas fa-heart fa-3x text-primary mb-3"></i>
                                <h3 class="h4 mb-3">Made with Love</h3>
                                <p>Every dish is prepared with care and attention to detail, just like Ma used to make.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Popular Items Section -->
        <div class="container my-5">
            <h2 class="text-center mb-5">Popular Menu Items</h2>
            <div class="row g-4">
                <div class="col-md-4">
                    <div class="card menu-item-card">
                        <div class="menu-item-image tacos"></div>
                        <div class="card-body">
                            <h5 class="card-title">Street Tacos</h5>
                            <p class="card-text">Authentic Mexican street tacos with your choice of meat, topped with fresh cilantro and onions.</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card menu-item-card">
                        <div class="menu-item-image quesadilla"></div>
                        <div class="card-body">
                            <h5 class="card-title">Quesadillas</h5>
                            <p class="card-text">Grilled flour tortillas stuffed with melted cheese and your choice of fillings.</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card menu-item-card">
                        <div class="menu-item-image burrito"></div>
                        <div class="card-body">
                            <h5 class="card-title">Burritos</h5>
                            <p class="card-text">Large flour tortillas filled with rice, beans, meat, and fresh toppings.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Contact Section -->
        <div class="py-5">
            <div class="container">
                <div class="row justify-content-center">
                    <div class="col-md-8 text-center">
                        <h2 class="mb-4">Visit Us Today</h2>
                        <p class="lead mb-4">Experience the authentic taste of Mexico at Ma's Tacos</p>
                        <div class="row g-4">
                            <div class="col-md-4">
                                <i class="fas fa-map-marker-alt fa-2x text-primary mb-3"></i>
                                <h5>Location</h5>
                                <p>123 Taco Street<br>Foodie City, FC 12345</p>
                            </div>
                            <div class="col-md-4">
                                <i class="fas fa-clock fa-2x text-primary mb-3"></i>
                                <h5>Hours</h5>
                                <p>Mon-Sat: 11am - 9pm<br>Sun: 12pm - 8pm</p>
                            </div>
                            <div class="col-md-4">
                                <i class="fas fa-phone fa-2x text-primary mb-3"></i>
                                <h5>Contact</h5>
                                <p>(555) 123-4567<br>info@mastacos.com</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
// Empty script block - keeping lang="ts" for consistency with project
</script>

<style scoped>
.landing-page {
    overflow-x: hidden;
    color: #fff;
}

.hero-section {
    background: linear-gradient(rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.7)),
                url('https://images.unsplash.com/photo-1565299585323-38d6b0865b47?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80');
    background-size: cover;
    background-position: center;
    height: 80vh;
    position: relative;
}

.about-section {
    background-color: rgba(0, 0, 0, 0.7);
    backdrop-filter: blur(10px);
    border: 1px solid rgba(255, 255, 255, 0.1);
    border-radius: 8px;
    padding: 2rem;
    margin: 2rem 0;
}

.about-image {
    background: url('https://images.unsplash.com/photo-1552332386-f8dd00dc2f85?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80');
    background-size: cover;
    background-position: center;
    height: 400px;
    border-radius: 8px;
    border: 1px solid rgba(255, 255, 255, 0.1);
}

.features-section {
    background-color: rgba(0, 0, 0, 0.5);
    backdrop-filter: blur(10px);
    padding: 4rem 0;
    border-top: 1px solid rgba(255, 255, 255, 0.1);
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.card {
    background-color: rgba(0, 0, 0, 0.7) !important;
    border: 1px solid rgba(255, 255, 255, 0.1) !important;
    backdrop-filter: blur(10px);
    transition: transform 0.3s ease, border-color 0.3s ease;
}

.card:hover {
    transform: translateY(-5px);
    border-color: rgba(255, 255, 255, 0.3) !important;
}

.menu-item-card {
    height: 100%;
}

.menu-item-image {
    height: 200px;
    background-size: cover;
    background-position: center;
    border-top-left-radius: 8px;
    border-top-right-radius: 8px;
}

.menu-item-image.tacos {
    background-image: url('https://images.unsplash.com/photo-1551504734-5ee1c4a1479b?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80');
}

.menu-item-image.quesadilla {
    background-image: url('https://images.unsplash.com/photo-1599974579688-8dbdd335c77f?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80');
}

.menu-item-image.burrito {
    background-image: url('https://images.unsplash.com/photo-1626700051175-6818013e1d4f?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80');
}

.contact-section {
    background-color: rgba(0, 0, 0, 0.7);
    backdrop-filter: blur(10px);
    padding: 4rem 0;
    border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.text-primary {
    color: #3b71ca !important;
}

.btn-light {
    background-color: rgba(255, 255, 255, 0.1);
    border-color: rgba(255, 255, 255, 0.2);
    color: #fff;
    backdrop-filter: blur(5px);
    transition: all 0.3s ease;
}

.btn-light:hover {
    background-color: rgba(255, 255, 255, 0.2);
    border-color: rgba(255, 255, 255, 0.4);
    color: #fff;
}

.shadow-2 {
    box-shadow: 0 .5rem 1rem rgba(0,0,0,.15);
}

.shadow-4 {
    box-shadow: 0 .5rem 2rem rgba(0,0,0,.25);
}

/* Responsive adjustments */
@media (max-width: 768px) {
    .hero-section {
        height: 60vh;
    }

    .about-image {
        height: 300px;
        margin-top: 2rem;
    }
}
</style> 