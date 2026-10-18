export function formatPeriod(fromYear: number, toYear: number) {
  if (fromYear === toYear) {
    return formatYear(fromYear)
  }

  return `${formatYear(fromYear)} — ${formatYear(toYear)}`
}

function formatYear(year: number) {
  return year < 0 ? `${Math.abs(year)} до н. э.` : `${year} н. э.`
}
