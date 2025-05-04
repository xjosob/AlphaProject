﻿using Business.Models;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
        Task<ProjectResult<Project>> GetProjectAsync(string id);
        Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
        Task<ProjectResult> UpdateProjectAsync(UpdateProjectFormData formData);
        Task<ProjectResult> DeleteProjectAsync(string id);
    }
}
