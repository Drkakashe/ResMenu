<template>
  <NuxtLayout name="dashboard">
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <h1 class="text-2xl font-bold bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent">{{ $t('qr.title') }}</h1>
        <UiButton v-if="!qrCode" @click="generateQRCode" variant="primary" :loading="generating">
          <svg class="h-5 w-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
          </svg>
          {{ $t('qr.generate') }}
        </UiButton>
        <UiButton v-else @click="regenerateQRCode" variant="secondary" :loading="generating">
          <svg class="h-5 w-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
          </svg>
          {{ $t('qr.regenerate') }}
        </UiButton>
      </div>

      <div v-if="loading" class="flex justify-center py-12">
        <LoadingSpinner size="lg" />
      </div>

      <div v-else class="grid grid-cols-1 gap-6 lg:grid-cols-2">
        <!-- QR Code Display -->
        <Card>
          <div v-if="qrCode" class="text-center space-y-6">
            <div class="bg-gradient-to-br from-purple-500/10 via-pink-500/10 to-purple-500/10 backdrop-blur-sm border border-white/20 p-8 inline-block rounded-2xl shadow-lg">
              <img 
                :src="qrCode.imageUrl" 
                alt="QR Code" 
                class="w-full h-auto mx-auto rounded-lg max-w-md" 
              />
            </div>
            <div class="space-y-3">
              <p class="text-sm font-medium text-gray-200">{{ $t('qr.link') }}</p>
              <div class="flex items-center gap-2">
                <input
                  :value="qrCode.link"
                  readonly
                  class="flex-1 px-4 py-2 text-sm border rounded-xl bg-white/5 backdrop-blur-sm border-white/20 text-gray-300"
                />
                <button
                  @click="copyLink"
                  class="px-4 py-2 text-sm font-medium text-purple-400 hover:bg-purple-500/20 rounded-xl transition-all duration-200 border border-purple-500/30 hover:border-purple-500/40"
                >
                  <svg class="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z" />
                  </svg>
                </button>
              </div>
            </div>
            <div class="flex gap-3">
              <UiButton @click="downloadQRCode" variant="primary" class="flex-1">
                <svg class="h-5 w-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
                </svg>
                {{ $t('qr.download') }}
              </UiButton>
            </div>
          </div>
          <EmptyState
            v-else
            title="No QR Code Yet"
            description="Generate a QR code for your restaurant menu that customers can scan"
          >
            <template #action>
              <UiButton @click="generateQRCode" variant="primary" :loading="generating">
                {{ $t('qr.generate') }}
              </UiButton>
            </template>
          </EmptyState>
        </Card>

        <!-- QR Code Theme Configuration -->
        <Card>
          <div class="space-y-6">
            <h3 class="text-lg font-semibold text-white">QR Code Theme</h3>
            
            <!-- Include Logo -->
            <div class="flex items-center justify-between p-4 bg-white/5 backdrop-blur-sm rounded-xl border border-white/10">
              <div>
                <label class="text-sm font-medium text-gray-200">Include Logo</label>
                <p class="text-xs text-gray-400 mt-1">Add your restaurant logo in the center of the QR code</p>
              </div>
              <input
                v-model="themeForm.includeLogo"
                type="checkbox"
                class="w-5 h-5 rounded text-purple-500 bg-white/10 border-white/20 focus:ring-purple-500 focus:ring-offset-slate-900"
              />
            </div>

            <!-- Border Radius -->
            <div class="p-4 bg-white/5 backdrop-blur-sm rounded-xl border border-white/10">
              <label class="block text-sm font-medium text-gray-200 mb-2">
                QR Dots Border Radius: {{ themeForm.borderRadius }}%
              </label>
              <input
                v-model.number="themeForm.borderRadius"
                type="range"
                min="0"
                max="100"
                step="10"
                class="w-full h-2 bg-white/10 rounded-lg appearance-none cursor-pointer accent-purple-500"
              />
              <p class="text-xs text-gray-400 mt-1">Rounded corners for each QR code square/dot</p>
            </div>

            <!-- Christmas Theme -->
            <div class="flex items-center justify-between p-4 bg-white/5 backdrop-blur-sm rounded-xl border border-white/10">
              <div>
                <label class="text-sm font-medium text-gray-200">🎄 Christmas Theme</label>
                <p class="text-xs text-gray-400 mt-1">Add snowflakes, Santa hat, ornaments & more!</p>
              </div>
              <input
                v-model="themeForm.christmasTheme"
                type="checkbox"
                class="w-5 h-5 rounded text-purple-500 bg-white/10 border-white/20 focus:ring-purple-500 focus:ring-offset-slate-900"
              />
            </div>

            <!-- Generate Button -->
            <UiButton @click="generateQRCodeWithTheme" variant="primary" :loading="generating" class="w-full">
              <svg class="h-5 w-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
              </svg>
              {{ qrCode ? 'Regenerate with Theme' : 'Generate with Theme' }}
            </UiButton>
          </div>
        </Card>

        <!-- QR Code Info -->
        <Card>
          <div class="space-y-6">
            <div v-if="qrCode" class="p-4 bg-purple-500/20 backdrop-blur-sm rounded-xl border border-purple-500/30">
              <div class="flex items-center justify-between">
                <span class="text-sm font-medium text-purple-300">{{ $t('qr.scanCount') }}</span>
                <span class="text-2xl font-bold text-purple-400">{{ qrCode.scanCount || 0 }}</span>
              </div>
            </div>

            <div class="prose prose-sm max-w-none">
              <h4 class="text-lg font-semibold text-white mb-3">How to use your QR Code:</h4>
              <ul class="space-y-2 text-gray-300 list-disc list-inside">
                <li>Download and print the QR code in high quality</li>
                <li>Place it prominently on your tables, menus, or storefront</li>
                <li>Customers scan it with their phone camera to instantly view your menu</li>
                <li>No app download required - works with any smartphone</li>
                <li>Update your menu anytime without reprinting</li>
              </ul>
            </div>

            <div class="p-4 bg-blue-500/20 backdrop-blur-sm rounded-xl border border-blue-500/30">
              <p class="text-sm text-blue-300">
                <strong class="text-blue-400">Pro Tip:</strong> Print multiple copies and place them at different locations in your restaurant for maximum visibility.
              </p>
            </div>
          </div>
        </Card>
      </div>
    </div>
  </NuxtLayout>
</template>

<script setup lang="ts">
import QRCode from 'qrcode'

definePageMeta({
  layout: false,
  middleware: ['owner']
})

const { t } = useI18n({ useScope: 'global' })
const authStore = useAuthStore()
const restaurantStore = useRestaurantStore()
const toast = useToast()

const loading = ref(false)
const generating = ref(false)
const qrCode = ref<any>(null)
const restaurantLogoUrl = ref<string | null>(null)
const menuLink = ref<string>('')

const restaurantId = computed(() => authStore.restaurantId)

const themeForm = ref({
  includeLogo: false,
  borderRadius: 0,
  christmasTheme: false
})

// Load restaurant data and existing QR code on mount
onMounted(async () => {
  if (!restaurantId.value) {
    return
  }
  
  loading.value = true
  try {
    // Load restaurant data to get logo URL and slug
    const api = useApi()
    const restaurantResponse = await api.get(`/restaurants/${restaurantId.value}`)
    if (restaurantResponse.data) {
      restaurantLogoUrl.value = restaurantResponse.data.logoUrl || null
      const slug = restaurantResponse.data.slug
      const baseUrl = window.location.origin
      menuLink.value = `${baseUrl}/menu/${slug}`
    }
    
    // Load existing QR code from backend
    await restaurantStore.generateQRCode(restaurantId.value)
    if (restaurantStore.qrCode) {
      qrCode.value = restaurantStore.qrCode
    }
  } catch (error) {
    toast.error(t('messages.errorOccurred'))
  } finally {
    loading.value = false
  }
})

const generateQRCode = async () => {
  if (!restaurantId.value || !menuLink.value) {
    toast.error(t('messages.errorOccurred'))
    return
  }

  generating.value = true
  try {
    // Generate default QR code from backend
    await restaurantStore.generateQRCode(restaurantId.value)
    if (restaurantStore.qrCode) {
      qrCode.value = restaurantStore.qrCode
    }
    toast.success(t('qr.generated') || 'QR Code generated successfully')
  } catch (error) {
    toast.error(t('messages.errorOccurred'))
  } finally {
    generating.value = false
  }
}

const generateQRCodeWithTheme = async () => {
  if (!restaurantId.value || !menuLink.value) {
    toast.error(t('messages.errorOccurred'))
    return
  }

  generating.value = true
  try {
    // Generate QR code on frontend with themes
    const qrCodeDataUrl = await generateThemedQRCode(menuLink.value, {
      includeLogo: themeForm.value.includeLogo,
      logoUrl: restaurantLogoUrl.value,
      borderRadius: themeForm.value.borderRadius,
      christmasTheme: themeForm.value.christmasTheme
    })
    
    // Save QR code to backend
    const api = useApi()
    const response = await api.post(`/qrcode/${restaurantId.value}/update-image`, {
      imageData: qrCodeDataUrl
    })
    
    if (response.data.success) {
      // Update local QR code with backend response
      qrCode.value = response.data.data
      toast.success(t('qr.generated') || 'QR Code generated and saved successfully')
    } else {
      throw new Error(response.data.message || 'Failed to save QR code')
    }
  } catch (error) {
    console.error('Error generating QR code:', error)
    toast.error(t('messages.errorOccurred'))
  } finally {
    generating.value = false
  }
}

const generateThemedQRCode = async (
  content: string,
  options: {
    includeLogo?: boolean
    logoUrl?: string | null
    borderRadius?: number
    christmasTheme?: boolean
  }
): Promise<string> => {
  return new Promise(async (resolve, reject) => {
    try {
      // Determine colors
      const darkColor = options.christmasTheme ? '#DC143C' : '#000000' // Crimson red or black
      const lightColor = options.christmasTheme ? '#FFFFFF' : '#FFFFFF' // White
      
      // Generate QR code data
      const qrData = await QRCode.create(content, {
        errorCorrectionLevel: 'M'
      })
      
      // Create canvas with proper size (add extra space for Christmas decorations)
      const moduleSize = 12 // Size of each QR module/square
      const margin = 4 // Margin in modules
      const qrSize = qrData.modules.size
      const extraMargin = options.christmasTheme ? 40 : 0 // Extra space for Christmas decorations
      const canvasSize = (qrSize + margin * 2) * moduleSize + extraMargin * 2
      
      const canvas = document.createElement('canvas')
      canvas.width = canvasSize
      canvas.height = canvasSize
      const ctx = canvas.getContext('2d')
      
      if (!ctx) {
        reject(new Error('Could not get canvas context'))
        return
      }
      
      // Fill background
      ctx.fillStyle = options.christmasTheme ? '#F0F8FF' : lightColor // Light blue for Christmas
      ctx.fillRect(0, 0, canvasSize, canvasSize)
      
      // Draw QR code with rounded squares if border radius is specified
      const startPos = margin * moduleSize + extraMargin
      const borderRadiusPixels = options.borderRadius ? (moduleSize * options.borderRadius / 100) : 0
      
      for (let row = 0; row < qrSize; row++) {
        for (let col = 0; col < qrSize; col++) {
          if (qrData.modules.get(row, col)) {
            const x = startPos + col * moduleSize
            const y = startPos + row * moduleSize
            
            ctx.fillStyle = darkColor
            
            if (borderRadiusPixels > 0) {
              // Draw rounded rectangle
              drawRoundedRect(ctx, x, y, moduleSize, moduleSize, borderRadiusPixels)
            } else {
              // Draw regular square
              ctx.fillRect(x, y, moduleSize, moduleSize)
            }
          }
        }
      }
      
      // Apply logo if needed
      if (options.includeLogo && options.logoUrl) {
        await addLogoToQRCode(canvas, options.logoUrl, startPos, qrSize * moduleSize)
      }
      
      // Apply Christmas decorations
      if (options.christmasTheme) {
        await addChristmasDecorations(ctx, canvasSize, startPos, qrSize * moduleSize)
      }
      
      resolve(canvas.toDataURL('image/png'))
    } catch (error) {
      reject(error)
    }
  })
}

const drawRoundedRect = (
  ctx: CanvasRenderingContext2D,
  x: number,
  y: number,
  width: number,
  height: number,
  radius: number
): void => {
  ctx.beginPath()
  ctx.moveTo(x + radius, y)
  ctx.lineTo(x + width - radius, y)
  ctx.quadraticCurveTo(x + width, y, x + width, y + radius)
  ctx.lineTo(x + width, y + height - radius)
  ctx.quadraticCurveTo(x + width, y + height, x + width - radius, y + height)
  ctx.lineTo(x + radius, y + height)
  ctx.quadraticCurveTo(x, y + height, x, y + height - radius)
  ctx.lineTo(x, y + radius)
  ctx.quadraticCurveTo(x, y, x + radius, y)
  ctx.closePath()
  ctx.fill()
}

const addLogoToQRCode = async (
  canvas: HTMLCanvasElement,
  logoUrl: string,
  qrStartPos: number,
  qrSize: number
): Promise<void> => {
  return new Promise((resolve, reject) => {
    const ctx = canvas.getContext('2d')
    if (!ctx) {
      reject(new Error('Could not get canvas context'))
      return
    }
    
    const img = new Image()
    img.crossOrigin = 'anonymous'
    
    img.onload = () => {
      const logoSize = qrSize * 0.2 // 20% of QR code size
      const logoX = qrStartPos + (qrSize - logoSize) / 2
      const logoY = qrStartPos + (qrSize - logoSize) / 2
      const padding = logoSize / 6
      
      // Draw white background circle for logo
      ctx.fillStyle = '#FFFFFF'
      ctx.beginPath()
      ctx.arc(
        logoX + logoSize / 2,
        logoY + logoSize / 2,
        logoSize / 2 + padding,
        0,
        2 * Math.PI
      )
      ctx.fill()
      
      // Add shadow for logo
      ctx.shadowColor = 'rgba(0, 0, 0, 0.2)'
      ctx.shadowBlur = 10
      ctx.shadowOffsetX = 2
      ctx.shadowOffsetY = 2
      
      // Draw logo
      ctx.drawImage(img, logoX, logoY, logoSize, logoSize)
      
      // Reset shadow
      ctx.shadowColor = 'transparent'
      ctx.shadowBlur = 0
      ctx.shadowOffsetX = 0
      ctx.shadowOffsetY = 0
      
      resolve()
    }
    
    img.onerror = () => {
      // If logo fails to load, just continue without it
      resolve()
    }
    
    img.src = logoUrl
  })
}

const addChristmasDecorations = async (
  ctx: CanvasRenderingContext2D,
  canvasSize: number,
  qrStartPos: number,
  qrSize: number
): Promise<void> => {
  const qrCenterX = qrStartPos + qrSize / 2
  const qrCenterY = qrStartPos + qrSize / 2
  
  // Draw snowflakes around the QR code
  drawSnowflakes(ctx, canvasSize, qrStartPos, qrSize)
  
  // Draw Santa hat on top
  drawSantaHat(ctx, qrCenterX, qrStartPos - 20)
  
  // Draw Christmas ornaments in corners
  drawChristmasOrnaments(ctx, qrStartPos, qrSize)
  
  // Draw holly leaves
  drawHollyLeaves(ctx, canvasSize, qrStartPos, qrSize)
}

const drawSnowflakes = (
  ctx: CanvasRenderingContext2D,
  canvasSize: number,
  qrStartPos: number,
  qrSize: number
): void => {
  const snowflakes = [
    { x: 20, y: 20, size: 15 },
    { x: canvasSize - 30, y: 25, size: 12 },
    { x: 25, y: canvasSize - 30, size: 18 },
    { x: canvasSize - 25, y: canvasSize - 25, size: 14 },
    { x: qrStartPos - 15, y: qrStartPos + qrSize / 3, size: 10 },
    { x: qrStartPos + qrSize + 15, y: qrStartPos + qrSize * 2 / 3, size: 11 },
  ]
  
  ctx.fillStyle = '#ADD8E6'
  ctx.strokeStyle = '#87CEEB'
  ctx.lineWidth = 1.5
  
  snowflakes.forEach(flake => {
    // Draw snowflake with 6 branches
    for (let i = 0; i < 6; i++) {
      const angle = (Math.PI / 3) * i
      const x1 = flake.x
      const y1 = flake.y
      const x2 = flake.x + Math.cos(angle) * flake.size
      const y2 = flake.y + Math.sin(angle) * flake.size
      
      ctx.beginPath()
      ctx.moveTo(x1, y1)
      ctx.lineTo(x2, y2)
      ctx.stroke()
      
      // Add smaller branches
      const branchSize = flake.size / 3
      const x3 = x1 + Math.cos(angle) * flake.size * 0.6
      const y3 = y1 + Math.sin(angle) * flake.size * 0.6
      
      ctx.beginPath()
      ctx.moveTo(x3, y3)
      ctx.lineTo(x3 + Math.cos(angle + Math.PI / 4) * branchSize, y3 + Math.sin(angle + Math.PI / 4) * branchSize)
      ctx.stroke()
      
      ctx.beginPath()
      ctx.moveTo(x3, y3)
      ctx.lineTo(x3 + Math.cos(angle - Math.PI / 4) * branchSize, y3 + Math.sin(angle - Math.PI / 4) * branchSize)
      ctx.stroke()
    }
  })
}

const drawSantaHat = (ctx: CanvasRenderingContext2D, x: number, y: number): void => {
  // Red hat
  ctx.fillStyle = '#DC143C'
  ctx.beginPath()
  ctx.moveTo(x, y)
  ctx.lineTo(x - 25, y + 40)
  ctx.lineTo(x + 25, y + 40)
  ctx.closePath()
  ctx.fill()
  
  // White trim
  ctx.fillStyle = '#FFFFFF'
  ctx.fillRect(x - 25, y + 40, 50, 6)
  
  // White pom-pom
  ctx.beginPath()
  ctx.arc(x, y - 5, 8, 0, 2 * Math.PI)
  ctx.fill()
}

const drawChristmasOrnaments = (
  ctx: CanvasRenderingContext2D,
  qrStartPos: number,
  qrSize: number
): void => {
  const ornaments = [
    { x: qrStartPos - 25, y: qrStartPos - 25, color: '#DC143C' },
    { x: qrStartPos + qrSize + 25, y: qrStartPos - 25, color: '#FFD700' },
    { x: qrStartPos - 25, y: qrStartPos + qrSize + 25, color: '#228B22' },
    { x: qrStartPos + qrSize + 25, y: qrStartPos + qrSize + 25, color: '#4169E1' },
  ]
  
  ornaments.forEach(ornament => {
    // Draw ornament ball
    const gradient = ctx.createRadialGradient(ornament.x - 3, ornament.y - 3, 2, ornament.x, ornament.y, 12)
    gradient.addColorStop(0, '#FFFFFF')
    gradient.addColorStop(1, ornament.color)
    
    ctx.fillStyle = gradient
    ctx.beginPath()
    ctx.arc(ornament.x, ornament.y, 12, 0, 2 * Math.PI)
    ctx.fill()
    
    // Draw ornament cap
    ctx.fillStyle = '#FFD700'
    ctx.fillRect(ornament.x - 4, ornament.y - 15, 8, 4)
    
    // Draw ornament hook
    ctx.strokeStyle = '#FFD700'
    ctx.lineWidth = 2
    ctx.beginPath()
    ctx.arc(ornament.x, ornament.y - 17, 3, 0, Math.PI, true)
    ctx.stroke()
  })
}

const drawHollyLeaves = (
  ctx: CanvasRenderingContext2D,
  canvasSize: number,
  qrStartPos: number,
  qrSize: number
): void => {
  const positions = [
    { x: qrStartPos + qrSize / 4, y: qrStartPos - 30 },
    { x: qrStartPos + qrSize * 3 / 4, y: qrStartPos + qrSize + 30 },
  ]
  
  positions.forEach(pos => {
    // Draw holly leaves
    ctx.fillStyle = '#228B22'
    ctx.beginPath()
    ctx.ellipse(pos.x - 8, pos.y, 8, 4, -Math.PI / 6, 0, 2 * Math.PI)
    ctx.fill()
    
    ctx.beginPath()
    ctx.ellipse(pos.x + 8, pos.y, 8, 4, Math.PI / 6, 0, 2 * Math.PI)
    ctx.fill()
    
    // Draw berries
    ctx.fillStyle = '#DC143C'
    ctx.beginPath()
    ctx.arc(pos.x - 2, pos.y + 2, 3, 0, 2 * Math.PI)
    ctx.fill()
    
    ctx.beginPath()
    ctx.arc(pos.x + 2, pos.y + 2, 3, 0, 2 * Math.PI)
    ctx.fill()
    
    ctx.beginPath()
    ctx.arc(pos.x, pos.y + 5, 3, 0, 2 * Math.PI)
    ctx.fill()
  })
}


const regenerateQRCode = async () => {
  const confirmed = confirm('Are you sure you want to regenerate the QR code? The old one will stop working.')
  if (!confirmed) {
    return
  }
  // Use theme if any theme options are set
  if (themeForm.value.includeLogo || themeForm.value.borderRadius > 0 || themeForm.value.christmasTheme) {
    await generateQRCodeWithTheme()
  } else {
    await generateQRCode()
  }
}

const copyLink = async () => {
  if (qrCode.value?.link) {
    try {
      await navigator.clipboard.writeText(qrCode.value.link)
      toast.success('Link copied to clipboard!')
    } catch (error) {
      toast.error('Failed to copy link')
    }
  }
}

const downloadQRCode = () => {
  if (!qrCode.value?.imageUrl) {
    return
  }

  // Create a temporary link element and trigger download
  const link = document.createElement('a')
  link.href = qrCode.value.imageUrl
  link.download = `qr-code-${restaurantId.value}.png`
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
}
</script>
