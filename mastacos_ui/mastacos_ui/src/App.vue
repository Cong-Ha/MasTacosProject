<template>
    <div class="container">
        <!-- Navigation links -->
        <header class="my-4">
            <nav class="navbar navbar-expand-lg navbar-light bg-light">
                <div class="container-fluid">
                    <router-link class="navbar-brand" to="/">Nav</router-link>
                    <button class="navbar-toggler"
                            type="button"
                            data-mdb-toggle="collapse"
                            data-mdb-target="#navbarNav"
                            aria-controls="navbarNav"
                            aria-expanded="false"
                            aria-label="Toggle navigation">
                        <i class="fas fa-bars"></i>
                    </button>
                    <div class="collapse navbar-collapse" id="navbarNav">
                        <ul class="navbar-nav">
                            <li class="nav-item">
                                <router-link class="nav-link" to="/">Menu</router-link>
                            </li>
                            <li class="nav-item">
                                <router-link class="nav-link" to="/admin">Admin</router-link>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
        </header>
        <router-view />
    </div>
</template>

<script setup lang="ts">

</script>

<style>
    #app {
        font-family: Roboto, Helvetica, Arial, sans-serif;
    }

    .router-link-active {
        font-weight: bold;
    }
</style>