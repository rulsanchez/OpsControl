using System;
using System.Collections.Generic;
using System.Text;

namespace OpsControl.Application.Incidents.DTOs
{
    public class IncidentSummaryResponse
    {
        /// <summary>
        /// Incidencias nuevas
        /// </summary>
        public int NewCount { get; set; }
        /// <summary>
        /// Incidencias en curso
        /// </summary>
        public int InProgressCount { get; set; }
        /// <summary>
        /// Incidencias resueltas
        /// </summary>
        public int ResolvedCount { get; set; }
        /// <summary>
        /// Incidencias criticas que aún no están resueltas
        /// </summary>
        public int CriticalPendingCount { get; set; }
    }
}