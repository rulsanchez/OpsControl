
import './App.css'

import { useEffect, useState } from 'react'

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
    const pageSize = 6


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
            await loadSummary()
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
            await loadSummary()
        } catch (error) {
            setError(
                error instanceof Error
                    ? error.message
                    : 'No se pudo resolver la incidencia'
            )
        }
    }

    useEffect(() => {
        void loadSummary()
        void loadIncidents(1)
        // Initial dashboard load only.
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

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
            await loadSummary()
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
        <main className="app-shell">

            {/* CABECERA */}
            <header className="app-header">
                <div className="brand">
                    <div className="brand-icon">⚙</div>
                    <div>
                        <h1>OpsControl</h1>
                        <p>Incident Management System</p>
                    </div>
                </div>

                <div className="system-status">
                    <span className="status-dot"></span>
                    System Online
                </div>
            </header>

            {/* DASHBOARD */}
            <div className="dashboard-title">
                <div>
                    <h2>Dashboard</h2>
                    <p>Resumen del estado actual de las incidencias</p>
                </div>

                <button
                    className="secondary-button"
                    onClick={loadSummary}
                >
                    ↻ Actualizar
                </button>
            </div>

            {/* TARJETAS DE RESUMEN */}
            {summary && (
                <div className="summary-grid">

                    <div className="summary-card total-card">
                        <div className="summary-icon">▣</div>
                        <div>
                            <span>Total</span>
                            <strong>
                                {summary.newCount +
                                    summary.inProgressCount +
                                    summary.resolvedCount}
                            </strong>
                        </div>
                    </div>

                    <div className="summary-card new-card">
                        <div className="summary-icon">!</div>
                        <div>
                            <span>Nuevas</span>
                            <strong>{summary.newCount}</strong>
                        </div>
                    </div>

                    <div className="summary-card progress-card">
                        <div className="summary-icon">◷</div>
                        <div>
                            <span>En curso</span>
                            <strong>{summary.inProgressCount}</strong>
                        </div>
                    </div>

                    <div className="summary-card resolved-card">
                        <div className="summary-icon">✓</div>
                        <div>
                            <span>Resueltas</span>
                            <strong>{summary.resolvedCount}</strong>
                        </div>
                    </div>

                </div>
            )}

            {error && <p role="alert">{error}</p>}

            {/* ZONA PRINCIPAL */}
            <div className="workspace">

                {/* NUEVA INCIDENCIA */}
                <section className="create-incident panel">
                    <div className="panel-heading">
                        <div>
                            <h2>Nueva incidencia</h2>
                            <p>Registra una nueva incidencia</p>
                        </div>
                    </div>

                    <label>
                        Título
                        <input
                            type="text"
                            placeholder="Introduce un título..."
                            value={newTitle}
                            onChange={event => setNewTitle(event.target.value)}
                        />
                    </label>

                    <label>
                        Descripción
                        <textarea
                            placeholder="Describe la incidencia..."
                            value={newDescription}
                            onChange={event =>
                                setNewDescription(event.target.value)
                            }
                        />
                    </label>

                    <div className="classification-fields">
                        <label>
                            Impacto
                            <select
                                value={newImpact}
                                onChange={event =>
                                    setNewImpact(event.target.value)
                                }
                            >
                                <option value="Low">Bajo</option>
                                <option value="Medium">Medio</option>
                                <option value="High">Alto</option>
                            </select>
                        </label>

                        <label>
                            Urgencia
                            <select
                                value={newUrgency}
                                onChange={event =>
                                    setNewUrgency(event.target.value)
                                }
                            >
                                <option value="Low">Baja</option>
                                <option value="Medium">Media</option>
                                <option value="High">Alta</option>
                            </select>
                        </label>
                    </div>

                    <button
                        className="primary-button create-button"
                        type="button"
                        onClick={createIncident}
                        disabled={isCreating}
                    >
                        {isCreating
                            ? 'Creando...'
                            : '+ Crear incidencia'}
                    </button>

                    {successMessage && (
                        <p
                            className="success-message"
                            role="status"
                        >
                            {successMessage}
                        </p>
                    )}
                </section>

                {/* LISTA */}
                <section className="incidents-panel panel">

                    <div className="panel-heading incidents-heading">
                        <div>
                            <h2>Incidencias</h2>

                            {totalCount !== null && (
                                <p>
                                    {totalCount} incidencia
                                    {totalCount !== 1 ? 's' : ''}
                                </p>
                            )}
                        </div>

                        <div className="filter-area">
                            <select
                                aria-label="Filtrar por estado"
                                value={statusFilter}
                                onChange={event =>
                                    setStatusFilter(event.target.value)
                                }
                            >
                                <option value="">Todos los estados</option>
                                <option value="New">Nuevas</option>
                                <option value="InProgress">En curso</option>
                                <option value="Resolved">Resueltas</option>
                            </select>

                            <button
                                className="secondary-button"
                                onClick={() => loadIncidents(1)}
                                disabled={isLoading}
                            >
                                {isLoading ? 'Cargando...' : 'Actualizar'}
                            </button>
                        </div>
                    </div>

                    <div className="table-container">
                        <table>
                            <thead>
                                <tr>
                                    <th>Título</th>
                                    <th>Estado</th>
                                    <th>Prioridad</th>
                                    <th>Acción</th>
                                </tr>
                            </thead>

                            <tbody>
                                {totalCount === 0 &&
                                    !isLoading &&
                                    !error && (
                                        <tr>
                                            <td
                                                colSpan={4}
                                                className="empty-table"
                                            >
                                                No hay incidencias que coincidan
                                                con el filtro.
                                            </td>
                                        </tr>
                                    )}

                                {incidents.map(incident => (
                                    <tr
                                        key={incident.id}
                                        className={
                                            selectedId === incident.id
                                                ? 'selected-row'
                                                : ''
                                        }
                                    >
                                        <td className="incident-title">
                                            {incident.title}
                                        </td>

                                        <td>
                                            <span
                                                className={`status-badge status-${incident.status.toLowerCase()}`}
                                            >
                                                {incident.status === 'New'
                                                    ? 'Nueva'
                                                    : incident.status ===
                                                        'InProgress'
                                                        ? 'En curso'
                                                        : 'Resuelta'}
                                            </span>
                                        </td>

                                        <td>
                                            <PriorityBadge
                                                priority={incident.priority}
                                            />
                                        </td>

                                        <td>
                                            <button
                                                className="detail-button"
                                                onClick={() =>
                                                    setSelectedId(incident.id)
                                                }
                                            >
                                                Ver detalle
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>

                    {/* PAGINACIÓN */}
                    <div className="pagination">
                        <button
                            onClick={() =>
                                loadIncidents(page - 1)
                            }
                            disabled={isLoading || page <= 1}
                        >
                            ‹
                        </button>

                        <span>Página {page}</span>

                        <button
                            onClick={() =>
                                loadIncidents(page + 1)
                            }
                            disabled={
                                isLoading ||
                                totalCount === null ||
                                page * pageSize >= totalCount
                            }
                        >
                            ›
                        </button>
                    </div>

                </section>
            </div>

            {/* DETALLE */}
            {selectedIncident && (
                <section className="detail-panel">

                    <div className="detail-header">
                        <div>
                            <span className="detail-label">
                                INCIDENCIA #{selectedIncident.id}
                            </span>
                            <h2>{selectedIncident.title}</h2>
                        </div>

                        <button
                            className="close-detail"
                            onClick={() => setSelectedId(null)}
                        >
                            ×
                        </button>
                    </div>

                    <div className="detail-info">
                        <div>
                            <span>Estado</span>
                            <strong>{selectedIncident.status}</strong>
                        </div>

                        <div>
                            <span>Prioridad</span>
                            <PriorityBadge
                                priority={selectedIncident.priority}
                            />
                        </div>
                    </div>

                    <div className="detail-actions">

                        {selectedIncident.status === 'Resolved' && (
                            <span className="resolved-message">
                                ✓ Incidencia resuelta
                            </span>
                        )}

                        {selectedIncident.status === 'New' && (
                            <button
                                className="primary-button"
                                onClick={() =>
                                    startIncident(selectedIncident.id)
                                }
                            >
                                Iniciar trabajo
                            </button>
                        )}

                        {selectedIncident.status === 'InProgress' && (
                            <button
                                className="resolve-button"
                                onClick={() =>
                                    resolveIncident(selectedIncident.id)
                                }
                            >
                                ✓ Resolver incidencia
                            </button>
                        )}

                    </div>
                </section>
            )}

        </main>
    )
}

export default App
