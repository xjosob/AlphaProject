using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ProjectsController(IProjectService projectService) : Controller
    {
        private readonly IProjectService _projectService = projectService;

        public async Task<IActionResult> Index()
        {
            var model = new ProjectsViewModel
            {
                ProjectsController = await _projectService.GetProjectsAsync(),
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectViewModel model)
        {
            var addProjectFormData = model.MapTo<AddProjectFormData>();

            var reult = await _projectService.CreateProjectAsync(addProjectFormData);

            return Json(new { });
        }

        [HttpPost]
        public IActionResult Update(EditProjectViewModel model)
        {
            return Json(new { });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(new { });
        }
    }
}
