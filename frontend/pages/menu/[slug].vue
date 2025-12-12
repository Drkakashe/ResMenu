<template>
  <div class="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900 text-white">
    <!-- Animated background elements -->
    <div class="fixed inset-0 overflow-hidden pointer-events-none">
      <div class="absolute top-1/4 left-1/4 w-96 h-96 bg-purple-500 rounded-full mix-blend-multiply filter blur-3xl opacity-10 animate-blob" />
      <div class="absolute top-1/3 right-1/4 w-96 h-96 bg-pink-500 rounded-full mix-blend-multiply filter blur-3xl opacity-10 animate-blob animation-delay-2000" />
      <div class="absolute bottom-1/4 left-1/2 w-96 h-96 bg-blue-500 rounded-full mix-blend-multiply filter blur-3xl opacity-10 animate-blob animation-delay-4000" />
    </div>

    <!-- Language Switcher -->
    <div class="absolute top-4 right-4 z-50">
      <LanguageSwitcher />
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="flex items-center justify-center min-h-screen relative z-10">
      <div class="text-center space-y-4">
        <div class="inline-flex h-16 w-16 items-center justify-center rounded-full border-4 border-purple-500/30 border-t-purple-500 animate-spin"></div>
        <p class="text-lg font-semibold text-white">{{ t('common.loading') }}</p>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="flex items-center justify-center min-h-screen px-4 relative z-10">
      <div class="bg-red-500/20 backdrop-blur-xl border border-red-500/30 shadow-2xl rounded-2xl px-8 py-6 text-center max-w-md">
        <svg class="w-16 h-16 mx-auto mb-4 text-red-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
        </svg>
        <p class="text-red-300 text-xl font-bold mb-2">{{ error }}</p>
        <p class="text-gray-400">{{ t('messages.errorOccurred') }}</p>
      </div>
    </div>

    <!-- Menu Content -->
    <div v-else-if="menu" class="relative z-10">
      <!-- Restaurant Header -->
      <header class="header-section relative overflow-hidden py-12 sm:py-16 md:py-20 backdrop-blur-sm bg-white/5 border-b border-white/10">
        <div class="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8">
          <div class="flex flex-col items-center text-center gap-6 sm:gap-8">
            <!-- Logo -->
            <div v-if="menu.logoUrl" class="logo-container relative">
              <div class="absolute inset-0 bg-gradient-to-r from-purple-500 to-pink-500 rounded-2xl blur-xl opacity-50 animate-pulse"></div>
              <img
                :src="menu.logoUrl"
                :alt="menu.restaurantLocalizedName"
                class="relative w-24 h-24 sm:w-32 sm:h-32 md:w-40 md:h-40 rounded-2xl object-cover shadow-2xl ring-4 ring-purple-500/50 bg-white/10 backdrop-blur-sm transition-all duration-500 hover:scale-110 hover:rotate-3 hover:ring-purple-500"
              />
            </div>

            <!-- Restaurant Name -->
            <div class="space-y-4 sm:space-y-6">
              <h1 
                v-if="menu.restaurantLocalizedName || menu.restaurantName"
                class="restaurant-name text-4xl sm:text-5xl md:text-6xl lg:text-7xl font-bold tracking-tight bg-gradient-to-r from-purple-400 via-pink-400 to-purple-400 bg-clip-text text-transparent bg-size-200 animate-gradient"
              >
                {{ menu.restaurantLocalizedName || menu.restaurantName }}
              </h1>
              <h1 
                v-else
                class="restaurant-name text-4xl sm:text-5xl md:text-6xl lg:text-7xl font-bold tracking-tight text-white"
              >
                Restaurant
              </h1>

              <!-- Decorative Line -->
              <div class="decorative-line w-24 h-1.5 mx-auto rounded-full bg-gradient-to-r from-purple-500 via-pink-500 to-purple-500"></div>
            </div>

            <!-- Address -->
            <div v-if="menu.address" class="flex items-center gap-2 text-base sm:text-lg text-gray-300">
              <svg class="w-5 h-5 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
              </svg>
              <span>{{ menu.address }}</span>
            </div>

            <!-- Contact Info -->
            <div v-if="menu.contactPhone || menu.contactEmail" class="flex flex-wrap items-center justify-center gap-4">
              <a
                v-if="menu.contactPhone"
                :href="`tel:${menu.contactPhone}`"
                class="flex items-center gap-2 px-4 py-2 rounded-xl bg-white/10 backdrop-blur-sm border border-white/20 hover:bg-white/20 transition-all text-gray-300"
              >
                <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                  <path d="M2 3a1 1 0 011-1h2.153a1 1 0 01.986.836l.74 4.435a1 1 0 01-.54 1.06l-1.548.773a11.037 11.037 0 006.105 6.105l.774-1.548a1 1 0 011.059-.54l4.435.74a1 1 0 01.836.986V17a1 1 0 01-1 1h-2C7.82 18 2 12.18 2 5V3z" />
                </svg>
                <span>{{ menu.contactPhone }}</span>
              </a>
              <a
                v-if="menu.contactEmail"
                :href="`mailto:${menu.contactEmail}`"
                class="flex items-center gap-2 px-4 py-2 rounded-xl bg-white/10 backdrop-blur-sm border border-white/20 hover:bg-white/20 transition-all text-gray-300"
              >
                <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                  <path d="M2.003 5.884L10 9.882l7.997-3.998A2 2 0 0016 4H4a2 2 0 00-1.997 1.884z" />
                  <path d="M18 8.118l-8 4-8-4V14a2 2 0 002 2h12a2 2 0 002-2V8.118z" />
                </svg>
                <span>{{ menu.contactEmail }}</span>
              </a>
            </div>
          </div>
        </div>
      </header>

      <!-- Search Bar -->
      <div v-if="displaySettings.enableSearch" class="search-bar sticky top-0 z-40 backdrop-blur-xl bg-slate-900/90 border-b border-white/10 shadow-lg shadow-purple-500/10">
        <div class="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-4 sm:py-5">
          <div class="relative group">
            <div class="absolute inset-y-0 left-0 pl-4 sm:pl-5 flex items-center pointer-events-none transition-transform group-focus-within:scale-110">
              <svg class="h-5 h-5 sm:h-6 sm:w-6 text-gray-400 group-focus-within:text-purple-400 transition-colors" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
            </div>
            <input
              v-model="searchQuery"
              type="text"
              :placeholder="t('menu.searchPlaceholder') || 'Search menu items...'"
              class="w-full pl-12 sm:pl-14 pr-12 py-3 sm:py-4 text-base sm:text-lg rounded-xl sm:rounded-2xl border-2 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-purple-500/50 focus:bg-white/15 transition-all duration-300 focus:shadow-lg focus:shadow-purple-500/20"
            />
            <Transition name="fade-scale-quick">
              <button
                v-if="searchQuery"
                @click="searchQuery = ''"
                class="absolute inset-y-0 right-0 pr-4 sm:pr-5 flex items-center hover:scale-110 active:scale-95 transition-transform"
              >
                <div class="p-1.5 rounded-full bg-red-500/20 hover:bg-red-500/30">
                  <svg class="h-5 h-5 sm:h-6 sm:w-6 text-red-400 hover:text-red-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M6 18L18 6M6 6l12 12" />
                  </svg>
                </div>
              </button>
            </Transition>
          </div>

          <!-- Search Results Count -->
          <Transition name="slide-fade">
            <div v-if="searchQuery" class="mt-3 sm:mt-4 text-sm sm:text-base flex items-center justify-between">
              <span class="text-gray-300 flex items-center gap-2">
                <span class="px-2.5 py-1 bg-purple-500/20 rounded-lg border border-purple-500/30">
                  <span class="text-purple-400 font-bold">{{ filteredItemsCount }}</span>
                </span>
                <span>{{ filteredItemsCount === 1 ? 'item' : 'items' }} found</span>
              </span>
              <button
                @click="searchQuery = ''"
                class="px-3 py-1.5 bg-gradient-to-r from-purple-500/20 to-pink-500/20 hover:from-purple-500/30 hover:to-pink-500/30 text-purple-400 hover:text-pink-400 font-medium rounded-lg border border-purple-500/30 transition-all active:scale-95"
              >
                Clear ✕
              </button>
            </div>
          </Transition>
        </div>
      </div>

      <!-- Menu Categories -->
      <main class="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-8 sm:py-12">
        <!-- Categories -->
        <div v-if="filteredCategories.length > 0" class="space-y-12 sm:space-y-16">
          <div v-for="(category, categoryIndex) in filteredCategories" :key="category.id" class="category-section space-y-6" :style="{ animationDelay: `${categoryIndex * 100}ms` }">
            <!-- Category Header -->
            <div class="flex items-center gap-3 sm:gap-4 group">
              <div class="p-2 sm:p-3 rounded-xl bg-gradient-to-br from-purple-500/20 to-pink-500/20 border border-purple-500/30 group-hover:scale-110 transition-transform duration-300">
                <svg class="w-5 h-5 sm:w-6 sm:h-6 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
                </svg>
              </div>
              <h2 class="text-2xl sm:text-3xl md:text-4xl font-bold bg-gradient-to-r from-purple-400 via-pink-400 to-purple-400 bg-clip-text text-transparent animate-gradient bg-size-200">
                {{ category.localizedName || category.name }}
              </h2>
              <div class="flex-1 h-px bg-gradient-to-r from-purple-500/50 via-pink-500/50 to-transparent"></div>
            </div>

            <!-- Items Grid -->
            <div v-if="category.items.length > 0" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6">
              <div
                v-for="(item, index) in category.items"
                :key="item.id"
                @click="openItemDetail(item, category)"
                class="menu-item-card bg-white/5 backdrop-blur-sm border border-white/10 rounded-2xl p-4 sm:p-5 hover:bg-white/10 hover:border-purple-500/50 hover:shadow-2xl hover:shadow-purple-500/20 transition-all duration-300 cursor-pointer group active:scale-95"
                :style="{ animationDelay: `${index * 50}ms` }"
              >
                <!-- Item Image -->
                <div v-if="displaySettings.showImages && item.imageUrl" class="mb-4 overflow-hidden rounded-xl shadow-lg relative">
                  <div class="absolute inset-0 bg-gradient-to-t from-black/60 via-transparent to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300 z-10"></div>
                  <img
                    :src="item.imageUrl"
                    :alt="item.localizedName || item.name"
                    class="w-full h-48 sm:h-56 object-cover group-hover:scale-110 transition-transform duration-500 ease-out"
                  />
                  <div v-if="!item.isAvailable" class="absolute top-3 right-3 px-3 py-1.5 text-xs font-bold bg-red-500 text-white rounded-full shadow-lg z-20 animate-pulse">
                    Unavailable
                  </div>
                </div>

                <!-- Item Info -->
                <div class="space-y-3">
                  <div class="flex items-start justify-between gap-2">
                    <h3 class="text-lg sm:text-xl font-bold text-white group-hover:text-transparent group-hover:bg-gradient-to-r group-hover:from-purple-400 group-hover:to-pink-400 group-hover:bg-clip-text transition-all duration-300 leading-tight">
                      {{ item.localizedName || item.name }}
                    </h3>
                    <div v-if="!item.isAvailable && !displaySettings.showImages" class="flex-shrink-0 px-2 py-1 text-xs font-medium bg-red-500/20 text-red-300 rounded-lg border border-red-500/30 animate-pulse">
                      Unavailable
                    </div>
                  </div>

                  <p v-if="displaySettings.showDescriptions && (item.localizedDescription || item.description)"
                     class="text-sm sm:text-base text-gray-400 line-clamp-2 group-hover:text-gray-300 transition-colors">
                    {{ item.localizedDescription || item.description }}
                  </p>

                  <div v-if="displaySettings.showPrices" class="flex items-center justify-between pt-2 border-t border-white/10">
                    <span class="text-xl sm:text-2xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent group-hover:scale-110 transition-transform duration-300 inline-block">
                      {{ formatPrice(item.price) }}
                    </span>
                    <button class="p-2 rounded-full bg-purple-500/20 text-purple-400 group-hover:bg-purple-500 group-hover:text-white group-hover:rotate-0 -rotate-45 transition-all duration-300">
                      <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M9 5l7 7-7 7" />
                      </svg>
                    </button>
                  </div>
                </div>
              </div>
            </div>

            <!-- Subcategories -->
            <div v-if="category.children && category.children.length > 0" class="ml-0 sm:ml-4 space-y-8 mt-8">
              <div v-for="subcategory in category.children" :key="subcategory.id" class="space-y-6">
                <div class="flex items-center gap-4">
                  <h3 class="text-2xl sm:text-3xl font-bold text-purple-300">
                    {{ subcategory.localizedName || subcategory.name }}
                  </h3>
                  <div class="flex-1 h-px bg-gradient-to-r from-purple-400/30 to-transparent"></div>
                </div>

                <div v-if="subcategory.items.length > 0" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6">
                  <div
                    v-for="(item, index) in subcategory.items"
                    :key="item.id"
                    @click="openItemDetail(item, subcategory)"
                    class="menu-item-card bg-white/5 backdrop-blur-sm border border-white/10 rounded-2xl p-4 sm:p-5 hover:bg-white/10 hover:border-purple-500/50 hover:shadow-2xl hover:shadow-purple-500/20 transition-all duration-300 cursor-pointer group active:scale-95"
                    :style="{ animationDelay: `${index * 50}ms` }"
                  >
                    <div v-if="displaySettings.showImages && item.imageUrl" class="mb-4 overflow-hidden rounded-xl shadow-lg relative">
                      <div class="absolute inset-0 bg-gradient-to-t from-black/60 via-transparent to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300 z-10"></div>
                      <img
                        :src="item.imageUrl"
                        :alt="item.localizedName || item.name"
                        class="w-full h-48 sm:h-56 object-cover group-hover:scale-110 transition-transform duration-500 ease-out"
                      />
                      <div v-if="!item.isAvailable" class="absolute top-3 right-3 px-3 py-1.5 text-xs font-bold bg-red-500 text-white rounded-full shadow-lg z-20 animate-pulse">
                        Unavailable
                      </div>
                    </div>

                    <div class="space-y-3">
                      <div class="flex items-start justify-between gap-2">
                        <h4 class="text-lg sm:text-xl font-bold text-white group-hover:text-transparent group-hover:bg-gradient-to-r group-hover:from-purple-400 group-hover:to-pink-400 group-hover:bg-clip-text transition-all duration-300 leading-tight">
                          {{ item.localizedName || item.name }}
                        </h4>
                        <div v-if="!item.isAvailable && !displaySettings.showImages" class="flex-shrink-0 px-2 py-1 text-xs font-medium bg-red-500/20 text-red-300 rounded-lg border border-red-500/30 animate-pulse">
                          Unavailable
                        </div>
                      </div>

                      <p v-if="displaySettings.showDescriptions && (item.localizedDescription || item.description)"
                         class="text-sm sm:text-base text-gray-400 line-clamp-2 group-hover:text-gray-300 transition-colors">
                        {{ item.localizedDescription || item.description }}
                      </p>

                      <div v-if="displaySettings.showPrices" class="flex items-center justify-between pt-2 border-t border-white/10">
                        <span class="text-xl sm:text-2xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent group-hover:scale-110 transition-transform duration-300 inline-block">
                          {{ formatPrice(item.price) }}
                        </span>
                        <button class="p-2 rounded-full bg-purple-500/20 text-purple-400 group-hover:bg-purple-500 group-hover:text-white group-hover:rotate-0 -rotate-45 transition-all duration-300">
                          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M9 5l7 7-7 7" />
                          </svg>
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- No Results -->
        <div v-else-if="searchQuery" class="text-center py-20">
          <svg class="w-24 h-24 mx-auto mb-6 text-purple-400 opacity-30" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
          <h3 class="text-2xl font-bold mb-2 text-white">{{ t('menu.noResults') || 'No items found' }}</h3>
          <p class="text-lg text-gray-400">{{ t('menu.tryDifferentSearch') || 'Try a different search' }}</p>
        </div>

        <!-- Empty Menu -->
        <div v-else-if="!menu.categories || menu.categories.length === 0" class="text-center py-20">
          <svg class="w-24 h-24 mx-auto mb-6 text-purple-400 opacity-30" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
          <h3 class="text-2xl font-bold text-white">{{ t('menu.comingSoon') || 'Menu coming soon' }}</h3>
        </div>
      </main>

      <!-- Scroll to Top Button -->
      <Transition name="fade-scale">
        <button
          v-if="showScrollTop"
          @click="scrollToTop"
          class="fixed bottom-6 right-6 sm:bottom-8 sm:right-8 p-4 sm:p-5 rounded-full shadow-2xl shadow-purple-500/50 bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 text-white transition-all hover:scale-110 active:scale-95 z-50 border-2 border-white/20"
          aria-label="Scroll to top"
        >
          <svg class="w-6 h-6 sm:w-7 sm:h-7 animate-bounce-slow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M5 10l7-7m0 0l7 7m-7-7v18" />
          </svg>
        </button>
      </Transition>
    </div>

    <!-- Item Detail Modal -->
    <Transition name="modal">
      <div
        v-if="showItemDetail && selectedItem"
        class="fixed inset-0 z-[100] flex items-end sm:items-center justify-center bg-black/70 backdrop-blur-md p-0 sm:p-4"
        @click.self="closeItemDetail"
      >
        <div class="modal-content bg-gradient-to-b from-slate-900 to-slate-800 backdrop-blur-xl border-t sm:border border-purple-500/30 rounded-t-3xl sm:rounded-3xl shadow-2xl shadow-purple-500/20 max-w-2xl w-full max-h-[95vh] sm:max-h-[90vh] overflow-y-auto">
          <!-- Modal Header -->
          <div class="sticky top-0 flex items-center justify-between p-4 sm:p-6 bg-slate-900/95 backdrop-blur-xl border-b border-white/10 rounded-t-3xl z-10">
            <!-- Mobile drag handle -->
            <div class="absolute top-2 left-1/2 -translate-x-1/2 w-12 h-1.5 bg-white/20 rounded-full sm:hidden"></div>
            <h3 class="text-xl sm:text-2xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent mt-3 sm:mt-0">
              {{ selectedItem.localizedName || selectedItem.name }}
            </h3>
            <button
              @click="closeItemDetail"
              class="p-2 sm:p-2.5 rounded-xl hover:bg-white/10 active:scale-95 transition-all text-gray-400 hover:text-white"
            >
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>

          <!-- Modal Body -->
          <div class="p-4 sm:p-6 space-y-4 sm:space-y-6">
            <!-- Image -->
            <div v-if="selectedItem.imageUrl" class="rounded-2xl overflow-hidden shadow-2xl">
              <img
                :src="selectedItem.imageUrl"
                :alt="selectedItem.localizedName || selectedItem.name"
                class="w-full h-56 sm:h-80 object-cover modal-image"
              />
            </div>

            <!-- Price -->
            <div v-if="displaySettings.showPrices" class="flex items-center justify-between p-4 bg-white/5 rounded-xl border border-white/10">
              <span class="text-2xl sm:text-3xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent">
                {{ formatPrice(selectedItem.price) }}
              </span>
              <div v-if="!selectedItem.isAvailable" class="px-3 py-2 text-sm font-bold bg-red-500 text-white rounded-full shadow-lg animate-pulse">
                Unavailable
              </div>
            </div>

            <!-- Description -->
            <div v-if="selectedItem.localizedDescription || selectedItem.description" class="space-y-3 p-4 bg-white/5 rounded-xl border border-white/10">
              <h4 class="text-lg font-bold text-purple-300 flex items-center gap-2">
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
                Description
              </h4>
              <p class="text-base text-gray-300 leading-relaxed">{{ selectedItem.localizedDescription || selectedItem.description }}</p>
            </div>

            <!-- Category -->
            <div v-if="selectedItemCategory" class="space-y-3">
              <h4 class="text-lg font-bold text-purple-300 flex items-center gap-2">
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z" />
                </svg>
                Category
              </h4>
              <span class="inline-block px-4 py-2 text-sm font-semibold bg-gradient-to-r from-purple-500/20 to-pink-500/20 text-purple-300 rounded-xl border border-purple-500/30">
                {{ selectedItemCategory.localizedName || selectedItemCategory.name }}
              </span>
            </div>
          </div>

          <!-- Modal Footer -->
          <div class="sticky bottom-0 p-4 sm:p-6 bg-slate-900/95 backdrop-blur-xl border-t border-white/10 rounded-b-3xl">
            <button
              @click="closeItemDetail"
              class="w-full px-6 py-3.5 sm:py-4 bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 active:scale-98 text-white font-bold rounded-xl sm:rounded-2xl transition-all shadow-lg shadow-purple-500/30 hover:shadow-purple-500/50 text-base sm:text-lg"
            >
              Close
            </button>
          </div>
        </div>
      </div>
    </Transition>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch, onBeforeUnmount } from 'vue'
import type { MenuCategory, MenuItem } from '~/stores/restaurant'

const route = useRoute()
const restaurantStore = useRestaurantStore()
const { t, locale } = useI18n()

const menu = ref<any>(null)
const loading = ref(true)
const error = ref('')

// Search state
const searchQuery = ref('')
const showScrollTop = ref(false)

// Modal state
const showItemDetail = ref(false)
const selectedItem = ref<MenuItem | null>(null)
const selectedItemCategory = ref<MenuCategory | null>(null)

const displaySettings = computed(() => menu.value?.displaySettings || {
  showPrices: true,
  showImages: true,
  showDescriptions: true,
  showCategories: true,
  enableSearch: true,
  enableFilters: true
})

const currency = computed(() => menu.value?.currency || 'USD')

// Format price with currency
const formatPrice = (price: number) => {
  try {
    return new Intl.NumberFormat(locale.value, {
      style: 'currency',
      currency: currency.value,
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }).format(price)
  } catch (error) {
    return `${currency.value} ${price.toFixed(2)}`
  }
}

// Filter categories by search
const filteredCategories = computed(() => {
  if (!menu.value?.categories) return []

  if (!searchQuery.value) return menu.value.categories

  const query = searchQuery.value.toLowerCase()
  return menu.value.categories
    .map(category => filterCategoryBySearch(category, query))
    .filter(c => c !== null) as MenuCategory[]
})

const filterCategoryBySearch = (category: MenuCategory, query: string): MenuCategory | null => {
  const matchingItems = category.items.filter(item =>
    (item.localizedName || item.name).toLowerCase().includes(query) ||
    (item.localizedDescription || item.description || '').toLowerCase().includes(query)
  )

  const matchingChildren = category.children
    .map(child => filterCategoryBySearch(child, query))
    .filter(c => c !== null) as MenuCategory[]

  if (matchingItems.length > 0 || matchingChildren.length > 0 ||
      (category.localizedName || category.name).toLowerCase().includes(query)) {
    return { ...category, items: matchingItems, children: matchingChildren }
  }

  return null
}

const filteredItemsCount = computed(() => {
  const countItems = (categories: MenuCategory[]): number => {
    return categories.reduce((sum, category) => {
      return sum + category.items.length + countItems(category.children)
    }, 0)
  }
  return countItems(filteredCategories.value)
})

// Modal handlers
const openItemDetail = (item: MenuItem, category: MenuCategory) => {
  selectedItem.value = item
  selectedItemCategory.value = category
  showItemDetail.value = true
  document.body.style.overflow = 'hidden'
}

const closeItemDetail = () => {
  showItemDetail.value = false
  selectedItem.value = null
  selectedItemCategory.value = null
  document.body.style.overflow = ''
}

const scrollToTop = () => {
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

const handleScroll = () => {
  showScrollTop.value = window.scrollY > 400
}

onMounted(async () => {
  try {
    const slug = route.params.slug as string
    menu.value = await restaurantStore.fetchPublicMenu(slug, locale.value)
    console.log('Menu data:', menu.value)
    console.log('Restaurant Name:', menu.value?.restaurantName)
    console.log('Restaurant Localized Name:', menu.value?.restaurantLocalizedName)
    window.addEventListener('scroll', handleScroll)
  } catch (err: any) {
    error.value = err.message || t('messages.errorOccurred')
  } finally {
    loading.value = false
  }
})

onBeforeUnmount(() => {
  window.removeEventListener('scroll', handleScroll)
  document.body.style.overflow = ''
})

watch(() => locale.value, async (newLocale) => {
  if (!menu.value) return
  const slug = route.params.slug as string
  loading.value = true
  try {
    menu.value = await restaurantStore.fetchPublicMenu(slug, newLocale)
  } catch (err: any) {
    error.value = err.message || t('messages.errorOccurred')
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
/* Background blob animations */
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

/* Gradient animation for text and backgrounds */
@keyframes gradient {
  0%, 100% { background-position: 0% 50%; }
  50% { background-position: 100% 50%; }
}

.animate-gradient {
  animation: gradient 3s ease infinite;
}

.bg-size-200 {
  background-size: 200% 200%;
}

/* Slide in from bottom animation for menu items */
@keyframes slideInUp {
  from {
    opacity: 0;
    transform: translateY(30px) scale(0.95);
  }
  to {
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}

.menu-item-card {
  animation: slideInUp 0.5s ease-out forwards;
  opacity: 0;
}

/* Slide in from left for categories */
@keyframes slideInLeft {
  from {
    opacity: 0;
    transform: translateX(-30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.category-section {
  animation: slideInLeft 0.6s ease-out forwards;
}

/* Header fade in */
@keyframes fadeInDown {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.header-section {
  animation: fadeInDown 0.8s ease-out;
}

.logo-container {
  animation: fadeInDown 0.8s ease-out 0.2s both;
}

.restaurant-name {
  animation: fadeInDown 0.8s ease-out 0.4s both;
}

.decorative-line {
  animation: scaleX 0.8s ease-out 0.6s both;
  transform-origin: center;
}

@keyframes scaleX {
  from {
    transform: scaleX(0);
    opacity: 0;
  }
  to {
    transform: scaleX(1);
    opacity: 1;
  }
}

/* Scroll to top button animations */
.fade-scale-enter-active,
.fade-scale-leave-active {
  transition: opacity 0.3s ease, transform 0.3s cubic-bezier(0.34, 1.56, 0.64, 1);
}

.fade-scale-enter-from,
.fade-scale-leave-to {
  opacity: 0;
  transform: scale(0.8) translateY(20px) rotate(-10deg);
}

/* Modal animations - mobile optimized */
.modal-enter-active {
  transition: opacity 0.3s ease;
}

.modal-leave-active {
  transition: opacity 0.25s ease;
}

.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}

.modal-enter-active .modal-content {
  animation: slideUpModal 0.4s cubic-bezier(0.34, 1.56, 0.64, 1);
}

.modal-leave-active .modal-content {
  animation: slideDownModal 0.3s ease-out;
}

@keyframes slideUpModal {
  from {
    transform: translateY(100%);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

@keyframes slideDownModal {
  from {
    transform: translateY(0);
    opacity: 1;
  }
  to {
    transform: translateY(100%);
    opacity: 0;
  }
}

/* Modal image zoom in */
.modal-image {
  animation: zoomIn 0.5s ease-out 0.2s both;
}

@keyframes zoomIn {
  from {
    transform: scale(1.1);
    opacity: 0;
  }
  to {
    transform: scale(1);
    opacity: 1;
  }
}

/* Active scale for mobile touch */
.active\:scale-95:active {
  transform: scale(0.95);
}

.active\:scale-98:active {
  transform: scale(0.98);
}

/* Line clamp utility */
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

/* Smooth scroll behavior */
html {
  scroll-behavior: smooth;
}

/* Slow bounce animation for scroll button */
@keyframes bounce-slow {
  0%, 100% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-5px);
  }
}

.animate-bounce-slow {
  animation: bounce-slow 2s ease-in-out infinite;
}

/* Shimmer effect for cards on hover */
@keyframes shimmer {
  0% {
    background-position: -200% 0;
  }
  100% {
    background-position: 200% 0;
  }
}

/* Search bar animation */
.search-bar {
  animation: slideInDown 0.5s ease-out;
}

@keyframes slideInDown {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Quick fade scale for search clear button */
.fade-scale-quick-enter-active,
.fade-scale-quick-leave-active {
  transition: all 0.2s ease;
}

.fade-scale-quick-enter-from,
.fade-scale-quick-leave-to {
  opacity: 0;
  transform: scale(0.8) rotate(-90deg);
}

/* Slide fade for search results */
.slide-fade-enter-active {
  transition: all 0.3s ease-out;
}

.slide-fade-leave-active {
  transition: all 0.2s ease-in;
}

.slide-fade-enter-from {
  opacity: 0;
  transform: translateY(-10px);
}

.slide-fade-leave-to {
  opacity: 0;
  transform: translateY(-5px);
}

/* Mobile-specific enhancements */
@media (max-width: 640px) {
  /* Faster animations on mobile */
  .menu-item-card {
    animation-duration: 0.4s;
  }

  .category-section {
    animation-duration: 0.5s;
  }

  /* Reduce animation delays on mobile for faster perceived performance */
  .menu-item-card:nth-child(n+6) {
    animation-delay: 0.25s !important;
  }
}

/* Reduce motion for accessibility */
@media (prefers-reduced-motion: reduce) {
  *,
  *::before,
  *::after {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}
</style>
