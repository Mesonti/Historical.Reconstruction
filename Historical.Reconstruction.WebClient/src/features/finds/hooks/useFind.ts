import { useQuery } from '@tanstack/react-query'
import { getFind } from '../api/findsApi.ts'

export function useFind(id: string | undefined) {
  return useQuery({
    queryKey: ['finds', id],
    queryFn: () => getFind(id!),
    enabled: Boolean(id),
  })
}
