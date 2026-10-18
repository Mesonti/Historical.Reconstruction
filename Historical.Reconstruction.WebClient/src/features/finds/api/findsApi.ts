import { httpClient } from '../../../shared/api/httpClient.ts'
import type {
  ApiPaginatedResponse,
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
    pageNumber: filters.page.toString(),
    pageSize: filters.pageSize.toString(),
  })

  return httpClient<ApiPaginatedResponse<ApiHistoricalFind>>(
    `/api/Find?${searchParams.toString()}`,
  ).then((response): PagedResponse<HistoricalFind> => ({
    items: response.items.map(normalizeFind),
    page: response.pagination.pageNumber,
    pageSize: response.pagination.pageSize,
    totalCount: response.pagination.totalCount,
    totalPages:
      response.pagination.pageSize > 0
        ? Math.ceil(response.pagination.totalCount / response.pagination.pageSize)
        : 0,
  }))
}

export function getFind(id: string) {
  return httpClient<ApiHistoricalFind>(`/api/Find/${id}`).then(normalizeFind)
}

export function getRegions() {
  return httpClient<Region[]>('/api/Region')
}

export function getCategories() {
  return httpClient<FindCategory[]>('/api/categories')
}

export function createFind(request: CreateFindRequest) {
  return httpClient<ApiHistoricalFind>('/api/Find', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(request),
  }).then(normalizeFind)
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

type ApiHistoricalFind = Omit<HistoricalFind, 'images'> & {
  images?: FindImage[]
}

function normalizeFind(find: ApiHistoricalFind): HistoricalFind {
  return {
    ...find,
    images: find.images ?? [],
  }
}
