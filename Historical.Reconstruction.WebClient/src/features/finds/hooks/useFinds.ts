import { useQuery } from '@tanstack/react-query'
import { getFinds } from '../api/findsApi.ts'
import type { FindFilters } from '../types.ts'

export function useFinds(filters: FindFilters) {
  return useQuery({
    queryKey: ['finds', filters],
    queryFn: () => getFinds(filters),
  })
}
