using System.Security.Claims;
using Business.Interfaces;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Authorize]
    public class ProjectsController(
        IProjectService projectService,
        IClientService clientService,
        IUserService userService,
        IStatusService statusService
    ) : Controller
    {
        private readonly IProjectService _projectService = projectService;
        private readonly IClientService _clientService = clientService;
        private readonly IUserService _userService = userService;
        private readonly IStatusService _statusService = statusService;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetProjectsAsync();
            var clients = await _clientService.GetClientsAsync();
            var users = await _userService.GetUsersAsync();
            if (!projects.Succeeded || !clients.Succeeded || !users.Succeeded)
                return BadRequest();

            ViewBag.Clients = clients.Result!;
            ViewBag.Members = users.Result!;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = users.Result!.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                ViewBag.FullName = $"{user.FirstName} {user.LastName}";
            }
            else
            {
                ViewBag.FullName = "User";
            }

            var model = new ProjectsViewModel { Projects = projects.Result! };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clients = (await _clientService.GetClientsAsync()).Result!;
                var vm = new ProjectsViewModel
                {
                    Projects = (await _projectService.GetProjectsAsync()).Result!,
                };
                return View("Index", vm);
            }

            var formData = model.MapTo<AddProjectFormData>();
            var result = await _projectService.CreateProjectAsync(formData);
            if (!result.Succeeded)
                return BadRequest(result.Error);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var result = await _projectService.GetProjectAsync(id);
            if (!result.Succeeded || result.Result == null)
                return BadRequest(result.Error);

            var vm = new EditProjectViewModel
            {
                Id = result.Result.Id,
                ProjectName = result.Result.ProjectName,
                Description = result.Result.Description,
                StartDate = result.Result.StartDate,
                EndDate = result.Result.EndDate,
                Budget = result.Result.Budget,
                ClientId = result.Result.Client.Id,
                StatusId = result.Result.Status.Id,
                Clients = (await _clientService.GetClientsAsync()).Result ?? [],
                Statuses = (await _statusService.GetStatusAsync()).Result ?? [],
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Clients = (await _clientService.GetClientsAsync()).Result ?? [];
                model.Statuses = (await _statusService.GetStatusAsync()).Result ?? [];
                return BadRequest(ModelState);
            }

            var updateDto = new UpdateProjectFormData
            {
                Id = model.Id,
                ProjectName = model.ProjectName,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Budget = model.Budget,
                ClientId = model.ClientId,
                StatusId = model.StatusId,
            };

            var result = await _projectService.UpdateProjectAsync(updateDto);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Error ?? "An unknown error occurred."
                );
                model.Clients = (await _clientService.GetClientsAsync()).Result ?? [];
                model.Statuses = (await _statusService.GetStatusAsync()).Result ?? [];
                return BadRequest(ModelState);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _projectService.DeleteProjectAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Error);

            return RedirectToAction("Index");
        }
    }
}
