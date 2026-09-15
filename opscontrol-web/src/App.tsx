
import './App.css'
import { useState } from 'react'


type Incident = {
    id: number
    title: string
    status: string
    priority: string
}
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



    async function loadIncidents(requestedPage = 1)  {
        setIsLoading(true)
        setError(null)
        try {
            //await new Promise(resolve => setTimeout(resolve, 2000))
            const url =
                `https://localhost:7085/api/incidents?page=${requestedPage}&pageSize=${pageSize}` +
                (statusFilter === '' ? '' : `&status=${statusFilter}`)

            const response = await fetch(url)

            if (!response.ok) {
                setError('No se pudieron cargar las incidencias')
                return
            }

            const data = await response.json()
            if (data.items.length === 0 && requestedPage > 1) {
                await loadIncidents(requestedPage - 1)
                return
            }
            setIncidents(data.items)
            setTotalCount(data.totalCount)
            setPage(data.page)
            setSelectedId(null)
        } catch {
            setError('No se pudo conectar con la API')
        }
        finally {
            setIsLoading(false)
        }

    }


    async function startIncident(id: number) {
        setError(null)

        try {
            const response = await fetch(
                `https://localhost:7085/api/incidents/${id}/start`,
                { method: 'PUT' }
            )

            if (!response.ok) {
                setError('No se pudo iniciar la incidencia')
                return
            }

            await loadIncidents(page)
        } catch {
            setError('No se pudo conectar con la API')
        }
    }

    async function resolveIncident(id: number) {
        setError(null)

        try {
            const response = await fetch(
                `https://localhost:7085/api/incidents/${id}/resolve`,
                { method: 'PUT' }
            )

            if (!response.ok) {
                setError('No se pudo resolver la incidencia')
                return
            }

            await loadIncidents(page)
        } catch {
            setError('No se pudo conectar con la API')
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
            const response = await fetch(
                'https://localhost:7085/api/incidents',
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        title: newTitle.trim(),
                        description: newDescription,
                        impact: newImpact,
                        urgency: newUrgency
                    })
                }
            )

            // if (!response.ok) {
            //     setError('No se pudo crear la incidencia')
            //     return
            // }
            if (!response.ok) {
                const detail = await response.text()
                setError(`Error ${response.status}: ${detail}`)
                return
            }
            setSuccessMessage('Incidencia creada correctamente')
            setNewTitle('')
            setNewDescription('')
            await loadIncidents()
        } catch {
            setError('No se pudo conectar con la API')
        }
        finally {
            setIsCreating(false)
        }
    }


    return (
        <main>
            <h1>OpsControl</h1>
            <p>Gestión de incidencias</p>


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
                                <span className={incident.priority === 'Critical' ? 'priority-critical' : ''}>
                                    {incident.priority}
                                </span>
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
