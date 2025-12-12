<template>
  <div class="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900 text-white relative overflow-hidden">
    <!-- Animated background elements -->
    <div class="absolute inset-0 overflow-hidden pointer-events-none">
      <div class="absolute top-1/4 left-1/4 w-96 h-96 bg-purple-500 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob" />
      <div class="absolute top-1/3 right-1/4 w-96 h-96 bg-pink-500 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob animation-delay-2000" />
      <div class="absolute bottom-1/4 left-1/2 w-96 h-96 bg-blue-500 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob animation-delay-4000" />
    </div>

    <!-- Navigation -->
    <nav :class="['fixed w-full z-50 transition-all duration-300', isScrolled ? 'bg-slate-900/80 backdrop-blur-lg shadow-lg' : 'bg-transparent']">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16 items-center">
          <div class="flex items-center space-x-2 group cursor-pointer">
            <svg class="w-7 h-7 text-purple-400 group-hover:text-pink-400 transition-colors" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 3l14 9-14 9V3z" />
            </svg>
            <h1 class="text-2xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent">
              Menufy
            </h1>
          </div>
          
          <!-- Desktop menu -->
          <div class="hidden md:flex items-center space-x-4">
            <NuxtLink to="/login" class="text-gray-300 hover:text-white px-4 py-2 rounded-lg transition-all hover:bg-white/10">
              Login
            </NuxtLink>
            <NuxtLink to="/register" class="bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 text-white px-6 py-2 rounded-lg font-medium transition-all transform hover:scale-105 shadow-lg hover:shadow-purple-500/50">
              Get Started
            </NuxtLink>
          </div>

          <!-- Mobile menu button -->
          <button class="md:hidden" @click="mobileMenuOpen = !mobileMenuOpen">
            <svg v-if="!mobileMenuOpen" class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
            <svg v-else class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
      </div>

      <!-- Mobile menu -->
      <Transition name="slide-down">
        <div v-if="mobileMenuOpen" class="md:hidden bg-slate-900/95 backdrop-blur-lg border-t border-white/10">
          <div class="px-4 py-4 space-y-3">
            <NuxtLink to="/login" class="block text-gray-300 hover:text-white px-4 py-2 rounded-lg transition-all hover:bg-white/10">
              Login
            </NuxtLink>
            <NuxtLink to="/register" class="block bg-gradient-to-r from-purple-500 to-pink-500 text-white px-4 py-2 rounded-lg font-medium text-center">
              Get Started
            </NuxtLink>
          </div>
        </div>
      </Transition>
    </nav>

    <!-- Hero Section -->
    <div class="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 pt-32 pb-20">
      <div class="text-center">
        <div class="inline-block mb-6 animate-fade-in-up" style="animation-delay: 0ms">
          <span class="inline-flex items-center space-x-2 bg-gradient-to-r from-purple-500/20 to-pink-500/20 border border-purple-500/30 rounded-full px-4 py-2 text-sm backdrop-blur-sm">
            <svg class="w-4 h-4 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 3l14 9-14 9V3z" />
            </svg>
            <!-- <span>Trusted by 1000+ restaurants</span> -->
          </span>
        </div>
        
        <h2 class="text-5xl md:text-7xl font-extrabold mb-6 animate-fade-in-up" style="animation-delay: 100ms">
          Digital Menus
          <br />
          <span class="bg-gradient-to-r from-purple-400 via-pink-400 to-purple-400 bg-clip-text text-transparent animate-gradient bg-300">
            Made Simple
          </span>
        </h2>
        
        <p class="text-xl text-gray-300 mb-10 max-w-2xl mx-auto animate-fade-in-up" style="animation-delay: 200ms">
          Create beautiful digital menus for your restaurant. Share them instantly with QR codes. 
          No app required for customers.
        </p>
        
        <div class="flex flex-col sm:flex-row justify-center gap-4 animate-fade-in-up" style="animation-delay: 300ms">
          <NuxtLink to="/register" class="group relative bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 text-white px-8 py-4 rounded-xl text-lg font-semibold shadow-2xl shadow-purple-500/50 transform hover:scale-105 transition-all overflow-hidden">
            <span class="relative z-10 flex items-center justify-center">
              Start Free Trial
              <svg class="w-5 h-5 ml-2 group-hover:translate-x-1 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
              </svg>
            </span>
            <div class="absolute inset-0 bg-gradient-to-r from-pink-500 to-purple-500 opacity-0 group-hover:opacity-100 transition-opacity" />
          </NuxtLink>
          
          <button class="bg-white/10 backdrop-blur-sm hover:bg-white/20 text-white px-8 py-4 rounded-xl text-lg font-semibold border border-white/20 transform hover:scale-105 transition-all">
            View Demo
          </button>
        </div>
      </div>

      <!-- Features -->
      <div class="mt-32 grid grid-cols-1 md:grid-cols-3 gap-8">
        <div
          v-for="(feature, index) in features"
          :key="index"
          class="group relative animate-fade-in-up"
          :style="{ animationDelay: `${(index + 4) * 100}ms` }"
          @mouseenter="hoveredCard = index"
          @mouseleave="hoveredCard = null"
        >
          <div class="relative bg-white/5 backdrop-blur-lg p-8 rounded-2xl border border-white/10 hover:border-white/20 transition-all transform hover:scale-105 hover:-translate-y-2 duration-300">
            <!-- Gradient overlay on hover -->
            <div :class="['absolute inset-0 rounded-2xl transition-opacity duration-300 bg-gradient-to-br', feature.gradient, 'opacity-0 group-hover:opacity-10']" />
            
            <!-- Icon with gradient -->
            <div :class="['inline-flex p-3 rounded-xl mb-4 transform group-hover:rotate-6 transition-transform duration-300 bg-gradient-to-br', feature.gradient]">
              <div v-html="feature.icon" class="w-8 h-8" />
            </div>
            
            <h3 class="text-2xl font-bold mb-3 group-hover:text-transparent group-hover:bg-gradient-to-r group-hover:bg-clip-text group-hover:from-purple-400 group-hover:to-pink-400 transition-all">
              {{ feature.title }}
            </h3>
            
            <p class="text-gray-400 group-hover:text-gray-300 transition-colors">
              {{ feature.description }}
            </p>

            <!-- Animated corner accent -->
            <div :class="['absolute top-0 right-0 w-20 h-20 blur-2xl transition-opacity duration-300 rounded-full bg-gradient-to-br', feature.gradient, 'opacity-0 group-hover:opacity-20']" />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'

const isScrolled = ref(false)
const mobileMenuOpen = ref(false)
const hoveredCard = ref<number | null>(null)

const features = [
  {
    icon: '<svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v1m6 11h2m-6 0h-2v4m0-11v3m0 0h.01M12 12h4.01M16 20h4M4 12h4m12 0h.01M5 8h2a1 1 0 001-1V5a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1zm12 0h2a1 1 0 001-1V5a1 1 0 00-1-1h-2a1 1 0 00-1 1v2a1 1 0 001 1zM5 20h2a1 1 0 001-1v-2a1 1 0 00-1-1H5a1 1 0 00-1 1v2a1 1 0 001 1z" /></svg>',
    title: "QR Code Menus",
    description: "Generate QR codes that customers can scan to view your menu instantly.",
    gradient: "from-purple-500 to-pink-500"
  },
  {
    icon: '<svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>',
    title: "Easy Management",
    description: "Update your menu items, prices, and photos in real-time from any device.",
    gradient: "from-blue-500 to-cyan-500"
  },
  {
    icon: '<svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 21a4 4 0 01-4-4V5a2 2 0 012-2h4a2 2 0 012 2v12a4 4 0 01-4 4zm0 0h12a2 2 0 002-2v-4a2 2 0 00-2-2h-2.343M11 7.343l1.657-1.657a2 2 0 012.828 0l2.829 2.829a2 2 0 010 2.828l-8.486 8.485M7 17h.01" /></svg>',
    title: "Beautiful Design",
    description: "Professional, mobile-friendly menus that make your dishes look amazing.",
    gradient: "from-orange-500 to-red-500"
  }
]

const handleScroll = () => {
  isScrolled.value = window.scrollY > 20
}

onMounted(() => {
  window.addEventListener('scroll', handleScroll)
})

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll)
})
</script>

<style scoped>
@keyframes blob {
  0%, 100% { transform: translate(0, 0) scale(1); }
  33% { transform: translate(30px, -50px) scale(1.1); }
  66% { transform: translate(-20px, 20px) scale(0.9); }
}

@keyframes gradient {
  0%, 100% { background-position: 0% 50%; }
  50% { background-position: 100% 50%; }
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.animate-blob {
  animation: blob 7s infinite;
}

.animation-delay-2000 {
  animation-delay: 2s;
}

.animation-delay-4000 {
  animation-delay: 4s;
}

.animate-gradient {
  animation: gradient 3s ease infinite;
}

.bg-300 {
  background-size: 300% 300%;
}

.animate-fade-in-up {
  animation: fadeInUp 0.7s ease-out forwards;
  opacity: 0;
}

/* Slide down transition for mobile menu */
.slide-down-enter-active,
.slide-down-leave-active {
  transition: all 0.3s ease;
}

.slide-down-enter-from {
  opacity: 0;
  transform: translateY(-10px);
}

.slide-down-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}
</style>