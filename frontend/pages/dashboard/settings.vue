<template>
  <NuxtLayout name="dashboard">
    <div class="space-y-6">
      <!-- Header -->
      <div>
        <h1 class="text-2xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent">Restaurant Settings</h1>
        <p class="mt-1 text-gray-300">Manage your restaurant's basic information and preferences</p>
      </div>

      <!-- Error State -->
      <div v-if="!authStore.restaurantId" class="p-6 text-center bg-red-500/20 backdrop-blur-sm rounded-xl border border-red-500/30">
        <p class="font-semibold text-red-300">No restaurant found</p>
        <p class="mt-2 text-sm text-red-400">Please make sure you're logged in as a restaurant owner</p>
      </div>

      <!-- Loading State -->
      <div v-else-if="loading" class="flex justify-center py-12">
        <div class="inline-flex h-12 w-12 items-center justify-center rounded-full border-4 border-purple-500/30 border-t-purple-500 animate-spin"></div>
      </div>

      <!-- Settings Content -->
      <div v-else class="space-y-6">
        <!-- Restaurant Info -->
        <Card>
          <template #header>
            <h2 class="text-lg font-semibold text-neutral-900">Restaurant Information</h2>
          </template>
          <div class="space-y-6">
            <!-- Name -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Input
                v-model="form.name"
                label="Restaurant Name"
                required
                placeholder="My Restaurant"
              />
              <Input
                v-model="form.nameAr"
                label="Restaurant Name (Arabic)"
                placeholder="مطعمي"
              />
            </div>

            <!-- Logo Upload -->
            <div>
              <label class="block text-sm font-medium text-gray-200 mb-2">
                Logo
              </label>
              <FileUpload
                v-model="form.logoUrl"
                label="Restaurant Logo"
                accept="image/*"
                :max-size="5"
                :disabled="uploadingLogo"
                :uploading="uploadingLogo"
                @upload="handleLogoUpload"
              />
            </div>

            <!-- Contact Information -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Input
                v-model="form.contactPhone"
                label="Phone Number"
                type="tel"
                placeholder="+1 234 567 8900"
              />
              <Input
                v-model="form.contactEmail"
                label="Email Address"
                type="email"
                placeholder="info@restaurant.com"
              />
            </div>

            <!-- Address -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-200 mb-1">
                  Address
                </label>
                <textarea
                  v-model="form.address"
                  rows="3"
                  class="block w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 sm:text-sm px-4 py-2"
                  placeholder="123 Main St, City, Country"
                ></textarea>
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-200 mb-1">
                  Address (Arabic)
                </label>
                <textarea
                  v-model="form.addressAr"
                  rows="3"
                  class="block w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30 sm:text-sm px-4 py-2"
                  placeholder="١٢٣ شارع الرئيسي، المدينة، الدولة"
                  dir="rtl"
                ></textarea>
              </div>
            </div>
          </div>
        </Card>

        <!-- Localization -->
        <Card>
          <template #header>
            <h2 class="text-lg font-semibold text-white">Localization</h2>
          </template>
          <div class="grid grid-cols-1 gap-6 md:grid-cols-2">
            <div>
              <label class="block mb-2 text-sm font-medium text-gray-200">
                Currency
              </label>
              <select
                v-model="form.currency"
                class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30"
              >
                <option value="USD" class="bg-slate-800">USD</option>
                <option value="IQD" class="bg-slate-800">IQD</option>
              
              </select>
            </div>

            <div>
              <label class="block mb-2 text-sm font-medium text-gray-200">
                Default Language
              </label>
              <select
                v-model="form.defaultLanguage"
                class="block px-4 py-2.5 w-full rounded-xl border shadow-sm transition-all duration-200 bg-white/10 backdrop-blur-sm border-white/20 text-white focus:border-transparent focus:ring-2 focus:ring-purple-500 hover:bg-white/15 hover:border-white/30"
              >
                <option value="en" class="bg-slate-800">English</option>
                <option value="ar" class="bg-slate-800">Arabic</option>
                <option value="fr" class="bg-slate-800">French</option>
                <option value="es" class="bg-slate-800">Spanish</option>
              </select>
            </div>
          </div>
        </Card>

        <!-- Save Button -->
        <div class="flex gap-3 justify-end pt-4 border-t border-white/10">
          <UiButton @click="loadSettings" variant="secondary">
            Cancel
          </UiButton>
          <UiButton @click="saveSettings" :loading="saving" variant="primary">
            Save Changes
          </UiButton>
        </div>
      </div>
    </div>
  </NuxtLayout>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAuthStore } from '~/stores/auth'
import Card from '~/components/ui/Card.vue'
import Input from '~/components/ui/Input.vue'
import UiButton from '~/components/ui/Button.vue'
import FileUpload from '~/components/ui/FileUpload.vue'

definePageMeta({
  layout: false,
  middleware: ['owner']
})

const authStore = useAuthStore()
const toast = useToast()
const api = useApi()

const loading = ref(false)
const saving = ref(false)
const uploadingLogo = ref(false)

interface RestaurantForm {
  name: string
  nameAr: string
  logoUrl: string
  contactPhone: string
  contactEmail: string
  address: string
  addressAr: string
  currency: string
  defaultLanguage: string
}

const form = ref<RestaurantForm>({
  name: '',
  nameAr: '',
  logoUrl: '',
  contactPhone: '',
  contactEmail: '',
  address: '',
  addressAr: '',
  currency: 'USD',
  defaultLanguage: 'en'
})

const loadSettings = async () => {
  if (!authStore.restaurantId) {
    toast.error('No restaurant ID found')
    return
  }

  loading.value = true
  try {
    // Fetch restaurant details
    const response = await api.get(`/restaurants/${authStore.restaurantId}`)
    const restaurant = response.data

    // Parse translations
    let translations: any = {}
    if (restaurant.translations) {
      try {
        translations = typeof restaurant.translations === 'string' 
          ? JSON.parse(restaurant.translations)
          : restaurant.translations
      } catch (e) {
        console.error('Failed to parse translations:', e)
      }
    }

    form.value = {
      name: restaurant.name || '',
      nameAr: translations.ar?.name || '',
      logoUrl: restaurant.logoUrl || '',
      contactPhone: restaurant.contactPhone || '',
      contactEmail: restaurant.contactEmail || '',
      address: restaurant.address || '',
      addressAr: translations.ar?.address || '',
      currency: restaurant.currency || 'USD',
      defaultLanguage: restaurant.defaultLanguage || 'en'
    }
  } catch (error: any) {
    console.error('Failed to load settings:', error)
    toast.error('Failed to load settings')
  } finally {
    loading.value = false
  }
}

const handleLogoUpload = async (file: File) => {
  uploadingLogo.value = true
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
      form.value.logoUrl = response.data.data
      toast.success('Logo uploaded successfully!')
    } else {
      throw new Error(response.data.message || 'Upload failed')
    }
  } catch (error: any) {
    toast.error('Failed to upload logo')
    const errorMessage = error?.response?.data?.message || error?.response?.data?.error || error?.message || 'Failed to upload logo'
    toast.error(errorMessage)
    form.value.logoUrl = ''
  } finally {
    uploadingLogo.value = false
  }
}

const saveSettings = async () => {
  if (!authStore.restaurantId) return

  saving.value = true
  try {
    // Prepare translations object
    const translations = {
      en: {
        name: form.value.name,
        address: form.value.address
      },
      ar: {
        name: form.value.nameAr || form.value.name,
        address: form.value.addressAr || form.value.address
      }
    }

    // Prepare update payload
    const payload = {
      name: form.value.name,
      logoUrl: form.value.logoUrl || null,
      contactPhone: form.value.contactPhone || null,
      contactEmail: form.value.contactEmail || null,
      address: form.value.address || null,
      translations: JSON.stringify(translations),
      currency: form.value.currency,
      defaultLanguage: form.value.defaultLanguage
    }

    await api.put(`/restaurants/${authStore.restaurantId}`, payload)
    toast.success('Settings saved successfully!')
  } catch (error: any) {
    console.error('Failed to save settings:', error)
    toast.error(error.response?.data?.message || 'Failed to save settings')
  } finally {
    saving.value = false
  }
}

onMounted(async () => {
  if (!authStore.restaurantId) {
    toast.error('No restaurant ID - cannot load settings')
    return
  }

  await loadSettings()
})
</script>
