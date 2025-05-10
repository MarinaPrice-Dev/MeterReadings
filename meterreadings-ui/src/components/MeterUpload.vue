<template>
  <div class="upload-form">
    <h2>Upload Meter Readings</h2>
    <input type="file" @change="handleFileChange" accept=".csv" />
    <button @click="uploadFile" :disabled="!file">Upload</button>

    <div v-if="result">
      <p><strong>Success:</strong> {{ result.successfulReadings }}</p>
      <p><strong>Failed:</strong> {{ result.failedReadings }}</p>
      <p><strong>Errors:</strong></p>
      <ul v-if="result.errors && result.errors.length > 0">
        <li v-for="(error, index) in result.errors" :key="index" style="color: red">
          {{ error }}
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import axios from 'axios'

const file = ref(null)
const result = ref(null)

function handleFileChange(event) {
  file.value = event.target.files[0]
}

async function uploadFile() {
  if (!file.value) return

  const formData = new FormData()
  formData.append('file', file.value)

  try {
    const response = await axios.post('http://localhost:8080/MeterReadingUpload', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    })
    result.value = response.data
  } catch (error) {
    alert('Upload failed: ' + error.message)
  }
}
</script>

<style scoped>
.upload-form {
  margin: 50px;
  padding: 1rem;
  border: 1px solid #ccc;
  border-radius: 8px;
}
</style>
