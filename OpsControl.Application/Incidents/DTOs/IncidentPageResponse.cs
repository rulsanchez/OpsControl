namespace OpsControl.Application.Incidents.DTOs
{
    public class IncidentPageResponse
    {
        public List<IncidentListItemDto> Items { get; set; } = new List<IncidentListItemDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; }

        public int PageSize { get; set; }


    }
}
