﻿namespace Domain.Models
{
    public class UpdateProjectFormData
    {
        public string Id { get; set; } = null!;

        public string ProjectName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Budget { get; set; }
        public string ClientId { get; set; } = null!;
        public int StatusId { get; set; }
    }
}
