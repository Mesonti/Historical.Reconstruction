export class HttpError extends Error {
  readonly status: number

  constructor(status: number, message?: string) {
    super(message ?? `API вернул ошибку ${status}`)
    this.name = 'HttpError'
    this.status = status
  }
}

export async function httpClient<T>(path: string, init?: RequestInit): Promise<T> {
  const headers = new Headers(init?.headers)
  headers.set('Accept', 'application/json')

  const response = await fetch(path, {
    ...init,
    headers,
  })

  if (!response.ok) {
    throw new HttpError(response.status, await getErrorMessage(response))
  }

  return response.json() as Promise<T>
}

async function getErrorMessage(response: Response) {
  const fallback = `API вернул ошибку ${response.status}`

  try {
    const problem = (await response.json()) as {
      title?: string
      detail?: string
      errors?: Record<string, string[]>
    }
    const validationMessage = problem.errors
      ? Object.values(problem.errors).flat().find(Boolean)
      : undefined

    return validationMessage || problem.detail || problem.title || fallback
  } catch {
    return fallback
  }
}
