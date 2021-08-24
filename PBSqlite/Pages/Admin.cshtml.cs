using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using PBSqlite.Models;
using PBSqlite.Services;

namespace PBSqlite.Pages
{
    public class AdminModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;
        private readonly IRazorRenderService _renderService;
        private readonly ITableDataService _tableData;

        public AdminModel(ILogger<ErrorModel> logger, ITableDataService table, IRazorRenderService renderService)
        {
            _logger = logger;
            _tableData = table;
            _renderService = renderService;
        }

        [BindProperty] public List<Table> AllTables => _tableData.GetAllTables();

        [BindProperty] public List<User> AllUsers => _tableData.GetAllUsers();

        public async Task<JsonResult> OnGetCreateUser(int id = 0)
        {
            if (id == 0)
                return new JsonResult(new
                    { isValid = true, html = await _renderService.ToStringAsync("_CreateUser", new User()) });

            return new JsonResult(new
                { isValid = true, html = await _renderService.ToStringAsync("Error", new ErrorModel(_logger)) });
        }

        public async Task<JsonResult> OnPostCreateUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                newUser.Id = 0;
                _tableData.CreateUser(newUser);
                var html = await _renderService.ToStringAsync("_ViewAll", _tableData.GetAllPlayer());
                return new JsonResult(new { isValid = true, html });
            }
            else
            {
                var html = await _renderService.ToStringAsync("_ViewAll", _tableData.GetAllPlayer());
                return new JsonResult(new { isValid = true, html });
            }
        }

        public async Task<JsonResult> OnGetCreateTable(int id = 0)
        {
            if (id == 0)
                return new JsonResult(new
                    { isValid = true, html = await _renderService.ToStringAsync("_CreateTable", new Table()) });

            return new JsonResult(new
                { isValid = true, html = await _renderService.ToStringAsync("Error", new ErrorModel(_logger)) });
        }

        public async Task<JsonResult> OnPostCreateTable(Table newTable)
        {
            if (ModelState.IsValid)
            {
                _tableData.CreateTable(newTable.TableName);
                var html = await _renderService.ToStringAsync("_ViewAll", _tableData.GetAllPlayer());
                return new JsonResult(new { isValid = true, html });
            }
            else
            {
                var html = await _renderService.ToStringAsync("_ViewAll", _tableData.GetAllPlayer());
                return new JsonResult(new { isValid = true, html });
            }
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(int id = 0)
        {
            if (id == 0)
                return new JsonResult(new
                    { isValid = true, html = await _renderService.ToStringAsync("_CreateOrEdit", new Player()) });

            return new JsonResult(new
            {
                isValid = true, html = await _renderService.ToStringAsync("_CreateOrEdit", _tableData.GetPlayer(id))
            });
        }

        public async Task<JsonResult> OnPostCreateOrEditAsync(int id, Player player)
        {
            if (ModelState.IsValid)
            {
                _tableData.AddOrUpdatePlayer(player);
                var html = await _renderService.ToStringAsync("_ViewAll", _tableData.GetAllPlayer());
                return new JsonResult(new { isValid = true, html });
            }
            else
            {
                var html = await _renderService.ToStringAsync("_ViewAll", _tableData.GetAllPlayer());
                return new JsonResult(new { isValid = true, html });
            }
        }

        public async Task<JsonResult> OnPostDeleteAsync(int id)
        {
            _tableData.DeletePlayer(id);
            var html = await _renderService.ToStringAsync("_ViewAll", _tableData.GetAllPlayer());
            return new JsonResult(new { isValid = true, html });
        }

        public PartialViewResult OnGetViewAllPartial()
        {
            return new PartialViewResult
            {
                ViewName = "_ViewAll",
                ViewData = new ViewDataDictionary<IEnumerable<Player>>(ViewData, _tableData.GetAllPlayer())
            };
        }

        public PartialViewResult OnGetViewSelectTable(string text)
        {
            return new PartialViewResult
            {
                ViewName = "_ViewAll",
                ViewData = new ViewDataDictionary<IEnumerable<Player>>(ViewData, _tableData.GetByTable(text))
            };
        }

        public PartialViewResult OnGetViewSelectUser(string text)
        {
            return new PartialViewResult
            {
                ViewName = "_ViewAll",
                ViewData = new ViewDataDictionary<IEnumerable<Player>>(ViewData, _tableData.GetByTable(text))
            };
        }
    }
}