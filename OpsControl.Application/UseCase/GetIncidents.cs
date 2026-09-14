using OpsControl.Application.Incidents.DTOs;
using OpsControl.Application.Incidents.Repositories;
using OpsControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpsControl.Application.UseCase
{
    public class GetIncidents
    {
        private readonly IIncidentRepository _repository;

        public GetIncidents(IIncidentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IncidentPageResponse> ExecuteAsync(IncidentStatus? status, int page, int pageSize)
        {
            var result = await _repository.GetPagedAsync(status, page, pageSize);

            var itemsResponse =  result.Items.Select(p => new IncidentListItemDto()
            {
                Id = p.Id,
                CreatedAt = p.CreatedAt,
                Priority = p.Priority,
                Status = p.Status,
                Title = p.Title ?? string.Empty
            }).ToList();

            return new IncidentPageResponse()
            {
                Items = itemsResponse,
                Page = page,
                TotalCount = result.TotalCount,
                PageSize = pageSize

            };

        
        }
    }
}
