<template>
  <div v-for="category in categories" :key="category.id" class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl p-4 hover:bg-white/10 hover:border-white/20 transition-all duration-200">
    <div class="flex items-start justify-between">
      <div>
        <h5 class="text-lg font-semibold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent">{{ category.localizedName || category.name }}</h5>
        <p class="text-sm text-gray-400">Display Order: {{ category.displayOrder }}</p>
      </div>
      <div class="flex flex-wrap items-center gap-2">
        <button
          @click="onAddSubcategory(category.id)"
          class="inline-flex items-center gap-1 px-3 py-1.5 text-sm bg-green-500/20 text-green-300 hover:bg-green-500/30 border border-green-500/30 hover:border-green-500/40 rounded-xl font-medium transition-all duration-200"
        >
          <svg class="h-4 w-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
          </svg>
          {{ t('menu.addCategory') }}
        </button>
        <button
          @click="onAddItem ? onAddItem(category.id) : onToggleItemForm(category.id)"
          class="inline-flex items-center gap-1 px-3 py-1.5 text-sm bg-purple-500/20 text-purple-300 hover:bg-purple-500/30 border border-purple-500/30 hover:border-purple-500/40 rounded-xl font-medium transition-all duration-200"
        >
          <svg class="h-4 w-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
          </svg>
          {{ t('menu.addItem') }}
        </button>
        <button
          @click="onEditCategory(category)"
          class="inline-flex items-center gap-1 px-3 py-1.5 text-sm bg-blue-500/20 text-blue-300 hover:bg-blue-500/30 border border-blue-500/30 hover:border-blue-500/40 rounded-xl font-medium transition-all duration-200"
        >
          <svg class="h-4 w-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
          </svg>
          {{ t('common.edit') }}
        </button>
        <button
          @click="onDeleteCategory(category)"
          class="inline-flex items-center gap-1 px-3 py-1.5 text-sm bg-red-500/20 text-red-300 hover:bg-red-500/30 border border-red-500/30 hover:border-red-500/40 rounded-xl font-medium transition-all duration-200"
        >
          <svg class="h-4 w-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
          </svg>
          {{ t('common.delete') }}
        </button>
      </div>
    </div>

    <div v-if="category.items.length" class="mt-3 space-y-2">
      <div
        v-for="item in category.items"
        :key="item.id"
        class="flex items-center justify-between bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl px-3 py-2 text-sm hover:bg-white/10 transition-all duration-200"
      >
        <div>
          <p class="font-medium text-white">{{ item.localizedName || item.name }}</p>
          <p v-if="item.localizedDescription || item.description" class="text-xs text-gray-400">
            {{ item.localizedDescription || item.description }}
          </p>
        </div>
        <div class="text-right">
          <p class="font-semibold text-purple-400">{{ formatPrice(item.price) }}</p>
          <p class="text-xs text-gray-500">Order: {{ item.displayOrder }}</p>
        </div>
      </div>
    </div>
    <div v-else class="mt-3 text-sm text-gray-400 italic">
      {{ t('menu.emptyCategoryMessage') }}
    </div>

    <div v-if="showItemForms[category.id]" class="mt-4 border-t border-white/10 pt-4">
      <h6 class="font-medium text-white mb-3">{{ t('menu.addItem') }}</h6>
      <div class="space-y-3">
        <!-- English Content -->
        <div class="space-y-3">
          <div class="flex items-center gap-2 mb-2">
            <svg class="w-4 h-4 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
            </svg>
            <span class="text-xs font-semibold text-purple-400">English Content</span>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-200 mb-1">{{ t('menu.itemName') }} (English)</label>
            <input
              v-model="itemForms[category.id].name"
              type="text"
              class="block w-full px-3 py-2 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 transition-all duration-200"
              placeholder="e.g., Grilled Chicken"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-200 mb-1">{{ t('menu.description') }} (English)</label>
            <textarea
              v-model="itemForms[category.id].description"
              class="block w-full px-3 py-2 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 transition-all duration-200"
              rows="2"
              placeholder="e.g., Tender grilled chicken breast"
            ></textarea>
          </div>
        </div>

        <!-- Divider -->
        <div class="border-t border-white/10"></div>

        <!-- Arabic Content -->
        <div class="space-y-3">
          <div class="flex items-center gap-2 mb-2">
            <svg class="w-4 h-4 text-pink-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
            </svg>
            <span class="text-xs font-semibold text-pink-400">المحتوى العربي</span>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-200 mb-1">اسم العنصر (عربي)</label>
            <input
              v-model="itemForms[category.id].translations.ar.name"
              type="text"
              dir="rtl"
              class="block w-full px-3 py-2 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-pink-500 hover:bg-white/15 hover:border-white/30 transition-all duration-200 text-right"
              placeholder="مثال: دجاج مشوي"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-200 mb-1">الوصف (عربي)</label>
            <textarea
              v-model="itemForms[category.id].translations.ar.description"
              class="block w-full px-3 py-2 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-pink-500 hover:bg-white/15 hover:border-white/30 transition-all duration-200 text-right"
              rows="2"
              dir="rtl"
              placeholder="مثال: صدر دجاج مشوي طري"
            ></textarea>
          </div>
        </div>

        <!-- Divider -->
        <div class="border-t border-white/10"></div>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-3">
          <div>
            <label class="block text-sm font-medium text-gray-200 mb-1">{{ t('menu.price') }}</label>
            <input
              v-model.number="itemForms[category.id].price"
              type="number"
              step="0.01"
              min="0"
              class="block w-full px-3 py-2 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 transition-all duration-200"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-200 mb-1">{{ t('menu.sortOrder') }}</label>
            <input
              v-model.number="itemForms[category.id].displayOrder"
              type="number"
              class="block w-full px-3 py-2 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 transition-all duration-200"
            />
          </div>
          <div class="flex items-center mt-6">
            <input
              v-model="itemForms[category.id].isAvailable"
              type="checkbox"
              :id="'available-' + category.id"
              class="h-4 w-4 rounded text-purple-500 bg-white/10 border-white/20 focus:ring-purple-500 focus:ring-offset-slate-900"
            />
            <label :for="'available-' + category.id" class="ml-2 text-sm text-gray-200">{{ t('menu.isAvailable') }}</label>
          </div>
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-200 mb-1">{{ t('menu.imageUrl') }}</label>
          <input
            v-model="itemForms[category.id].imageUrl"
            type="text"
            class="block w-full px-3 py-2 bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 transition-all duration-200"
          />
        </div>
        <button
          @click="onSaveItem(category.id)"
          class="bg-gradient-to-r from-purple-500 to-pink-500 hover:from-purple-600 hover:to-pink-600 text-white px-4 py-2 rounded-xl text-sm font-medium shadow-lg hover:shadow-purple-500/50 transition-all duration-200 transform hover:scale-[1.02] active:scale-[0.98]"
        >
          {{ t('common.save') }}
        </button>
      </div>
    </div>

    <div v-if="category.children.length" class="mt-4 space-y-3 border-l-2 border-purple-500/30 pl-4">
      <MenuCategoryTree
        :categories="category.children"
        :showItemForms="showItemForms"
        :itemForms="itemForms"
        :onToggleItemForm="onToggleItemForm"
        :onSaveItem="onSaveItem"
        :onAddSubcategory="onAddSubcategory"
        :onEditCategory="onEditCategory"
        :onDeleteCategory="onDeleteCategory"
        :onAddItem="onAddItem"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { MenuCategory } from '@/stores/restaurant';

defineOptions({
  name: 'MenuCategoryTree',
});

interface MenuItemForm {
  name: string;
  description?: string | null;
  price: number;
  imageUrl?: string | null;
  isAvailable: boolean;
  displayOrder: number;
  translations: {
    ar: {
      name: string;
      description: string;
    };
  };
}

defineProps<{
  categories: MenuCategory[];
  showItemForms: Record<string, boolean>;
  itemForms: Record<string, MenuItemForm>;
  onToggleItemForm: (categoryId: string) => void;
  onSaveItem: (categoryId: string) => Promise<void>;
  onAddSubcategory: (categoryId: string) => void;
  onEditCategory: (category: MenuCategory) => void;
  onDeleteCategory: (category: MenuCategory) => void;
  onAddItem?: (categoryId: string) => void;
}>();

const { t, locale } = useI18n({ useScope: 'global' });
const runtimeConfig = useRuntimeConfig();
const defaultCurrency = computed(() => (runtimeConfig.public.defaultCurrency as string) || 'USD');

const formatPrice = (price: number) => {
  try {
    return new Intl.NumberFormat(locale.value, {
      style: 'currency',
      currency: defaultCurrency.value,
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }).format(price);
  } catch (error) {
    return price.toFixed(2);
  }
};
</script>

