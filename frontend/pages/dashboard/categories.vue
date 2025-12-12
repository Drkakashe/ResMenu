<template>
  <NuxtLayout name="dashboard">
    <div class="space-y-6">
      <!-- Header with Search -->
      <div class="flex flex-col gap-4 justify-between items-start sm:flex-row sm:items-center">
        <h1 class="text-2xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent">{{ $t('menu.categories') }}</h1>
        <div class="flex gap-3 items-center w-full sm:w-auto">
          <div class="relative flex-1 sm:w-64">
            <input
              v-model="searchQuery"
              type="text"
              :placeholder="$t('common.search')"
              class="py-2.5 pr-4 pl-11 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:ring-2 focus:ring-purple-500 focus:border-transparent hover:bg-white/15 hover:border-white/30"
            />
            <svg class="absolute top-3 left-4 w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
          </div>
          <button
            @click.stop.prevent="openCreateModal"
            type="button"
            class="inline-flex items-center justify-center font-medium rounded-xl transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-slate-900 disabled:opacity-50 disabled:cursor-not-allowed bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 text-white focus:ring-purple-500 px-5 py-2.5 text-sm shadow-lg hover:shadow-purple-500/50 transform hover:scale-[1.02] active:scale-[0.98] gap-2"
            :disabled="!restaurantId"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
            </svg>
            {{ $t('menu.addCategory') }}
          </button>
        </div>
      </div>

      <!-- Categories Tree -->
      <Card>
        <div v-if="loading" class="flex justify-center py-12">
          <LoadingSpinner size="lg" />
        </div>

        <div v-else-if="filteredCategories.length === 0 && !searchQuery">
          <EmptyState
            :title="$t('messages.noData')"
            :description="$t('menu.addCategory')"
          >
            <template #action>
              <UiButton @click="openCreateModal" variant="primary">
                {{ $t('menu.addCategory') }}
              </UiButton>
            </template>
          </EmptyState>
        </div>

        <div v-else-if="filteredCategories.length === 0 && searchQuery">
          <EmptyState
            :title="$t('messages.noResults')"
            description="Try adjusting your search"
          />
        </div>

        <div v-else class="space-y-2">
          <MenuCategoryTree
            :categories="paginatedCategories"
            :showItemForms="showItemForms"
            :itemForms="itemForms"
            :onToggleItemForm="toggleItemForm"
            :onSaveItem="createMenuItemForCategory"
            :onAddSubcategory="startAddSubcategory"
            :onEditCategory="editCategory"
            :onDeleteCategory="deleteCategory"
            :onAddItem="openAddItemModal"
          />
          
          <!-- Pagination -->
          <div v-if="totalPages > 1" class="flex justify-between items-center pt-4 mt-6 border-t border-white/10">
            <div class="text-sm text-gray-300">
              {{ $t('pagination.showing') }} {{ startIndex + 1 }} {{ $t('pagination.to') }} {{ Math.min(endIndex, filteredCategories.length) }} {{ $t('pagination.of') }} {{ filteredCategories.length }}
            </div>
            <div class="flex gap-2">
              <button
                @click="currentPage--"
                :disabled="currentPage === 1"
                class="px-4 py-2 text-sm font-medium rounded-xl border transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-gray-300 disabled:opacity-50 disabled:cursor-not-allowed hover:bg-white/15 hover:border-white/30"
              >
                {{ $t('pagination.previous') }}
              </button>
              <button
                v-for="page in visiblePages"
                :key="page"
                @click="currentPage = page"
                :class="[
                  'px-4 py-2 border rounded-xl text-sm font-medium transition-all duration-200',
                  currentPage === page ? 'bg-gradient-to-r from-purple-500 to-pink-500 text-white border-transparent shadow-lg shadow-purple-500/30' : 'bg-white/10 backdrop-blur-sm border-white/20 text-gray-300 hover:bg-white/15 hover:border-white/30'
                ]"
              >
                {{ page }}
              </button>
              <button
                @click="currentPage++"
                :disabled="currentPage === totalPages"
                class="px-4 py-2 text-sm font-medium rounded-xl border transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-gray-300 disabled:opacity-50 disabled:cursor-not-allowed hover:bg-white/15 hover:border-white/30"
              >
                {{ $t('pagination.next') }}
              </button>
            </div>
          </div>
        </div>
      </Card>

      <!-- Create/Edit Category Modal -->
      <div v-if="showCreateModal" class="fixed inset-0 z-[10000] flex items-center justify-center bg-black/60 backdrop-blur-sm" @click.self="showCreateModal = false">
        <div class="mx-4 w-full max-w-lg bg-white/10 backdrop-blur-xl rounded-2xl transition-all transform shadow-2xl border border-white/20 animate-fade-in-up">
          <!-- Header -->
          <div class="flex justify-between items-center px-6 py-5 border-b border-white/10">
            <h3 class="text-xl font-semibold text-white">
              {{ editingCategory ? $t('menu.editCategory') : $t('menu.addCategory') }}
            </h3>
            <button
              @click="showCreateModal = false"
              type="button"
              class="p-1 rounded-lg transition-colors text-gray-400 hover:text-white focus:outline-none hover:bg-white/10"
            >
              <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>

          <!-- Body -->
          <div class="px-6 py-6 space-y-5">
            <!-- English Name -->
            <div class="space-y-4">
              <div class="flex items-center gap-2 mb-2">
                <svg class="w-5 h-5 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
                </svg>
                <span class="text-sm font-semibold text-purple-400">English Content</span>
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-200">
                  Category Name (English) <span class="text-pink-400">*</span>
                </label>
                <input
                  v-model="form.name"
                  type="text"
                  required
                  class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 sm:text-sm"
                  placeholder="e.g., Main Dishes"
                />
              </div>
            </div>

            <!-- Divider -->
            <div class="border-t border-white/10"></div>

            <!-- Arabic Name -->
            <div class="space-y-4">
              <div class="flex items-center gap-2 mb-2">
                <svg class="w-5 h-5 text-pink-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
                </svg>
                <span class="text-sm font-semibold text-pink-400">المحتوى العربي</span>
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-200">
                  اسم الفئة (عربي)
                </label>
                <input
                  v-model="form.translations.ar"
                  type="text"
                  dir="rtl"
                  class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-pink-500 hover:bg-white/15 hover:border-white/30 sm:text-sm text-right"
                  placeholder="مثال: الأطباق الرئيسية"
                />
              </div>
            </div>

            <!-- Divider -->
            <div class="border-t border-white/10"></div>

            <!-- Parent Category -->
            <div>
              <label class="block mb-2 text-sm font-medium text-gray-200">
                {{ $t('menu.parentCategory') }}
              </label>
              <div class="relative">
                <select
                  v-model="form.parentId"
                  class="block py-2.5 pr-10 pl-4 w-full rounded-xl border shadow-sm transition-all duration-200 appearance-none bg-white/10 backdrop-blur-sm border-white/20 text-white focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 sm:text-sm"
                >
                  <option :value="null" class="bg-slate-800">{{ $t('menu.noCategory') }}</option>
                  <option
                    v-for="option in categoryOptions"
                    :key="option.id"
                    :value="option.id"
                    class="bg-slate-800"
                  >
                    {{ option.label }}
                  </option>
                </select>
                <svg class="absolute inset-y-0 right-3 my-auto w-4 h-4 pointer-events-none text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                </svg>
              </div>
            </div>

            <!-- Sort Order -->
            <div>
              <label class="block mb-2 text-sm font-medium text-gray-200">
                {{ $t('menu.sortOrder') }}
              </label>
              <input
                v-model.number="form.sortOrder"
                type="number"
                min="0"
                class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 sm:text-sm"
                :placeholder="$t('menu.sortOrder')"
              />
            </div>
          </div>

          <!-- Footer -->
          <div class="flex gap-3 justify-end px-6 py-4 rounded-b-2xl border-t border-white/10 bg-white/5">
            <button
              @click="showCreateModal = false"
              type="button"
              class="px-5 py-2.5 text-sm font-medium rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm text-gray-300 border-white/20 hover:bg-white/15 hover:border-white/30 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500 focus:ring-offset-slate-900"
            >
              {{ $t('common.cancel') }}
            </button>
            <button
              @click="saveCategory"
              type="button"
              :disabled="saving"
              class="px-5 py-2.5 text-sm font-medium text-white bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 border border-transparent rounded-xl focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500 focus:ring-offset-slate-900 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-200 shadow-lg hover:shadow-purple-500/50 transform hover:scale-[1.02] active:scale-[0.98]"
            >
              <span v-if="saving">Saving...</span>
              <span v-else>{{ $t('common.save') }}</span>
            </button>
          </div>
        </div>
      </div>

      <!-- Add Item Modal -->
      <div v-if="showAddItemModal" class="fixed inset-0 z-[10000] flex items-center justify-center bg-black/60 backdrop-blur-sm" @click.self="showAddItemModal = false">
        <div class="bg-white/10 backdrop-blur-xl rounded-2xl shadow-2xl border border-white/20 w-full max-w-2xl mx-4 max-h-[90vh] overflow-y-auto transform transition-all animate-fade-in-up">
          <!-- Header -->
          <div class="flex sticky top-0 justify-between items-center px-6 py-5 bg-slate-900/80 backdrop-blur-xl rounded-t-2xl border-b border-white/10">
            <h3 class="text-xl font-semibold text-white">
              {{ $t('menu.addItem') }}
            </h3>
            <button
              @click="showAddItemModal = false"
              type="button"
              class="p-1 rounded-lg transition-colors text-gray-400 hover:text-white focus:outline-none hover:bg-white/10"
            >
              <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>

          <!-- Body -->
          <div class="px-6 py-6 space-y-5">
            <!-- English Content -->
            <div class="space-y-4">
              <div class="flex items-center gap-2 mb-2">
                <svg class="w-5 h-5 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
                </svg>
                <span class="text-sm font-semibold text-purple-400">English Content</span>
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-200">
                  {{ $t('menu.itemName') }} (English) <span class="text-pink-400">*</span>
                </label>
                <input
                  v-model="newItemForm.name"
                  type="text"
                  required
                  class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 sm:text-sm"
                  placeholder="e.g., Grilled Chicken"
                />
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-200">
                  {{ $t('menu.description') }} (English)
                </label>
                <textarea
                  v-model="newItemForm.description"
                  rows="3"
                  class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 sm:text-sm"
                  placeholder="e.g., Tender grilled chicken breast"
                ></textarea>
              </div>
            </div>

            <!-- Divider -->
            <div class="border-t border-white/10"></div>

            <!-- Arabic Content -->
            <div class="space-y-4">
              <div class="flex items-center gap-2 mb-2">
                <svg class="w-5 h-5 text-pink-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
                </svg>
                <span class="text-sm font-semibold text-pink-400">المحتوى العربي</span>
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-200">
                  اسم العنصر (عربي)
                </label>
                <input
                  v-model="newItemForm.translations.ar.name"
                  type="text"
                  dir="rtl"
                  class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-pink-500 hover:bg-white/15 hover:border-white/30 sm:text-sm text-right"
                  placeholder="مثال: دجاج مشوي"
                />
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-200">
                  الوصف (عربي)
                </label>
                <textarea
                  v-model="newItemForm.translations.ar.description"
                  rows="3"
                  dir="rtl"
                  class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-pink-500 hover:bg-white/15 hover:border-white/30 sm:text-sm text-right"
                  placeholder="مثال: صدر دجاج مشوي طري"
                ></textarea>
              </div>
            </div>

            <!-- Divider -->
            <div class="border-t border-white/10"></div>
            <div class="grid grid-cols-2 gap-4">
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-200">
                  {{ $t('menu.price') }} <span class="text-pink-400">*</span>
                </label>
                <input
                  v-model.number="newItemForm.price"
                  type="number"
                  step="0.01"
                  min="0"
                  required
                  class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 sm:text-sm"
                  :placeholder="$t('menu.price')"
                />
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-200">
                  {{ $t('menu.sortOrder') }}
                </label>
                <input
                  v-model.number="newItemForm.displayOrder"
                  type="number"
                  min="0"
                  class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 sm:text-sm"
                  :placeholder="$t('menu.sortOrder')"
                />
              </div>
            </div>
            <div class="w-full">
              <FileUpload
                v-model="newItemForm.imageUrl"
                :label="$t('menu.imageUrl') || 'Image'"
                accept="image/*"
                :max-size="5"
                :disabled="uploadingImage"
                :uploading="uploadingImage"
                @upload="handleImageUpload"
              />
            </div>
            <div class="flex items-center">
              <input
                v-model="newItemForm.isAvailable"
                type="checkbox"
                id="item-available"
                class="w-4 h-4 rounded text-purple-500 bg-white/10 border-white/20 focus:ring-purple-500 focus:ring-offset-slate-900"
              />
              <label for="item-available" class="ml-2 text-sm text-gray-200">
                {{ $t('menu.isAvailable') }}
              </label>
            </div>
          </div>

          <!-- Footer -->
          <div class="flex sticky bottom-0 gap-3 justify-end px-6 py-4 rounded-b-2xl border-t border-white/10 bg-white/5">
            <button
              @click="showAddItemModal = false"
              type="button"
              class="px-5 py-2.5 text-sm font-medium rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm text-gray-300 border-white/20 hover:bg-white/15 hover:border-white/30 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500 focus:ring-offset-slate-900"
            >
              {{ $t('common.cancel') }}
            </button>
            <button
              @click="saveNewItem"
              type="button"
              :disabled="saving"
              class="px-5 py-2.5 text-sm font-medium text-white bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 border border-transparent rounded-xl focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500 focus:ring-offset-slate-900 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-200 shadow-lg hover:shadow-purple-500/50 transform hover:scale-[1.02] active:scale-[0.98]"
            >
              <span v-if="saving">Saving...</span>
              <span v-else>{{ $t('common.save') }}</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  </NuxtLayout>
</template>

<script setup lang="ts">
import type { MenuCategory } from '@/stores/restaurant'
import FileUpload from '@/components/ui/FileUpload.vue'

definePageMeta({
  layout: false,
  middleware: ['owner']
})

const { t, locale } = useI18n({ useScope: 'global' })
const authStore = useAuthStore()
const restaurantStore = useRestaurantStore()
const toast = useToast()

const loading = ref(false)
const saving = ref(false)
const showCreateModal = ref(false)
const showAddItemModal = ref(false)
const selectedCategoryForItem = ref<string | null>(null)
const editingCategory = ref<MenuCategory | null>(null)
const showItemForms = ref<Record<string, boolean>>({})
const searchQuery = ref('')
const currentPage = ref(1)
const itemsPerPage = ref(10)
const itemForms = ref<Record<
  string,
  {
    name: string
    description?: string | null
    price: number
    imageUrl?: string | null
    isAvailable: boolean
    displayOrder: number
    translations: {
      ar: {
        name: string
        description: string
      }
    }
  }
>>({})

const form = ref({
  name: '',
  parentId: null as string | null,
  sortOrder: 0,
  translations: {
    en: '',
    ar: ''
  }
})

const newItemForm = ref({
  name: '',
  description: '',
  price: 0,
  imageUrl: '',
  isAvailable: true,
  displayOrder: 0,
  translations: {
    ar: {
      name: '',
      description: ''
    }
  }
})

const categories = computed(() => restaurantStore.categories)

// Flatten categories for search
const flattenCategories = (cats: MenuCategory[]): MenuCategory[] => {
  const result: MenuCategory[] = []
  const flatten = (list: MenuCategory[]) => {
    for (const cat of list) {
      result.push(cat)
      if (cat.children.length) {
        flatten(cat.children)
      }
    }
  }
  flatten(cats)
  return result
}

const filteredCategories = computed(() => {
  if (!searchQuery.value) {
    return categories.value
  }
  
  const query = searchQuery.value.toLowerCase()
  const flat = flattenCategories(categories.value)
  return flat.filter(cat => 
    (cat.name?.toLowerCase().includes(query)) ||
    (cat.localizedName?.toLowerCase().includes(query))
  )
})

const totalPages = computed(() => Math.ceil(filteredCategories.value.length / itemsPerPage.value))
const startIndex = computed(() => (currentPage.value - 1) * itemsPerPage.value)
const endIndex = computed(() => startIndex.value + itemsPerPage.value)

const paginatedCategories = computed(() => {
  if (!searchQuery.value) {
    return filteredCategories.value.slice(startIndex.value, endIndex.value)
  }
  return filteredCategories.value.slice(startIndex.value, endIndex.value)
})

const visiblePages = computed(() => {
  const pages = []
  const maxVisible = 5
  let start = Math.max(1, currentPage.value - Math.floor(maxVisible / 2))
  let end = Math.min(totalPages.value, start + maxVisible - 1)
  
  if (end - start < maxVisible - 1) {
    start = Math.max(1, end - maxVisible + 1)
  }
  
  for (let i = start; i <= end; i++) {
    pages.push(i)
  }
  return pages
})

const categoryOptions = computed(() => {
  const options: { id: string; label: string }[] = []

  const buildOptions = (list: MenuCategory[], prefix = '') => {
    for (const category of list) {
      options.push({
        id: category.id,
        label: `${prefix}${category.localizedName || category.name}`
      })

      if (category.children.length) {
        buildOptions(category.children, `${prefix}› `)
      }
    }
  }

  buildOptions(categories.value)
  return options
})

const restaurantId = computed(() => authStore.restaurantId || '')

watch(searchQuery, () => {
  currentPage.value = 1
})

const openCreateModal = (event?: Event) => {
  event?.preventDefault()
  event?.stopPropagation()
  
  if (!restaurantId.value) {
    toast.error(t('messages.errorOccurred') || 'Restaurant ID not found. Please login again.')
    return
  }
  
  resetForm()
  editingCategory.value = null
  showCreateModal.value = true
}

const openAddItemModal = (categoryId: string) => {
  if (!categoryId) {
    toast.error(t('messages.errorOccurred') || 'Category ID not found.')
    return
  }
  selectedCategoryForItem.value = categoryId
  // Reset form
  newItemForm.value = {
    name: '',
    description: '',
    price: 0,
    imageUrl: '',
    isAvailable: true,
    displayOrder: 0,
    translations: {
      ar: {
        name: '',
        description: ''
      }
    }
  }
  showAddItemModal.value = true
}

const resetForm = () => {
  form.value = {
    name: '',
    parentId: null,
    sortOrder: 0,
    translations: {
      en: '',
      ar: ''
    }
  }
  editingCategory.value = null
}

const loadCategories = async () => {
  if (!restaurantId.value) {
    return
  }

  loading.value = true
  try {
    await restaurantStore.fetchCategories(restaurantId.value, locale.value)
  } catch (error: any) {
    toast.error('Failed to load categories')
    const errorMessage = error?.response?.data?.message || error?.message || t('messages.errorOccurred') || 'Failed to load categories'
    toast.error(errorMessage)
  } finally {
    loading.value = false
  }
}

const ensureItemForm = (categoryId: string) => {
  if (!itemForms.value[categoryId]) {
    itemForms.value[categoryId] = {
      name: '',
      description: '',
      price: 0,
      imageUrl: '',
      isAvailable: true,
      displayOrder: 0,
      translations: {
        ar: {
          name: '',
          description: ''
        }
      }
    }
  }
}

const toggleItemForm = (categoryId: string) => {
  showItemForms.value[categoryId] = !showItemForms.value[categoryId]
  if (showItemForms.value[categoryId]) {
    ensureItemForm(categoryId)
  }
}

const resetItemForm = (categoryId: string) => {
  itemForms.value[categoryId] = {
    name: '',
    description: '',
    price: 0,
    imageUrl: '',
    isAvailable: true,
    displayOrder: 0,
    translations: {
      ar: {
        name: '',
        description: ''
      }
    }
  }
}

const startAddSubcategory = (categoryId: string) => {
  resetForm()
  form.value.parentId = categoryId
  showCreateModal.value = true
}

const editCategory = (category: MenuCategory) => {
  editingCategory.value = category
  form.value = {
    name: category.name,
    parentId: category.parentCategoryId ?? null,
    sortOrder: category.displayOrder ?? 0,
    translations: {
      en: category.translations?.en ?? '',
      ar: category.translations?.ar ?? ''
    }
  }
  showCreateModal.value = true
}

const deleteCategory = async (category: MenuCategory) => {
  if (!restaurantId.value) {
    return
  }
  const confirmation = confirm(t('messages.confirmDelete'))
  if (!confirmation) {
    return
  }

  try {
    await restaurantStore.deleteCategory(category.id, restaurantId.value, locale.value)
    await loadCategories()
    toast.success(t('messages.success') || 'Category deleted successfully!')
  } catch (error: any) {
    toast.error('Failed to delete category')
    const errorMessage = error?.response?.data?.message || error?.message || t('messages.errorOccurred') || 'Failed to delete category'
    toast.error(errorMessage)
  }
}

const saveCategory = async () => {
  if (!restaurantId.value) {
    return
  }

  // Build translations object with proper structure
  const payloadTranslations: any = {}
  if (form.value.translations.ar?.trim()) {
    payloadTranslations.ar = form.value.translations.ar.trim()
  }

  const payload = {
    name: form.value.name.trim(),
    displayOrder: Number.isFinite(form.value.sortOrder) ? Number(form.value.sortOrder) : 0,
    parentCategoryId: form.value.parentId || null,
    translations: Object.keys(payloadTranslations).length ? payloadTranslations : undefined
  }

  if (!payload.name) {
    toast.error(t('validation.required'))
    return
  }

  saving.value = true
  try {
    if (editingCategory.value) {
      await restaurantStore.updateCategory(editingCategory.value.id, payload, restaurantId.value, locale.value)
    } else {
      await restaurantStore.createCategory(restaurantId.value, payload, locale.value)
    }
    await loadCategories()
    showCreateModal.value = false
    resetForm()
    toast.success(t('messages.success') || 'Category saved successfully!')
  } catch (error: any) {
    toast.error('Failed to save category')
    const errorMessage = error?.response?.data?.message || error?.message || t('messages.errorOccurred') || 'Failed to save category'
    toast.error(errorMessage)
  } finally {
    saving.value = false
  }
}

const uploadingImage = ref(false)

const handleImageUpload = async (file: File) => {
  uploadingImage.value = true
  const api = useApi()
  const formData = new FormData()
  formData.append('file', file)

  try {
    const response = await api.post('/Files/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })

    // The API returns Result<string> with success and data properties
    if (response.data.success) {
      newItemForm.value.imageUrl = response.data.data
    } else {
      throw new Error(response.data.message || 'Upload failed')
    }
  } catch (error: any) {
    toast.error('Failed to upload image')
    const errorMessage = error?.response?.data?.message || error?.response?.data?.error || error?.message || 'Failed to upload image'
    toast.error(errorMessage)
    newItemForm.value.imageUrl = ''
  } finally {
    uploadingImage.value = false
  }
}

const saveNewItem = async () => {
  if (!selectedCategoryForItem.value) {
    toast.error(t('messages.errorOccurred') || 'Category not selected.')
    return
  }

  if (!newItemForm.value.name.trim()) {
    toast.error(t('validation.required') || 'Item name is required')
    return
  }

  const price = Number(newItemForm.value.price)
  if (!price || price <= 0 || !isFinite(price)) {
    toast.error(t('validation.required') || 'Price must be greater than 0')
    return
  }

  saving.value = true
  try {
    // Build translations object with proper structure
    const payloadTranslations: any = {}
    if (newItemForm.value.translations.ar.name?.trim() || newItemForm.value.translations.ar.description?.trim()) {
      payloadTranslations.ar = {}
      if (newItemForm.value.translations.ar.name?.trim()) {
        payloadTranslations.ar.name = newItemForm.value.translations.ar.name.trim()
      }
      if (newItemForm.value.translations.ar.description?.trim()) {
        payloadTranslations.ar.description = newItemForm.value.translations.ar.description.trim()
      }
    }

    await restaurantStore.createMenuItem(selectedCategoryForItem.value, {
      name: newItemForm.value.name.trim(),
      description: newItemForm.value.description?.trim() || null,
      price: price,
      imageUrl: newItemForm.value.imageUrl?.trim() || null,
      isAvailable: newItemForm.value.isAvailable,
      displayOrder: Number(newItemForm.value.displayOrder) || 0,
      translations: Object.keys(payloadTranslations).length ? payloadTranslations : undefined
    })
    
    // Reset form
    newItemForm.value = {
      name: '',
      description: '',
      price: 0,
      imageUrl: '',
      isAvailable: true,
      displayOrder: 0,
      translations: {
        ar: {
          name: '',
          description: ''
        }
      }
    }
    
    showAddItemModal.value = false
    selectedCategoryForItem.value = null
    await loadCategories()
    toast.success(t('messages.success') || 'Menu item created successfully!')
  } catch (error: any) {
    toast.error('Failed to create menu item')
    const errorMessage = error?.response?.data?.message || error?.message || t('messages.errorOccurred') || 'Failed to create menu item'
    toast.error(errorMessage)
  } finally {
    saving.value = false
  }
}

const createMenuItemForCategory = async (categoryId: string) => {
  const formState = itemForms.value[categoryId]
  if (!formState) {
    return
  }

  if (!formState.name.trim()) {
    toast.error(t('validation.required') || 'Item name is required')
    return
  }

  try {
    // Build translations object with proper structure
    const payloadTranslations: any = {}
    if (formState.translations.ar.name?.trim() || formState.translations.ar.description?.trim()) {
      payloadTranslations.ar = {}
      if (formState.translations.ar.name?.trim()) {
        payloadTranslations.ar.name = formState.translations.ar.name.trim()
      }
      if (formState.translations.ar.description?.trim()) {
        payloadTranslations.ar.description = formState.translations.ar.description.trim()
      }
    }

    await restaurantStore.createMenuItem(categoryId, {
      name: formState.name.trim(),
      description: formState.description?.trim() || null,
      price: Number(formState.price) || 0,
      imageUrl: formState.imageUrl || null,
      isAvailable: formState.isAvailable,
      displayOrder: Number(formState.displayOrder) || 0,
      translations: Object.keys(payloadTranslations).length ? payloadTranslations : undefined
    })
    resetItemForm(categoryId)
    showItemForms.value[categoryId] = false
    await loadCategories()
  } catch (error: any) {
    toast.error('Failed to create menu item')
    const errorMessage = error?.response?.data?.message || error?.message || t('messages.errorOccurred') || 'Failed to create menu item'
    toast.error(errorMessage)
  }
}

onMounted(async () => {
  authStore.loadFromStorage()
  if (!authStore.isAuthenticated) {
    navigateTo('/login')
    return
  }
  await loadCategories()
})

watch(
  () => locale.value,
  async () => {
    await loadCategories()
  }
)

watch(
  () => authStore.restaurantId,
  async (newId, oldId) => {
    if (newId && newId !== oldId) {
      await loadCategories()
    }
  }
)
</script>

<style scoped>
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

.animate-fade-in-up {
  animation: fadeInUp 0.4s ease-out forwards;
}
</style>
