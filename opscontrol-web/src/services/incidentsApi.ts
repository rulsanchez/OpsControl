import type {
    IncidentPageResponse,
    CreateIncidentRequest,
    IncidentSummaryResponse
} from '../types/incident'

const API_URL = import.meta.env.VITE_API_URL


export async function startIncidentRequest(id: number) {
    const response = await fetch(`${API_URL}/${id}/start`, {
        method: 'PUT'
    })

    if (!response.ok) {
        throw new Error('No se pudo iniciar la incidencia')
    }
}

export async function getIncidentSummaryRequest(): Promise <IncidentSummaryResponse>  {

    const response = await fetch(`${API_URL}/summary`, {
        method: 'GET'
    })

    if (!response.ok) {
        throw new Error('No se pudo consultar el sumario')
    }
    return await response.json()

}
export async function resolveIncidentRequest(id: number) {
    const response = await fetch(`${API_URL}/${id}/resolve`, {
        method: 'PUT'
    })

    if (!response.ok) {
        throw new Error('No se pudo resolver la incidencia')
    }
}
export async function getIncidentsRequest(
    status: string,
    page: number,
    pageSize: number
): Promise<IncidentPageResponse> {
    const url =
        `${API_URL}?page=${page}&pageSize=${pageSize}` +
        (status === '' ? '' : `&status=${encodeURIComponent(status)}`)

    const response = await fetch(url)

    if (!response.ok) {
        throw new Error('No se pudieron cargar las incidencias')
    }

    return await response.json()
}
export async function createIncidentRequest(
    incident: CreateIncidentRequest
) {
    const response = await fetch(API_URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(incident)
    })

    if (!response.ok) {
        const detail = await response.text()
        throw new Error(`Error ${response.status}: ${detail}`)
    }
}