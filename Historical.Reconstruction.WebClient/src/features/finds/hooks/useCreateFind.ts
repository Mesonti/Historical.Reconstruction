import { useMutation, useQueryClient } from '@tanstack/react-query'
import { createFind } from '../api/findsApi.ts'

export function useCreateFind() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: createFind,
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['finds'] })
    },
  })
}
