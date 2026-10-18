import type { FormEvent } from 'react'
import { useSearchParams } from 'react-router-dom'
import { FindCard } from '../features/finds/components/FindCard.tsx'
import { useFinds } from '../features/finds/hooks/useFinds.ts'
import type { FindFilters } from '../features/finds/types.ts'

const PAGE_SIZE = 6
const PAGE_SIZE_OPTIONS = [2, 6, 12]

export function FindsPage() {
  const [searchParams, setSearchParams] = useSearchParams()
  const filters = getFilters(searchParams)
  const { data, error, isError, isPending, refetch } = useFinds(filters)
  const finds = data?.items ?? []

  function handlePageSizeSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const formData = new FormData(event.currentTarget)
    const pageSize = String(formData.get('pageSize') ?? PAGE_SIZE)
    const nextParams = new URLSearchParams()

    if (pageSize !== PAGE_SIZE.toString()) nextParams.set('pageSize', pageSize)
    setSearchParams(nextParams)
  }

  function changePage(page: number) {
    const nextParams = new URLSearchParams(searchParams)

    if (page <= 1) {
      nextParams.delete('page')
    } else {
      nextParams.set('page', page.toString())
    }

    setSearchParams(nextParams)
  }

  return (
    <main className="catalog">
      <header className="catalog__header">
        <p className="catalog__eyebrow">Цифровой архив</p>
        <div className="catalog__title-row">
          <div>
            <h1>Исторические находки</h1>
            <p className="catalog__description">
              Каталог археологических предметов, мест и эпох.
            </p>
          </div>
          {!isPending && !isError && data && (
            <span className="catalog__count">
              {data.totalCount} {getFindsCountLabel(data.totalCount)}
            </span>
          )}
        </div>
      </header>

      <form
        className="find-filters find-filters--compact"
        key={filters.pageSize}
        onSubmit={handlePageSizeSubmit}
      >
        <label>
          <span>На странице</span>
          <select name="pageSize" defaultValue={filters.pageSize}>
            {PAGE_SIZE_OPTIONS.map((option) => (
              <option key={option} value={option}>
                {option}
              </option>
            ))}
          </select>
        </label>
        <div className="find-filters__actions">
          <button className="find-filters__submit" type="submit">
            Применить
          </button>
        </div>
      </form>

      {isPending && <FindsSkeleton />}

      {isError && (
        <section className="catalog-state" role="alert">
          <span className="catalog-state__icon" aria-hidden="true">
            !
          </span>
          <h2>Не удалось загрузить находки</h2>
          <p>{getErrorMessage(error)}</p>
          <button type="button" onClick={() => void refetch()}>
            Попробовать снова
          </button>
        </section>
      )}

      {!isPending && !isError && finds.length === 0 && (
        <section className="catalog-state">
          <span className="catalog-state__icon" aria-hidden="true">
            0
          </span>
          <h2>Каталог пока пуст</h2>
          <p>В API пока нет находок для отображения.</p>
        </section>
      )}

      {!isPending && !isError && finds.length > 0 && data && (
        <>
          <section className="finds-grid" aria-label="Список исторических находок">
            {finds.map((find) => (
              <FindCard key={find.id} find={find} />
            ))}
          </section>

          {data.totalPages > 1 && (
            <nav className="pagination" aria-label="Навигация по страницам">
              <button
                type="button"
                disabled={data.page <= 1}
                onClick={() => changePage(data.page - 1)}
              >
                ← Назад
              </button>
              <span>
                Страница <strong>{data.page}</strong> из {data.totalPages}
              </span>
              <button
                type="button"
                disabled={data.page >= data.totalPages}
                onClick={() => changePage(data.page + 1)}
              >
                Дальше →
              </button>
            </nav>
          )}
        </>
      )}
    </main>
  )
}

function FindsSkeleton() {
  return (
    <section className="finds-grid" aria-label="Загрузка находок" aria-busy="true">
      {[1, 2, 3].map((item) => (
        <article className="find-card find-card--skeleton" key={item}>
          <div className="skeleton skeleton--media" />
          <div className="find-card__body">
            <div className="skeleton skeleton--label" />
            <div className="skeleton skeleton--title" />
            <div className="skeleton skeleton--text" />
            <div className="skeleton skeleton--text skeleton--short" />
          </div>
        </article>
      ))}
    </section>
  )
}

function getFilters(searchParams: URLSearchParams): FindFilters {
  return {
    page: parsePositiveInteger(searchParams.get('page')),
    pageSize: parsePageSize(searchParams.get('pageSize')),
  }
}

function parsePositiveInteger(value: string | null) {
  const parsed = Number.parseInt(value ?? '', 10)
  return Number.isFinite(parsed) && parsed > 0 ? parsed : 1
}

function parsePageSize(value: string | null) {
  const parsed = Number.parseInt(value ?? '', 10)
  return PAGE_SIZE_OPTIONS.includes(parsed) ? parsed : PAGE_SIZE
}

function getFindsCountLabel(count: number) {
  const lastTwoDigits = count % 100
  const lastDigit = count % 10

  if (lastTwoDigits >= 11 && lastTwoDigits <= 14) return 'находок'
  if (lastDigit === 1) return 'находка'
  if (lastDigit >= 2 && lastDigit <= 4) return 'находки'

  return 'находок'
}

function getErrorMessage(error: Error) {
  return error.message || 'Убедитесь, что backend запущен на порту 5145.'
}
