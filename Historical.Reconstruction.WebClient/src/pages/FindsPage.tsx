import { useEffect, useState, type FormEvent } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { FindCard } from '../features/finds/components/FindCard.tsx'
import { useFinds } from '../features/finds/hooks/useFinds.ts'
import { useRegions } from '../features/finds/hooks/useReferenceData.ts'
import type { FindFilters, Region } from '../features/finds/types.ts'

const PAGE_SIZE = 6
const PAGE_SIZE_OPTIONS = [2, 6, 12]

export function FindsPage() {
  const [searchParams, setSearchParams] = useSearchParams()
  const filters = getFilters(searchParams)
  const regionsQuery = useRegions()
  const [selectedCountry, setSelectedCountry] = useState(filters.country ?? '')
  const [selectedRegionId, setSelectedRegionId] = useState(filters.regionId ?? '')
  const { data, error, isError, isPending, refetch } = useFinds(filters)
  const finds = data?.items ?? []
  const countries = Array.from(
    new Set(regionsQuery.data?.map((region) => region.country) ?? []),
  ).sort((first, second) => first.localeCompare(second, 'ru'))
  const availableRegions =
    regionsQuery.data?.filter(
      (region) => !selectedCountry || region.country === selectedCountry,
    ) ?? []
  const hasActiveFilters = Boolean(
    filters.query ||
      filters.country ||
      filters.regionId ||
      filters.yearFrom !== undefined ||
      filters.yearTo !== undefined,
  )

  useEffect(() => {
    setSelectedCountry(filters.country ?? '')
    setSelectedRegionId(filters.regionId ?? '')
  }, [filters.country, filters.regionId])

  function handleFiltersSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const formData = new FormData(event.currentTarget)
    const nextParams = new URLSearchParams()
    const query = String(formData.get('query') ?? '').trim()
    const country = String(formData.get('country') ?? '').trim()
    const regionId = String(formData.get('regionId') ?? '').trim()
    const yearFrom = String(formData.get('yearFrom') ?? '').trim()
    const yearTo = String(formData.get('yearTo') ?? '').trim()
    const pageSize = String(formData.get('pageSize') ?? PAGE_SIZE)

    if (query) nextParams.set('query', query)
    if (country) nextParams.set('country', country)
    if (regionId) nextParams.set('regionId', regionId)
    if (yearFrom) nextParams.set('yearFrom', yearFrom)
    if (yearTo) nextParams.set('yearTo', yearTo)
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

  function resetFilters() {
    setSelectedCountry('')
    setSelectedRegionId('')
    setSearchParams(new URLSearchParams())
  }

  function changeCountry(country: string) {
    setSelectedCountry(country)

    const selectedRegion = regionsQuery.data?.find(
      (region) => region.id === selectedRegionId,
    )
    if (selectedRegion && country && selectedRegion.country !== country) {
      setSelectedRegionId('')
    }
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
          <div className="catalog__heading-actions">
            {!isPending && !isError && data && (
              <span className="catalog__count">
                {data.totalCount} {getFindsCountLabel(data.totalCount)}
              </span>
            )}
            <Link className="catalog__create-link" to="/finds/new">
              + Добавить находку
            </Link>
          </div>
        </div>
      </header>

      <form
        className="find-filters"
        key={`${filters.query ?? ''}-${filters.yearFrom ?? ''}-${filters.yearTo ?? ''}-${filters.pageSize}`}
        onSubmit={handleFiltersSubmit}
      >
        <label className="find-filters__search">
          <span>Поиск</span>
          <input
            type="search"
            name="query"
            defaultValue={filters.query}
            placeholder="Название или описание"
          />
        </label>
        <label>
          <span>Страна</span>
          <select
            name="country"
            value={selectedCountry}
            disabled={regionsQuery.isPending || regionsQuery.isError}
            onChange={(event) => changeCountry(event.target.value)}
          >
            <option value="">Все страны</option>
            {countries.map((country) => (
              <option key={country} value={country}>
                {country}
              </option>
            ))}
          </select>
        </label>
        <label>
          <span>Регион</span>
          <select
            name="regionId"
            value={selectedRegionId}
            disabled={regionsQuery.isPending || regionsQuery.isError}
            onChange={(event) => setSelectedRegionId(event.target.value)}
          >
            <option value="">Все регионы</option>
            {availableRegions.map((region) => (
              <option key={region.id} value={region.id}>
                {getRegionLabel(region, Boolean(selectedCountry))}
              </option>
            ))}
          </select>
        </label>
        <label>
          <span>Год от</span>
          <input
            type="number"
            name="yearFrom"
            defaultValue={filters.yearFrom}
            placeholder="−1300"
          />
        </label>
        <label>
          <span>Год до</span>
          <input
            type="number"
            name="yearTo"
            defaultValue={filters.yearTo}
            placeholder="1500"
          />
        </label>
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
          {hasActiveFilters && (
            <button className="find-filters__reset" type="button" onClick={resetFilters}>
              Сбросить
            </button>
          )}
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
          <h2>{hasActiveFilters ? 'Ничего не найдено' : 'Каталог пока пуст'}</h2>
          <p>
            {hasActiveFilters
              ? 'Измените запрос или расширьте диапазон лет.'
              : 'Добавьте первую находку, и она появится здесь.'}
          </p>
          {hasActiveFilters && (
            <button type="button" onClick={resetFilters}>
              Сбросить фильтры
            </button>
          )}
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
    query: searchParams.get('query')?.trim() || undefined,
    country: searchParams.get('country')?.trim() || undefined,
    regionId: parseOptionalGuid(searchParams.get('regionId')),
    yearFrom: parseOptionalInteger(searchParams.get('yearFrom')),
    yearTo: parseOptionalInteger(searchParams.get('yearTo')),
    page: parsePositiveInteger(searchParams.get('page')),
    pageSize: parsePageSize(searchParams.get('pageSize')),
  }
}

function parseOptionalGuid(value: string | null) {
  return value && /^[0-9a-f]{8}(?:-[0-9a-f]{4}){3}-[0-9a-f]{12}$/i.test(value)
    ? value
    : undefined
}

function parseOptionalInteger(value: string | null) {
  if (!value) return undefined

  const parsed = Number.parseInt(value, 10)
  return Number.isFinite(parsed) ? parsed : undefined
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

function getRegionLabel(region: Region, countrySelected: boolean) {
  const parts = [region.region, region.city].filter(
    (part, index, values) => part && values.indexOf(part) === index,
  )

  if (!countrySelected) parts.unshift(region.country)

  return parts.join(', ')
}

function getErrorMessage(error: Error) {
  return error.message || 'Убедитесь, что backend запущен на порту 5145.'
}
