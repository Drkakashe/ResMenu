<template>
  <NuxtLayout name="dashboard">
    <!-- Header -->
    <div class="flex items-center justify-between mb-6 px-6 pt-6">
      <div>
        <h1 class="text-3xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent">Menu Builder</h1>
        <p class="mt-2 text-gray-300">Drag and drop to organize your menu</p>
      </div>

      <!-- Auto-save Indicator & Actions -->
      <div class="flex items-center gap-3">
        <Transition name="fade">
          <div v-if="saving" class="flex items-center gap-2 px-3 py-2 rounded-lg bg-blue-500/20 backdrop-blur-sm border border-blue-500/30 text-blue-300">
            <svg class="w-4 h-4 animate-spin" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            <span class="text-sm font-medium">Saving...</span>
          </div>
          <div v-else-if="saved" class="flex items-center gap-2 px-3 py-2 rounded-lg bg-green-500/20 backdrop-blur-sm border border-green-500/30 text-green-300">
            <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
              <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
            </svg>
            <span class="text-sm font-medium">Saved</span>
          </div>
        </Transition>

        <button
          @click="showPreview = !showPreview"
          class="px-4 py-2 rounded-lg bg-white/10 backdrop-blur-sm border border-white/20 hover:bg-white/20 text-white transition-all transform hover:scale-105 shadow-lg"
          :title="showPreview ? 'Hide Preview' : 'Show Preview'"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
          </svg>
        </button>

        <button
          @click="showExportDialog = true"
          class="px-4 py-2 rounded-lg bg-white/10 backdrop-blur-sm border border-white/20 hover:bg-white/20 text-white transition-all transform hover:scale-105 shadow-lg flex items-center gap-2"
          :disabled="categories.length === 0"
          title="Export Menu"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
          </svg>
          <span>Export</span>
        </button>

        <UiButton @click="addCategory" variant="primary" class="bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 text-white px-6 py-2.5 rounded-lg font-semibold shadow-lg hover:shadow-purple-500/50 transition-all transform hover:scale-105">
          + Add Category
        </UiButton>
      </div>
    </div>

    <!-- Split View: Editor + Preview -->
    <div class="flex gap-4 px-6 pb-6" :class="showPreview ? 'h-[calc(100vh-180px)]' : ''">
      <!-- Editor Panel -->
      <div :class="showPreview ? 'w-1/2 overflow-auto custom-scrollbar' : 'w-full'">
        <div class="space-y-4">

      <!-- Loading State -->
      <div v-if="loading" class="flex justify-center py-12">
        <div class="inline-flex h-12 w-12 items-center justify-center rounded-full border-4 border-purple-500/30 border-t-purple-500 animate-spin"></div>
      </div>

      <!-- Categories -->
      <div v-else-if="categories.length > 0" class="space-y-4">
        <DraggableCategory
          v-for="(category, index) in categories"
          :key="category.id"
          :category="category"
          :is-dragged-over="draggedOverIndex === index"
          @drag-start="handleDragStart"
          @drag-over="handleDragOver(index, $event)"
          @drag-leave="draggedOverIndex = null"
          @drop="handleDrop(index)"
          @drag-end="draggedOverIndex = null"
          @update="updateCategory"
          @delete="deleteCategory(category.id)"
          @customize="openStyleEditor(category)"
          @edit-translations="openCategoryDialog"
          @add-item="openItemDialog(category)"
          @edit-item="openEditItemDialog"
          @reorder-items="reorderItems(category.id, $event)"
        />
      </div>

      <!-- Empty State -->
      <div v-else class="text-center py-16">
        <div class="inline-flex items-center justify-center w-20 h-20 rounded-2xl bg-gradient-to-br from-purple-500/20 to-pink-500/20 backdrop-blur-sm border border-white/10 mb-6">
          <svg class="w-10 h-10 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
        </div>
        <h3 class="text-xl font-bold text-white mb-2">No categories yet</h3>
        <p class="text-gray-400 mb-6">Create your first category to start building your menu</p>
        <UiButton @click="addCategory" variant="primary" class="bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 text-white px-6 py-3 rounded-lg font-semibold shadow-lg hover:shadow-purple-500/50 transition-all transform hover:scale-105">
          + Add Category
        </UiButton>
      </div>
        </div>
      </div>

      <!-- Preview Panel -->
      <div v-if="showPreview" class="w-1/2 bg-white/5 backdrop-blur-sm rounded-2xl border border-white/10 p-4 overflow-auto custom-scrollbar">
        <PreviewPanel
          :categories="categories"
          :theme="currentTheme"
          :display-settings="displaySettings"
          :restaurant="restaurant"
          :language="'en'"
          :currency="'USD'"
        />
      </div>
    </div>

    <!-- Category Style Editor Modal -->
    <CategoryStyleEditor
      v-if="styleEditorCategory"
      :show="showStyleEditor"
      :category="styleEditorCategory"
      :restaurant-id="authStore.restaurantId || ''"
      @close="closeStyleEditor"
      @save="handleStyleSave"
    />

    <!-- Add Item Modal -->
    <Modal :model-value="showItemDialog" @close="closeItemDialog" size="lg" dark>
      <template #header>
        <div class="flex items-center justify-between">
          <h2 class="text-xl font-bold text-white">Add Item to {{ selectedCategory?.name }}</h2>
          <!-- Language Toggle -->
          <div class="flex items-center gap-2 bg-white/5 rounded-lg p-1">
            <button
              @click="activeLanguage = 'en'"
              :class="[
                'px-4 py-2 rounded-md text-sm font-medium transition-all',
                activeLanguage === 'en'
                  ? 'bg-gradient-to-r from-purple-500 to-pink-500 text-white shadow-lg'
                  : 'text-gray-400 hover:text-white'
              ]"
            >
              English
            </button>
            <button
              @click="activeLanguage = 'ar'"
              :class="[
                'px-4 py-2 rounded-md text-sm font-medium transition-all',
                activeLanguage === 'ar'
                  ? 'bg-gradient-to-r from-purple-500 to-pink-500 text-white shadow-lg'
                  : 'text-gray-400 hover:text-white'
              ]"
            >
              العربية
            </button>
          </div>
        </div>
      </template>

      <div class="space-y-6">
        <!-- English Fields -->
        <Transition name="slide-fade" mode="out-in">
          <div v-if="activeLanguage === 'en'" class="space-y-4">
            <div class="flex items-center gap-2 mb-4">
              <svg class="w-5 h-5 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5h12M9 3v2m1.048 9.5A18.022 18.022 0 016.412 9m6.088 9h7M11 21l5-10 5 10M12.751 5C11.783 10.77 8.07 15.61 3 18.129" />
              </svg>
              <span class="text-sm font-semibold text-purple-400">English Content</span>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Item Name (English) *</label>
              <input
                v-model="newItem.name"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all"
                placeholder="e.g., Grilled Chicken"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Description (English)</label>
              <textarea
                v-model="newItem.description"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all"
                rows="3"
                placeholder="Brief description of the item"
              ></textarea>
            </div>
          </div>

          <!-- Arabic Fields -->
          <div v-else class="space-y-4">
            <div class="flex items-center gap-2 mb-4">
              <svg class="w-5 h-5 text-pink-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5h12M9 3v2m1.048 9.5A18.022 18.022 0 016.412 9m6.088 9h7M11 21l5-10 5 10M12.751 5C11.783 10.77 8.07 15.61 3 18.129" />
              </svg>
              <span class="text-sm font-semibold text-pink-400">المحتوى العربي</span>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">اسم العنصر (عربي)</label>
              <input
                v-model="newItem.translations.ar.name"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-pink-500 focus:border-transparent transition-all text-right"
                placeholder="مثال: دجاج مشوي"
                dir="rtl"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">الوصف (عربي)</label>
              <textarea
                v-model="newItem.translations.ar.description"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-pink-500 focus:border-transparent transition-all text-right"
                rows="3"
                placeholder="وصف مختصر للعنصر"
                dir="rtl"
              ></textarea>
            </div>
          </div>
        </Transition>

        <!-- Common Fields (always visible) -->
        <div class="pt-4 border-t border-white/10">
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Price *</label>
              <input
                v-model.number="newItem.price"
                type="number"
                step="0.01"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all"
                placeholder="0.00"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Image URL (optional)</label>
              <input
                v-model="newItem.imageUrl"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all"
                placeholder="https://..."
              />
            </div>
          </div>

          <div class="mt-4">
            <label class="flex items-center gap-2 cursor-pointer">
              <input
                v-model="newItem.isAvailable"
                type="checkbox"
                class="w-5 h-5 rounded border-white/20 bg-white/10 text-purple-500 focus:ring-purple-500 focus:ring-offset-0"
              />
              <span class="text-sm font-medium text-gray-300">Item is available</span>
            </label>
          </div>
        </div>
      </div>

      <template #footer>
        <div class="flex justify-end gap-3">
          <UiButton @click="closeItemDialog" variant="secondary" class="px-6 py-2.5 rounded-lg bg-white/10 backdrop-blur-sm border border-white/20 hover:bg-white/20 text-white transition-all">
            Cancel
          </UiButton>
          <UiButton @click="saveNewItem" variant="primary" class="bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 text-white px-6 py-2.5 rounded-lg font-semibold shadow-lg hover:shadow-purple-500/50 transition-all transform hover:scale-105">
            Add Item
          </UiButton>
        </div>
      </template>
    </Modal>

    <!-- Edit Item Modal -->
    <Modal :model-value="showEditItemDialog" @close="closeEditItemDialog" size="lg" dark>
      <template #header>
        <div class="flex items-center justify-between">
          <h2 class="text-xl font-bold text-white">Edit Item</h2>
          <!-- Language Toggle -->
          <div class="flex items-center gap-2 bg-white/5 rounded-lg p-1">
            <button
              @click="activeLanguage = 'en'"
              :class="[
                'px-4 py-2 rounded-md text-sm font-medium transition-all',
                activeLanguage === 'en'
                  ? 'bg-gradient-to-r from-purple-500 to-pink-500 text-white shadow-lg'
                  : 'text-gray-400 hover:text-white'
              ]"
            >
              English
            </button>
            <button
              @click="activeLanguage = 'ar'"
              :class="[
                'px-4 py-2 rounded-md text-sm font-medium transition-all',
                activeLanguage === 'ar'
                  ? 'bg-gradient-to-r from-purple-500 to-pink-500 text-white shadow-lg'
                  : 'text-gray-400 hover:text-white'
              ]"
            >
              العربية
            </button>
          </div>
        </div>
      </template>

      <div class="space-y-6">
        <!-- English Fields -->
        <Transition name="slide-fade" mode="out-in">
          <div v-if="activeLanguage === 'en'" class="space-y-4">
            <div class="flex items-center gap-2 mb-4">
              <svg class="w-5 h-5 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5h12M9 3v2m1.048 9.5A18.022 18.022 0 016.412 9m6.088 9h7M11 21l5-10 5 10M12.751 5C11.783 10.77 8.07 15.61 3 18.129" />
              </svg>
              <span class="text-sm font-semibold text-purple-400">English Content</span>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Item Name (English) *</label>
              <input
                v-model="newItem.name"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all"
                placeholder="e.g., Grilled Chicken"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Description (English)</label>
              <textarea
                v-model="newItem.description"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all"
                rows="3"
                placeholder="Brief description of the item"
              ></textarea>
            </div>
          </div>

          <!-- Arabic Fields -->
          <div v-else class="space-y-4">
            <div class="flex items-center gap-2 mb-4">
              <svg class="w-5 h-5 text-pink-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5h12M9 3v2m1.048 9.5A18.022 18.022 0 016.412 9m6.088 9h7M11 21l5-10 5 10M12.751 5C11.783 10.77 8.07 15.61 3 18.129" />
              </svg>
              <span class="text-sm font-semibold text-pink-400">المحتوى العربي</span>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">اسم العنصر (عربي)</label>
              <input
                v-model="newItem.translations.ar.name"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-pink-500 focus:border-transparent transition-all text-right"
                placeholder="مثال: دجاج مشوي"
                dir="rtl"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">الوصف (عربي)</label>
              <textarea
                v-model="newItem.translations.ar.description"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-pink-500 focus:border-transparent transition-all text-right"
                rows="3"
                placeholder="وصف مختصر للعنصر"
                dir="rtl"
              ></textarea>
            </div>
          </div>
        </Transition>

        <!-- Common Fields (always visible) -->
        <div class="pt-4 border-t border-white/10">
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Price *</label>
              <input
                v-model.number="newItem.price"
                type="number"
                step="0.01"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all"
                placeholder="0.00"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Image URL (optional)</label>
              <input
                v-model="newItem.imageUrl"
                class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all"
                placeholder="https://..."
              />
            </div>
          </div>

          <div class="mt-4">
            <label class="flex items-center gap-2 cursor-pointer">
              <input
                v-model="newItem.isAvailable"
                type="checkbox"
                class="w-5 h-5 rounded border-white/20 bg-white/10 text-purple-500 focus:ring-purple-500 focus:ring-offset-0"
              />
              <span class="text-sm font-medium text-gray-300">Item is available</span>
            </label>
          </div>
        </div>
      </div>

      <template #footer>
        <div class="flex justify-end gap-3">
          <UiButton @click="closeEditItemDialog" variant="secondary" class="px-6 py-2.5 rounded-lg bg-white/10 backdrop-blur-sm border border-white/20 hover:bg-white/20 text-white transition-all">
            Cancel
          </UiButton>
          <UiButton @click="saveEditedItem" variant="primary" class="bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 text-white px-6 py-2.5 rounded-lg font-semibold shadow-lg hover:shadow-purple-500/50 transition-all transform hover:scale-105">
            Save Changes
          </UiButton>
        </div>
      </template>
    </Modal>

    <!-- Category Translation Modal -->
    <Modal :model-value="showCategoryDialog" @close="closeCategoryDialog" size="md" dark>
      <template #header>
        <h2 class="text-xl font-bold text-white">
          {{ editingCategory ? 'Edit Category' : 'Add Category' }}
        </h2>
      </template>

      <div class="space-y-6">
        <!-- English Field -->
        <div class="space-y-4">
          <div class="flex items-center gap-2 mb-2">
            <svg class="w-5 h-5 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
            </svg>
            <span class="text-sm font-semibold text-purple-400">English Content</span>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-300 mb-2">Category Name (English) *</label>
            <input
              v-model="categoryForm.name"
              class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:border-transparent transition-all"
              placeholder="e.g., Main Dishes"
            />
          </div>
        </div>

        <!-- Divider -->
        <div class="border-t border-white/10"></div>

        <!-- Arabic Field -->
        <div class="space-y-4">
          <div class="flex items-center gap-2 mb-2">
            <svg class="w-5 h-5 text-pink-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
            </svg>
            <span class="text-sm font-semibold text-pink-400">المحتوى العربي</span>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-300 mb-2">اسم الفئة (عربي)</label>
            <input
              v-model="categoryForm.translations.ar.name"
              class="w-full px-4 py-3 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-pink-500 focus:border-transparent transition-all text-right"
              placeholder="مثال: الأطباق الرئيسية"
              dir="rtl"
            />
          </div>
        </div>
      </div>

      <template #footer>
        <div class="flex justify-end gap-3">
          <UiButton @click="closeCategoryDialog" variant="secondary" class="px-6 py-2.5 rounded-lg bg-white/10 backdrop-blur-sm border border-white/20 hover:bg-white/20 text-white transition-all">
            Cancel
          </UiButton>
          <UiButton @click="saveCategoryWithTranslations" variant="primary" class="bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 text-white px-6 py-2.5 rounded-lg font-semibold shadow-lg hover:shadow-purple-500/50 transition-all transform hover:scale-105">
            {{ editingCategory ? 'Save Changes' : 'Add Category' }}
          </UiButton>
        </div>
      </template>
    </Modal>

    <!-- Export Dialog -->
    <ExportDialog
      v-if="showExportDialog"
      :show="showExportDialog"
      :categories="categories"
      :theme="currentTheme"
      :restaurant-name="restaurant.name"
      :logo-url="restaurant.logoUrl"
      @close="showExportDialog = false"
      @exported="handleExported"
    />
  </NuxtLayout>
</template>

<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue'
import { useAuthStore } from '~/stores/auth'
import { useRestaurantStore } from '~/stores/restaurant'
import { useTemplateStore, createDefaultTheme } from '~/stores/templates'
import { useDragDrop } from '~/composables/useDragDrop'
import type { MenuCategory, MenuItem } from '~/stores/restaurant'
import DraggableCategory from '~/components/menu-builder/DraggableCategory.vue'
import CategoryStyleEditor from '~/components/menu/CategoryStyleEditor.vue'
import PreviewPanel from '~/components/menu-builder/PreviewPanel.vue'
import ExportDialog from '~/components/menu/ExportDialog.vue'
import Modal from '~/components/ui/Modal.vue'
import UiButton from '~/components/ui/Button.vue'

definePageMeta({
  layout: false,
  middleware: ['owner']
})

const authStore = useAuthStore()
const restaurantStore = useRestaurantStore()
const templateStore = useTemplateStore()
const toast = useToast()
const { $api } = useNuxtApp()

const loading = ref(true)
const saving = ref(false)
const saved = ref(false)
const categories = ref<MenuCategory[]>([])
const draggedOverIndex = ref<number | null>(null)

// Preview panel state
const showPreview = ref(false)
const currentTheme = ref(createDefaultTheme())
const displaySettings = ref({
  showImages: true,
  showPrices: true,
  showDescriptions: true
})
const restaurant = computed(() => ({
  name: 'Restaurant Preview',
  logoUrl: null
}))

const showStyleEditor = ref(false)
const styleEditorCategory = ref<MenuCategory | null>(null)

const showItemDialog = ref(false)
const showEditItemDialog = ref(false)
const showCategoryDialog = ref(false)
const showExportDialog = ref(false)
const selectedCategory = ref<MenuCategory | null>(null)
const editingItem = ref<MenuItem | null>(null)
const editingCategory = ref<MenuCategory | null>(null)
const activeLanguage = ref<'en' | 'ar'>('en')
const categoryForm = ref({
  name: '',
  translations: {
    ar: {
      name: ''
    }
  }
})
const newItem = ref({
  name: '',
  description: '',
  price: 0,
  imageUrl: '',
  isAvailable: true,
  displayOrder: 1,
  translations: {
    ar: {
      name: '',
      description: ''
    }
  }
})

// Drag & Drop handlers
const {
  handleDragStart,
  handleDragOver,
  handleDrop
} = useDragDrop(categories.value, async (reorderedCategories) => {
  categories.value = reorderedCategories
  await saveReorder()
})

const saveReorder = async () => {
  if (!authStore.restaurantId) return

  saving.value = true
  try {
    const api = useApi()
    await api.post(`/restaurants/${authStore.restaurantId}/categories/reorder`,
      categories.value.map((c, i) => ({ id: c.id, displayOrder: i + 1 }))
    )

    showSavedFeedback()
  } catch (error) {
    toast.error('Failed to reorder categories')
  } finally {
    saving.value = false
  }
}

const reorderItems = async (categoryId: string, reorderedItems: MenuItem[]) => {
  saving.value = true
  try {
    const api = useApi()
    await api.post(`/categories/${categoryId}/items/reorder`,
      reorderedItems.map((item, i) => ({ id: item.id, displayOrder: i + 1 }))
    )

    showSavedFeedback()
  } catch (error) {
    toast.error('Failed to reorder items')
  } finally {
    saving.value = false
  }
}

const openCategoryDialog = (category?: MenuCategory) => {
  if (category) {
    // Edit mode
    editingCategory.value = category
    categoryForm.value = {
      name: category.name,
      translations: {
        ar: {
          name: category.translations?.ar?.name || ''
        }
      }
    }
  } else {
    // Create mode
    editingCategory.value = null
    categoryForm.value = {
      name: '',
      translations: {
        ar: {
          name: ''
        }
      }
    }
  }
  activeLanguage.value = 'en'
  showCategoryDialog.value = true
}

const closeCategoryDialog = () => {
  showCategoryDialog.value = false
  editingCategory.value = null
}

const saveCategoryWithTranslations = async () => {
  if (!categoryForm.value.name) {
    toast.error('Please enter a category name')
    return
  }

  try {
    const translations: any = {}
    if (categoryForm.value.translations.ar.name) {
      translations.ar = {
        name: categoryForm.value.translations.ar.name
      }
    }

    if (editingCategory.value) {
      // Update existing category
      await restaurantStore.updateCategory(
        editingCategory.value.id,
        {
          name: categoryForm.value.name,
          displayOrder: editingCategory.value.displayOrder,
          parentCategoryId: editingCategory.value.parentCategoryId,
          translations
        },
        authStore.restaurantId || ''
      )
      toast.success('Category updated successfully')
    } else {
      // Create new category
      await restaurantStore.createCategory(
        authStore.restaurantId || '',
        {
          name: categoryForm.value.name,
          displayOrder: categories.value.length + 1,
          translations
        }
      )
      toast.success('Category created successfully')
    }

    await loadCategories()
    closeCategoryDialog()
  } catch (error: any) {
    toast.error(error.message || 'Failed to save category')
  }
}

const addCategory = async () => {
  openCategoryDialog()
}

const updateCategory = async (category: MenuCategory) => {
  saving.value = true
  try {
    await restaurantStore.updateCategory(
      category.id,
      {
        name: category.name,
        displayOrder: category.displayOrder,
        parentCategoryId: category.parentCategoryId,
        translations: category.translations
      },
      authStore.restaurantId || ''
    )

    showSavedFeedback()
  } catch (error) {
    toast.error('Failed to update category')
  } finally {
    saving.value = false
  }
}

const deleteCategory = async (categoryId: string) => {
  if (!confirm('Are you sure you want to delete this category? All items will be deleted.')) {
    return
  }

  try {
    await restaurantStore.deleteCategory(categoryId, authStore.restaurantId || '')
    await loadCategories()
  } catch (error) {
    toast.error('Failed to delete category')
  }
}

const openStyleEditor = (category: MenuCategory) => {
  styleEditorCategory.value = category
  showStyleEditor.value = true
}

const closeStyleEditor = () => {
  showStyleEditor.value = false
  styleEditorCategory.value = null
}

const handleStyleSave = async (style: any) => {
  // Style is already saved by the editor component
  showSavedFeedback()
  await loadCategories()
}

const openItemDialog = (category: MenuCategory) => {
  selectedCategory.value = category
  activeLanguage.value = 'en'
  newItem.value = {
    name: '',
    description: '',
    price: 0,
    imageUrl: '',
    isAvailable: true,
    displayOrder: (category.items?.length || 0) + 1,
    translations: {
      ar: {
        name: '',
        description: ''
      }
    }
  }
  showItemDialog.value = true
}

const closeItemDialog = () => {
  showItemDialog.value = false
  selectedCategory.value = null
}

const openEditItemDialog = (item: MenuItem, category: MenuCategory) => {
  selectedCategory.value = category
  editingItem.value = item
  activeLanguage.value = 'en'

  // Pre-fill the form with existing values
  newItem.value = {
    name: item.name,
    description: item.description || '',
    price: item.price,
    imageUrl: item.imageUrl || '',
    isAvailable: item.isAvailable,
    displayOrder: item.displayOrder,
    translations: {
      ar: {
        name: item.translations?.ar?.name || '',
        description: item.translations?.ar?.description || ''
      }
    }
  }
  showEditItemDialog.value = true
}

const closeEditItemDialog = () => {
  showEditItemDialog.value = false
  selectedCategory.value = null
  editingItem.value = null
}

const saveEditedItem = async () => {
  if (!selectedCategory.value || !editingItem.value || !newItem.value.name) {
    toast.error('Please fill in the required fields')
    return
  }

  try {
    // Build translations object
    const translations: any = {}

    // Add Arabic translations if provided
    if (newItem.value.translations.ar.name || newItem.value.translations.ar.description) {
      translations.ar = {}
      if (newItem.value.translations.ar.name) {
        translations.ar.name = newItem.value.translations.ar.name
      }
      if (newItem.value.translations.ar.description) {
        translations.ar.description = newItem.value.translations.ar.description
      }
    }

    await restaurantStore.updateMenuItem(editingItem.value.id, {
      name: newItem.value.name,
      description: newItem.value.description || null,
      price: newItem.value.price,
      imageUrl: newItem.value.imageUrl || null,
      isAvailable: newItem.value.isAvailable,
      displayOrder: newItem.value.displayOrder,
      translations
    })

    toast.success('Item updated successfully')
    await loadCategories()
    closeEditItemDialog()
  } catch (error: any) {
    toast.error(error.message || 'Failed to update item')
  }
}

const saveNewItem = async () => {
  if (!selectedCategory.value || !newItem.value.name) {
    toast.error('Please fill in the required fields')
    return
  }

  try {
    // Build translations object
    const translations: any = {}

    // Add Arabic translations if provided
    if (newItem.value.translations.ar.name || newItem.value.translations.ar.description) {
      translations.ar = {}
      if (newItem.value.translations.ar.name) {
        translations.ar.name = newItem.value.translations.ar.name
      }
      if (newItem.value.translations.ar.description) {
        translations.ar.description = newItem.value.translations.ar.description
      }
    }

    await restaurantStore.createMenuItem(selectedCategory.value.id, {
      name: newItem.value.name,
      description: newItem.value.description || null,
      price: newItem.value.price,
      imageUrl: newItem.value.imageUrl || null,
      isAvailable: newItem.value.isAvailable,
      displayOrder: newItem.value.displayOrder,
      translations
    })

    toast.success('Item added successfully')
    await loadCategories()
    closeItemDialog()
  } catch (error: any) {
    toast.error(error.message || 'Failed to create item')
  }
}

const handleExported = () => {
  showSavedFeedback()
}

const loadCategories = async () => {
  if (!authStore.restaurantId) return

  loading.value = true
  try {
    const loadedCategories = await restaurantStore.fetchCategories(authStore.restaurantId)
    categories.value = loadedCategories
  } catch (error) {
    toast.error('Failed to load categories')
  } finally {
    loading.value = false
  }
}

const showSavedFeedback = () => {
  saved.value = true
  setTimeout(() => {
    saved.value = false
  }, 2000)
}

onMounted(async () => {
  await loadCategories()
})
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

/* Slide fade animation for language toggle */
.slide-fade-enter-active {
  transition: all 0.3s ease-out;
}

.slide-fade-leave-active {
  transition: all 0.2s ease-in;
}

.slide-fade-enter-from {
  opacity: 0;
  transform: translateX(20px);
}

.slide-fade-leave-to {
  opacity: 0;
  transform: translateX(-20px);
}

/* Custom scrollbar for dark theme */
.custom-scrollbar::-webkit-scrollbar {
  width: 8px;
}

.custom-scrollbar::-webkit-scrollbar-track {
  background: rgba(255, 255, 255, 0.05);
  border-radius: 4px;
}

.custom-scrollbar::-webkit-scrollbar-thumb {
  background: rgba(147, 51, 234, 0.5);
  border-radius: 4px;
}

.custom-scrollbar::-webkit-scrollbar-thumb:hover {
  background: rgba(147, 51, 234, 0.7);
}
</style>