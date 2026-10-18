import { useMutation, useQueryClient } from '@tanstack/react-query'
import { uploadFindImage } from '../api/findsApi.ts'

interface UploadImageVariables {
  file: File
  caption: string
}

export function useUploadFindImage(findId: string) {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: ({ file, caption }: UploadImageVariables) =>
      uploadFindImage(findId, file, caption),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['finds'] })
    },
  })
}
