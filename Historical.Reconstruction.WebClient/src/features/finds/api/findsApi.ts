import { httpClient } from '../../../shared/api/httpClient.ts'
import type {
  CreateFindRequest,
  FindCategory,
  FindFilters,
  FindImage,
  HistoricalFind,
  PagedResponse,
  Region,
} from '../types.ts'

export function getFinds(filters: FindFilters) {
  const searchParams = new URLSearchParams({
    page: filters.page.toString(),
    pageSize: filters.pageSize.toString(),
  })

  if (filters.query) searchParams.set('query', filters.query)
  if (filters.country) searchParams.set('country', filters.country)
  if (filters.regionId) searchParams.set('regionId', filters.regionId)
  if (filters.yearFrom !== undefined) {
    searchParams.set('yearFrom', filters.yearFrom.toString())
  }
  if (filters.yearTo !== undefined) {
    searchParams.set('yearTo', filters.yearTo.toString())
  }

  return httpClient<PagedResponse<HistoricalFind>>(
    `/api/finds?${searchParams.toString()}`,
  )
}

export function getFind(id: string) {
  return httpClient<HistoricalFind>(`/api/finds/${id}`)
}

export function getRegions() {
  return httpClient<Region[]>('/api/regions')
}

export function getCategories() {
  return httpClient<FindCategory[]>('/api/categories')
}

export function createFind(request: CreateFindRequest) {
  return httpClient<HistoricalFind>('/api/finds', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(request),
  })
}

export function uploadFindImage(findId: string, file: File, caption: string) {
  const formData = new FormData()
  formData.set('file', file)
  if (caption.trim()) formData.set('caption', caption.trim())

  return httpClient<FindImage>(`/api/finds/${findId}/images`, {
    method: 'POST',
    body: formData,
  })
}
