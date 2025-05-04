using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ProjectService(
        IProjectRepository projectRepository,
        IStatusService statusService,
        AppDbContext dbContext
    ) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IStatusService _statusService = statusService;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
        {
            if (formData == null)
                return new ProjectResult
                {
                    Succeeded = false,
                    StatusCode = 400,
                    Error = "Not all required fields are supplied.",
                };

            var statusCheck = await _statusService.GetStatusByIdAsync(formData.StatusId);
            if (!statusCheck.Succeeded)
                return new ProjectResult
                {
                    Succeeded = false,
                    StatusCode = statusCheck.StatusCode,
                    Error = statusCheck.Error,
                };

            var projectEntity = formData.MapTo<ProjectEntity>();

            var addResult = await _projectRepository.AddAsync(projectEntity);
            if (!addResult.Succeeded)
                return new ProjectResult
                {
                    Succeeded = false,
                    StatusCode = addResult.StatusCode,
                    Error = addResult.Error,
                };

            return new ProjectResult { Succeeded = true, StatusCode = 201 };
        }

        public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
        {
            var allStatuses = await _statusService.GetStatusAsync();
            if (!allStatuses.Succeeded || allStatuses.Result == null)
                return new ProjectResult<IEnumerable<Project>>
                {
                    Succeeded = false,
                    StatusCode = allStatuses.StatusCode,
                    Error = allStatuses.Error,
                };

            var statusMap = allStatuses.Result.ToDictionary(s => s.Id);

            var entities = await _dbContext
                .Projects.Include(p => p.Client)
                .OrderByDescending(p => p.Created)
                .ToListAsync();

            var projects = entities.Select(e => new Project
            {
                Id = e.Id,
                Image = e.Image,
                ProjectName = e.ProjectName,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Budget = e.Budget,
                Client = new Client { Id = e.Client.Id, ClientName = e.Client.ClientName },
                Status = statusMap[e.StatusId],
            });

            return new ProjectResult<IEnumerable<Project>>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = projects,
            };
        }

        public async Task<ProjectResult<Project>> GetProjectAsync(string id)
        {
            var allStatuses = await _statusService.GetStatusAsync();
            if (!allStatuses.Succeeded || allStatuses.Result == null)
                return new ProjectResult<Project>
                {
                    Succeeded = false,
                    StatusCode = allStatuses.StatusCode,
                    Error = allStatuses.Error,
                };
            var statusMap = allStatuses.Result.ToDictionary(s => s.Id);

            var e = await _dbContext
                .Projects.Include(p => p.Client)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (e == null)
                return new ProjectResult<Project>
                {
                    Succeeded = false,
                    StatusCode = 404,
                    Error = $"Project '{id}' was not found.",
                };

            var project = new Project
            {
                Id = e.Id,
                Image = e.Image,
                ProjectName = e.ProjectName,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Budget = e.Budget,
                Client = new Client { Id = e.Client.Id, ClientName = e.Client.ClientName },
                Status = statusMap[e.StatusId],
            };

            return new ProjectResult<Project>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = project,
            };
        }

        public async Task<ProjectResult> UpdateProjectAsync(UpdateProjectFormData formData)
        {
            if (formData == null)
                return new ProjectResult
                {
                    Succeeded = false,
                    StatusCode = 400,
                    Error = "Invalid form data.",
                };

            var statusCheck = await _statusService.GetStatusByIdAsync(formData.StatusId);
            if (!statusCheck.Succeeded)
                return new ProjectResult
                {
                    Succeeded = false,
                    StatusCode = statusCheck.StatusCode,
                    Error = statusCheck.Error,
                };

            var projectEntity = await _dbContext.Projects.FirstOrDefaultAsync(p =>
                p.Id == formData.Id
            );
            if (projectEntity == null)
                return new ProjectResult
                {
                    Succeeded = false,
                    StatusCode = 404,
                    Error = "Project not found.",
                };

            projectEntity.ProjectName = formData.ProjectName;
            projectEntity.Description = formData.Description;
            projectEntity.Budget = formData.Budget;
            projectEntity.ClientId = formData.ClientId;
            projectEntity.StatusId = formData.StatusId;
            projectEntity.StartDate = formData.StartDate;
            projectEntity.EndDate = formData.EndDate;

            var updateResult = await _projectRepository.UpdateAsync(projectEntity);
            if (!updateResult.Succeeded)
                return new ProjectResult
                {
                    Succeeded = false,
                    StatusCode = updateResult.StatusCode,
                    Error = updateResult.Error,
                };

            return new ProjectResult { Succeeded = true, StatusCode = 200 };
        }

        public async Task<ProjectResult> DeleteProjectAsync(string id)
        {
            var projectEntity = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (projectEntity == null)
                return new ProjectResult
                {
                    Succeeded = false,
                    StatusCode = 404,
                    Error = "Project not found.",
                };

            var deleteResult = await _projectRepository.DeleteAsync(projectEntity);
            if (!deleteResult.Succeeded)
                return new ProjectResult
                {
                    Succeeded = false,
                    StatusCode = deleteResult.StatusCode,
                    Error = deleteResult.Error,
                };

            return new ProjectResult { Succeeded = true, StatusCode = 200 };
        }
    }
}
