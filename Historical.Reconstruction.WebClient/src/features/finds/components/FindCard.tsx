import { Link } from 'react-router-dom'
import type { HistoricalFind } from '../types.ts'
import { formatPeriod } from '../lib/formatPeriod.ts'

interface FindCardProps {
  find: HistoricalFind
}

export function FindCard({ find }: FindCardProps) {
  const primaryImage = find.images[0]

  return (
    <article className="find-card">
      <Link className="find-card__media" to={`/finds/${find.id}`} tabIndex={-1}>
        {primaryImage ? (
          <img src={primaryImage.url} alt={primaryImage.caption || find.name} />
        ) : (
          <span aria-hidden="true">Архив</span>
        )}
      </Link>
      <div className="find-card__body">
        <span className="find-card__period">
          {formatPeriod(find.dateFromYear, find.dateToYear)}
        </span>
        <h2>
          <Link to={`/finds/${find.id}`}>{find.name}</Link>
        </h2>
        <p className="find-card__description">{find.description}</p>
        <Link className="find-card__detail-link" to={`/finds/${find.id}`}>
          Открыть карточку →
        </Link>
      </div>
    </article>
  )
}
