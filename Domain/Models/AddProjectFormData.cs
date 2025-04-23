﻿using Domain.Models;

namespace Business.Models
{
    public class AddProjectFormData
    {
        public string? Image { get; set; }
        public string ProjectName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Budget { get; set; }
        public string ClientId { get; set; } = null!;
        public User User { get; set; } = null!;
        public int StatusId { get; set; }
    }
}
