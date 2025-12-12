<template>
  <div class="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900 text-white">
    <!-- Animated background elements -->
    <div class="fixed inset-0 overflow-hidden pointer-events-none">
      <div class="absolute top-1/4 left-1/4 w-96 h-96 bg-purple-500 rounded-full mix-blend-multiply filter blur-3xl opacity-10 animate-blob" />
      <div class="absolute top-1/3 right-1/4 w-96 h-96 bg-pink-500 rounded-full mix-blend-multiply filter blur-3xl opacity-10 animate-blob animation-delay-2000" />
      <div class="absolute bottom-1/4 left-1/2 w-96 h-96 bg-blue-500 rounded-full mix-blend-multiply filter blur-3xl opacity-10 animate-blob animation-delay-4000" />
    </div>

    <!-- Sidebar -->
    <aside
      class="fixed inset-y-0 z-40 w-72 bg-slate-900/80 backdrop-blur-xl border-r border-white/10 shadow-2xl transition-transform duration-300 ease-in-out md:translate-x-0"
      :class="sidebarClasses"
    >
      <div class="flex items-center justify-between px-6 py-6 border-b border-white/10">
        <div>
          <p class="text-xs uppercase tracking-widest text-purple-400 font-semibold">{{ $t('common.appName') }}</p>
          <h1 class="text-xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent mt-1">{{ restaurantName }}</h1>
        </div>
        <button
          @click="toggleSidebar"
          class="rounded-lg p-2 text-gray-400 hover:bg-white/10 hover:text-white transition-colors md:hidden"
        >
          <svg class="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <nav class="mt-6 space-y-2 px-4">
        <NuxtLink
          v-for="item in navigation"
          :key="item.name"
          :to="item.href"
          :class="navClass(item.href)"
        >
          <div
            :class="iconClass(item.href)"
          >
            <component :is="item.icon" class="h-5 w-5" />
          </div>
          <span>{{ item.name }}</span>
        </NuxtLink>
      </nav>

      <div class="absolute bottom-0 left-0 right-0 px-4 pb-6 pt-8">
        <div class="rounded-xl bg-white/5 backdrop-blur-sm border border-white/10 p-4 text-sm hover:bg-white/10 transition-all duration-300">
          <p class="font-semibold text-white">{{ authStore.user?.name }}</p>
          <p class="text-xs text-purple-400 uppercase tracking-wide mt-1">{{ authStore.user?.role }}</p>
          <button
            @click="handleLogout"
            class="mt-4 flex w-full items-center justify-center gap-2 rounded-lg bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 px-4 py-2.5 text-sm font-medium text-white shadow-lg hover:shadow-purple-500/50 transition-all duration-200 transform hover:scale-[1.02] active:scale-[0.98]"
          >
            <svg class="h-4 w-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
            </svg>
            {{ $t('common.logout') }}
          </button>
        </div>
      </div>
    </aside>

    <!-- Mobile backdrop -->
    <Transition name="fade">
      <div
        v-if="sidebarOpen"
        class="fixed inset-0 z-30 bg-black/60 backdrop-blur-sm md:hidden"
        @click="toggleSidebar"
      ></div>
    </Transition>

    <!-- Main content -->
    <div :class="mainContainerClasses">
      <header :class="headerClasses">
        <div :class="headerLeftClasses">
          <button
            @click="toggleSidebar"
            :class="menuButtonClasses"
          >
            <svg class="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>
          <div :class="headerTextClasses">
            <p class="text-xs uppercase tracking-widest text-purple-400 font-semibold">{{ pageTitle }}</p>
            <h2 class="text-lg font-semibold text-white">{{ pageSubtitle }}</h2>
          </div>
        </div>

        <div class="flex items-center gap-4">
          <LanguageSwitcher />
          <div class="hidden sm:flex items-center gap-2 rounded-lg bg-white/10 backdrop-blur-sm border border-white/10 px-4 py-2 text-sm shadow-lg">
            <span class="font-medium text-gray-300">{{ $t('common.welcome') }}</span>
            <span class="text-transparent bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text font-semibold">{{ authStore.user?.name }}</span>
          </div>
        </div>
      </header>

      <main class="px-4 py-8 sm:px-8 relative z-10">
        <slot />
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { h } from 'vue'
import { useRestaurantStore } from '@/stores/restaurant'

const authStore = useAuthStore()
const restaurantStore = useRestaurantStore()
const router = useRouter()
const route = useRoute()
const { t, locale } = useI18n({ useScope: 'global' })

const rtlLocales = ['ar']
const sidebarOpen = ref(false)

const isRtl = computed(() => rtlLocales.includes(locale.value))

const restaurantName = computed(() => {
  const localized = restaurantStore.publicMenu?.restaurantLocalizedName
  if (localized && localized.trim().length > 0) {
    return localized
  }
  const baseName = restaurantStore.publicMenu?.restaurantName
  if (baseName && baseName.trim().length > 0) {
    return baseName
  }
  return authStore.user?.name || t('common.appName')
})

const toggleSidebar = () => {
  sidebarOpen.value = !sidebarOpen.value
}

const navigation = computed(() => [
  {
    name: t('navigation.dashboard'),
    href: '/dashboard',
    icon: () =>
      h('svg', { class: 'h-4 w-4', fill: 'none', stroke: 'currentColor', viewBox: '0 0 24 24' }, [
        h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M3 13h8V3H3v10zm10 8h8v-6h-8v6zm0-8h8V3h-8v10zM3 21h8v-6H3v6z' })
      ])
  },
  {
    name: t('navigation.categories'),
    href: '/dashboard/categories',
    icon: () =>
      h('svg', { class: 'h-4 w-4', fill: 'none', stroke: 'currentColor', viewBox: '0 0 24 24' }, [
        h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M5 4h4v4H5V4zm0 6h4v4H5v-4zm0 6h4v4H5v-4zM11 6h8M11 12h8M11 18h8' })
      ])
  },
  {
    name: 'Settings',
    href: '/dashboard/settings',
    icon: () =>
      h('svg', { class: 'h-4 w-4', fill: 'none', stroke: 'currentColor', viewBox: '0 0 24 24' }, [
        h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z' }),
        h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M15 12a3 3 0 11-6 0 3 3 0 016 0z' })
      ])
  },
  {
    name: t('navigation.qrcodes'),
    href: '/dashboard/qrcodes',
    icon: () =>
      h('svg', { class: 'h-4 w-4', fill: 'none', stroke: 'currentColor', viewBox: '0 0 24 24' }, [
        h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M4 4h6v6H4V4zm10 0h6v6h-6V4zM4 14h6v6H4v-6zm10 4h2m2 0h2m-6-4h2m2 0h2' })
      ])
  }
])

const navClass = (href: string) => [
  'group flex items-center gap-3 rounded-xl px-4 py-3 text-sm font-medium transition-all duration-200',
  route.path === href
    ? 'bg-gradient-to-r from-purple-500 to-pink-500 text-white shadow-lg shadow-purple-500/30'
    : 'text-gray-300 hover:bg-white/10 hover:text-white'
]

const iconClass = (href: string) => [
  'flex h-10 w-10 items-center justify-center rounded-lg transition-all duration-200',
  route.path === href
    ? 'bg-white/20 text-white'
    : 'bg-white/5 text-gray-400 group-hover:bg-white/10 group-hover:text-white'
]

const pageTitle = computed(() => {
  const item = navigation.value.find((link) => link.href === route.path)
  return item ? item.name : t('navigation.dashboard')
})

const pageSubtitle = computed(() => {
  switch (route.path) {
    case '/dashboard/categories':
      return t('menu.categories')
    case '/dashboard/qrcodes':
      return t('qr.title')
    case '/dashboard/templates':
      return t('navigation.templates')
    case '/dashboard/settings':
      return t('navigation.settings')
    default:
      return t('dashboard.overview')
  }
})

const handleLogout = async () => {
  await authStore.logout()
  router.push('/login')
}

const sidebarClasses = computed(() => ({
  'left-0': !isRtl.value,
  'right-0': isRtl.value,
  '-translate-x-full md:translate-x-0': !isRtl.value && !sidebarOpen.value,
  'translate-x-full md:translate-x-0': isRtl.value && !sidebarOpen.value
}))

const mainContainerClasses = computed(() => [
  isRtl.value ? 'md:pr-72 md:pl-0' : 'md:pl-72 md:pr-0',
  'transition-all'
])

const headerClasses = computed(() => [
  'sticky top-0 z-20 flex h-20 items-center justify-between border-b border-white/10 bg-slate-900/60 backdrop-blur-xl px-6 shadow-lg',
  isRtl.value ? 'flex-row-reverse' : 'flex-row'
])

const headerLeftClasses = computed(() => [
  'flex items-center gap-4',
  isRtl.value ? 'flex-row-reverse' : 'flex-row'
])

const headerTextClasses = computed(() => [
  'hidden sm:block',
  isRtl.value ? 'text-right' : 'text-left'
])

const menuButtonClasses = computed(() => [
  'rounded-lg bg-white/10 backdrop-blur-sm border border-white/10 p-2 text-gray-300 shadow-lg hover:bg-white/20 hover:text-white transition-all duration-200 md:hidden',
  isRtl.value ? 'ml-3' : 'mr-3'
])

const applyDocumentDirection = (localeCode: string) => {
  if (process.client) {
    const dir = rtlLocales.includes(localeCode) ? 'rtl' : 'ltr'
    document.documentElement.dir = dir
    document.documentElement.lang = localeCode
  }
}

onMounted(async () => {
  authStore.loadFromStorage()
  if (!authStore.isAuthenticated) {
    router.push('/login')
    return
  }

  const restaurantId = authStore.restaurantId
  if (restaurantId) {
    await restaurantStore.fetchCategories(restaurantId, locale.value)
  }

  applyDocumentDirection(locale.value)
})

watch(
  () => locale.value,
  async (newLocale, oldLocale) => {
    if (newLocale === oldLocale) {
      return
    }
    const restaurantId = authStore.restaurantId
    if (restaurantId) {
      await restaurantStore.fetchCategories(restaurantId, newLocale)
    }

    applyDocumentDirection(newLocale)
  }
)
</script>

<style scoped>
@keyframes blob {
  0%, 100% { transform: translate(0, 0) scale(1); }
  33% { transform: translate(30px, -50px) scale(1.1); }
  66% { transform: translate(-20px, 20px) scale(0.9); }
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

/* Fade transition for mobile backdrop */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>