
import './App.css'

import { useState } from 'react'

import {
    startIncidentRequest,
    resolveIncidentRequest,
    getIncidentsRequest,
    createIncidentRequest,
    getIncidentSummaryRequest
} from './services/incidentsApi'

import type { Incident,IncidentSummaryResponse } from './types/incident'

import PriorityBadge from './components/PriorityBadge'
function App() {


    const [incidents, setIncidents] = useState<Incident[]>([])


    const [selectedId, setSelectedId] = useState<number | null>(null)

    const selectedIncident = incidents.find(
        incident => incident.id === selectedId
    )

    const [isLoading, setIsLoading] = useState(false)


    const [error, setError] = useState <string | null>(null)

    const [statusFilter, setStatusFilter] = useState('')

    const [newTitle, setNewTitle] = useState('')
    const [newDescription, setNewDescription] = useState('')

    const [newImpact, setNewImpact] = useState('Low')
    const [newUrgency, setNewUrgency] = useState('Low')
    const [isCreating, setIsCreating] = useState(false)
    const [successMessage, setSuccessMessage] = useState<string | null>(null)

    const [totalCount, setTotalCount] = useState<number | null>(null)
    const [page, setPage] = useState(1)
    const pageSize = 3


    const [summary, setSummary] =
        useState<IncidentSummaryResponse | null>(null)



    async function loadSummary() {


        setError(null)
        try {
            const data = await getIncidentSummaryRequest()
            setSummary(data);
        }
        catch(error) {
            setError(
                error instanceof Error
                    ? error.message
                    : 'No se pudo cargar el resumen'
            )
        }
    }
    async function loadIncidents(requestedPage = 1) {
        setIsLoading(true)
        setError(null)

        try {
            const data = await getIncidentsRequest(
                statusFilter,
                requestedPage,
                pageSize
            )

            if (data.items.length === 0 && requestedPage > 1) {
                await loadIncidents(requestedPage - 1)
                return
            }

            setIncidents(data.items)
            setTotalCount(data.totalCount)
            setPage(data.page)
            setSelectedId(null)
        } catch (error) {
            setError(
                error instanceof Error
                    ? error.message
                    : 'No se pudieron cargar las incidencias'
            )
        } finally {
            setIsLoading(false)
        }
    }


    async function startIncident(id: number) {
        setError(null)

        try {
            await startIncidentRequest(id)
            await loadIncidents(page)
        } catch (error) {
            setError(
                error instanceof Error
                    ? error.message
                    : 'No se pudo iniciar la incidencia'
            )
        }
    }

    async function resolveIncident(id: number) {
        setError(null)

        try {
            await resolveIncidentRequest(id)
            await loadIncidents(page)
        } catch (error) {
            setError(
                error instanceof Error
                    ? error.message
                    : 'No se pudo resolver la incidencia'
            )
        }
    }

    async function createIncident() {
        setError(null)
        setSuccessMessage(null)

        if (newTitle.trim() === '') {
            setError('Escribe un título para la incidencia')
            return
        }

        setIsCreating(true)

        try {
            await createIncidentRequest({
                title: newTitle.trim(),
                description: newDescription,
                impact: newImpact,
                urgency: newUrgency
            })

            setSuccessMessage('Incidencia creada correctamente')
            setNewTitle('')
            setNewDescription('')

            await loadIncidents()
        } catch (error) {
            setError(
                error instanceof Error
                    ? error.message
                    : 'No se pudo crear la incidencia'
            )
        } finally {
            setIsCreating(false)
        }
    }

    return (
        <main>
            <h1>OpsControl</h1>
            <p>Gestión de incidencias</p>
            <section>
                <h2>Resumen de incidencias</h2>

                <button onClick={loadSummary}>
                    Actualizar resumen
                </button>

                {summary && (
                    <div className="summary-grid">
                        <p className="summary-card">
                            Nuevas: 
                            <strong>{summary.newCount}</strong>
                        </p>
                        <p className="summary-card">
                            En curso:
                            <strong>{summary.inProgressCount}</strong>
                        </p>

                        <p className="summary-card">
                            Resueltas:
                            <strong>{summary.resolvedCount}</strong>
                        </p>
                        <p className="summary-card">
                            Críticas pendientes:
                            <strong>{summary.criticalPendingCount}</strong>
                        </p>

                       
                    </div>
                )}
            </section>

            <section className="create-incident">
                <h2>Nueva incidencia</h2>

                <label>
                    Título:
                    <input
                        type="text"
                        value={newTitle}
                        onChange={event => setNewTitle(event.target.value)}
                    />
                </label>
                <label>
                    Descripción:
                    <textarea
                        value={newDescription}
                        onChange={event => setNewDescription(event.target.value)}
                    />
                </label>

                <label>
                    Impacto:
                    <select
                        value={newImpact}
                        onChange={event => setNewImpact(event.target.value)}
                    >
                        <option value="Low">Bajo</option>
                        <option value="Medium">Medio</option>
                        <option value="High">Alto</option>
                    </select>
                </label>

                <label>
                    Urgencia:
                    <select
                        value={newUrgency}
                        onChange={event => setNewUrgency(event.target.value)}
                    >
                        <option value="Low">Baja</option>
                        <option value="Medium">Media</option>
                        <option value="High">Alta</option>
                    </select>
                </label>
                

                <button
                    type="button"
                    onClick={createIncident}
                    disabled={isCreating}
                >
                    {isCreating ? 'Creando...' : 'Crear incidencia'}
                </button>
                {successMessage && (
                    <p className="success-message" role="status">
                        {successMessage}
                    </p>
                )}
            </section>

            {error && <p role="alert">{error}</p>}

            <h2>Incidencias</h2>

            <label>
                Estado:
                <select
                    value={statusFilter}
                    onChange={event => setStatusFilter(event.target.value)}
                >
                    <option value="">Todos</option>
                    <option value="New">Nuevas</option>
                    <option value="InProgress">En curso</option>
                    <option value="Resolved">Resueltas</option>
                </select>
            </label>
{/* 
            <button onClick={loadIncidents} disabled={isLoading}>
                {isLoading ? 'Cargando...' : 'Cargar incidencias'}
            </button> */}
            <button onClick={() => loadIncidents(1)}>
                {isLoading ? 'Cargando...' : 'Cargar incidencias'}
            </button>
            
            
            <p>ID Seleccionado: {selectedId ?? 'Ninguno'}</p>
            <button onClick={() => setSelectedId(null)} >Limpiar selección </button>
            {totalCount !== null && (
                <p>
                    Mostrando {incidents.length} de {totalCount} incidencias
                </p>
            )}
            <table>
                <thead>
                    <tr>
                        <th>Título</th>
                        <th>Estado</th>
                        <th>Prioridad</th>
                        <th>Acciones</th>
                        
                    </tr>
                </thead>
                <tbody>
                    {totalCount === 0 && !isLoading && !error && (
                        <tr>
                            <td colSpan={4}>
                                No hay incidencias que coincidan con el filtro.
                            </td>
                        </tr>
                    )}
                    {incidents.map(incident => (
                        <tr
                            key={incident.id}
                            className={selectedId === incident.id ? 'selected-row' : ''}
                        >
                            <td>{incident.title}</td>
                            <td>{incident.status}</td> 
                            <td>
                                <PriorityBadge priority={incident.priority} />
                            </td>
                            <td>
                                <button
                                    className="detail-button"
                                    onClick={() => setSelectedId(incident.id)}>
                                    Ver detalle
                                </button>
                              {/*   {selectedId === incident.id && <strong> Seleccionada</strong>} */}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <div>
                <button
                    onClick={() => loadIncidents(page - 1)}
                    disabled={isLoading || page <= 1}
                >
                    Anterior
                </button>

                <span>Página {page}</span>

                <button
                    onClick={() => loadIncidents(page + 1)}
                    disabled={
                        isLoading ||
                        totalCount === null ||
                        page * pageSize >= totalCount
                    }
                >
                    Siguiente
                </button>
            </div>
            {selectedIncident && (
                <section className='detailcss'>
                    <h2>Detalle de la incidencia</h2>
                    <p>{selectedIncident.title}</p>
                    <p>
                        Prioridad: <PriorityBadge priority={selectedIncident.priority} />
                    </p>
                    <p>Estado: {selectedIncident.status}</p>
                    {selectedIncident.status === 'Resolved' && (
                        <p>Esta incidencia ya está cerrada</p>

                    )}
                    {selectedIncident.status === 'New' && (
                        <button onClick={() => startIncident(selectedIncident.id)}>
                            Iniciar trabajo
                        </button>
                    )}
                    {selectedIncident.status === 'InProgress' && (
                        <button onClick={() => resolveIncident(selectedIncident.id)}>
                            Resolver
                        </button>
                    )}
                </section>

            )}
        </main>
    )
}

export default App
