export type Incident = {
    id: number
    title: string
    status: string
    priority: string
}
export type IncidentPageResponse = {
    items: Incident[]
    totalCount: number
    page: number
    pageSize: number
}
export type CreateIncidentRequest = {
    title: string
    description: string
    impact: string
    urgency: string
}
export type IncidentSummaryResponse = {
    newCount: number
    inProgressCount: number
    resolvedCount: number
    criticalPendingCount: number
}