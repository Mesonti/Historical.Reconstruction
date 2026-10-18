import { useQuery } from '@tanstack/react-query'
import { getCategories, getRegions } from '../api/findsApi.ts'

export function useRegions() {
  return useQuery({
    queryKey: ['regions'],
    queryFn: getRegions,
  })
}

export function useCategories() {
  return useQuery({
    queryKey: ['categories'],
    queryFn: getCategories,
  })
}
