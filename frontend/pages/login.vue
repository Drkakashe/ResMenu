<template>
  <div class="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900 text-white relative overflow-hidden flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
    <!-- Animated background elements -->
    <div class="absolute inset-0 overflow-hidden pointer-events-none">
      <div class="absolute top-1/4 left-1/4 w-96 h-96 bg-purple-500 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob" />
      <div class="absolute top-1/3 right-1/4 w-96 h-96 bg-pink-500 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob animation-delay-2000" />
      <div class="absolute bottom-1/4 left-1/2 w-96 h-96 bg-blue-500 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob animation-delay-4000" />
    </div>

    <!-- Language Switcher -->
    <div class="absolute top-6 right-6 z-50">
      <LanguageSwitcher />
    </div>

    <!-- Login Card -->
    <div class="max-w-md w-full relative z-10 animate-fade-in-up">
      <div class="bg-white/10 backdrop-blur-xl rounded-3xl shadow-2xl border border-white/20 p-8 sm:p-10 hover:border-white/30 transition-all duration-300">
        
        <!-- Logo and Title -->
        <div class="text-center mb-8">
          <div class="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-br from-purple-500 to-pink-500 rounded-2xl mb-4 transform hover:rotate-6 transition-transform duration-300 shadow-lg">
            <svg class="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
            </svg>
          </div>
          <h2 class="text-3xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent">
            {{ $t('common.appName') }}
          </h2>
          <p class="mt-2 text-white font-medium text-lg">{{ $t('auth.loginTitle') }}</p>
          <p class="text-sm text-gray-300 mt-1">{{ $t('auth.loginSubtitle') }}</p>
        </div>

        <!-- Login Form -->
        <form @submit.prevent="handleLogin" class="space-y-5">
          <div>
            <label for="email" class="block text-sm font-medium text-gray-200 mb-2">
              {{ $t('common.email') }}
            </label>
            <input
              id="email"
              v-model="form.email"
              type="email"
              required
              :placeholder="$t('auth.emailPlaceholder')"
              class="block w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl shadow-sm text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200 hover:bg-white/15 hover:border-white/30"
            />
          </div>

          <div>
            <label for="password" class="block text-sm font-medium text-gray-200 mb-2">
              {{ $t('common.password') }}
            </label>
            <input
              id="password"
              v-model="form.password"
              type="password"
              required
              :placeholder="$t('auth.passwordPlaceholder')"
              class="block w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl shadow-sm text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all duration-200 hover:bg-white/15 hover:border-white/30"
            />
          </div>

          <!-- Error Message -->
          <Transition name="slide-down">
            <div v-if="error" class="bg-red-500/20 backdrop-blur-sm border border-red-500/50 text-red-100 px-4 py-3 rounded-xl text-sm">
              {{ error }}
            </div>
          </Transition>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="loading"
            class="group relative w-full flex justify-center items-center gap-2 py-3.5 px-4 rounded-xl text-sm font-semibold text-white bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500 focus:ring-offset-slate-900 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-200 shadow-lg hover:shadow-purple-500/50 transform hover:scale-[1.02] active:scale-[0.98] overflow-hidden"
          >
            <div class="absolute inset-0 bg-gradient-to-r from-pink-500 to-purple-500 opacity-0 group-hover:opacity-100 transition-opacity duration-300" />
            <svg v-if="loading" class="relative z-10 animate-spin h-5 w-5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            <span class="relative z-10">
              {{ loading ? $t('common.loading') : $t('common.login') }}
            </span>
          </button>
        </form>

        <!-- Sign Up Link -->
        <div class="mt-6 text-center">
          <p class="text-sm text-gray-300">
            {{ $t('auth.noAccount') }}
            <NuxtLink 
              to="/register" 
              class="font-semibold text-transparent bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text hover:from-purple-300 hover:to-pink-300 transition-all"
            >
              {{ $t('auth.signUpHere') }}
            </NuxtLink>
          </p>
        </div>
      </div>

      <!-- Decorative elements -->
      <div class="absolute -bottom-6 -right-6 w-32 h-32 bg-gradient-to-br from-purple-500 to-pink-500 rounded-full opacity-20 blur-2xl animate-pulse" />
      <div class="absolute -top-6 -left-6 w-32 h-32 bg-gradient-to-br from-blue-500 to-cyan-500 rounded-full opacity-20 blur-2xl animate-pulse animation-delay-1000" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

definePageMeta({
  layout: false,
  middleware: ['guest'],
});

const authStore = useAuthStore();
const router = useRouter();
const { t } = useI18n();

const form = ref({
  email: '',
  password: '',
});

const loading = ref(false);
const error = ref('');

const handleLogin = async () => {
  loading.value = true;
  error.value = '';

  try {
    await authStore.login(form.value.email, form.value.password);
    redirectAfterAuth();
  } catch (err: any) {
    error.value = err.message || t('auth.loginError');
  } finally {
    loading.value = false;
  }
};

const redirectAfterAuth = () => {
  if (authStore.user?.role === 'Admin') {
    router.push('/admin');
    return;
  }

  router.push('/dashboard');
};

onMounted(() => {
  authStore.loadFromStorage();
  if (authStore.isAuthenticated) {
    redirectAfterAuth();
  }
});
</script>

<style scoped>
@keyframes blob {
  0%, 100% { transform: translate(0, 0) scale(1); }
  33% { transform: translate(30px, -50px) scale(1.1); }
  66% { transform: translate(-20px, 20px) scale(0.9); }
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

@keyframes pulse {
  0%, 100% { opacity: 0.2; }
  50% { opacity: 0.3; }
}

.animate-blob {
  animation: blob 7s infinite;
}

.animation-delay-1000 {
  animation-delay: 1s;
}

.animation-delay-2000 {
  animation-delay: 2s;
}

.animation-delay-4000 {
  animation-delay: 4s;
}

.animate-fade-in-up {
  animation: fadeInUp 0.7s ease-out forwards;
}

.animate-pulse {
  animation: pulse 3s ease-in-out infinite;
}

/* Slide down transition for error message */
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