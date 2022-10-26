using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Administration;
using Microsoft.Windows.EventTracing.ScheduledTasks;
using System.Threading.Tasks;
using Microsoft.Win32.TaskScheduler;
using System.Text.RegularExpressions;
using Task = Microsoft.Win32.TaskScheduler.Task;
using IIS_TEST.Models;
using System.ServiceProcess;
using System.Management;

namespace IIS_TEST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        [Route("/task")]
        public IEnumerable<TaskSchedule> GetTasks()
        {
            return TaskService.Instance.FindAllTasks(new Regex(".*")).Select(task => { return new TaskSchedule() {
                Autor = task.Definition.RegistrationInfo.Author,
                Name = task.Name,
                IsActive = task.IsActive,
                Enabled = task.Enabled,
                State = task.State.ToString(),
                ReadOnly = task.ReadOnly,
                Description = task.Definition.RegistrationInfo.Description,
                Path = task.Path,
                LastRunTime = task.LastRunTime,
                LastTimeResult = task.LastTaskResult,
                NextRunTime = task.NextRunTime,
                DateCreation = task.Definition.RegistrationInfo.Date 
            }; } );
        }

        [HttpGet]
        [Route("/services")]
        public IEnumerable<WindowService> GetWindowServices()
        {
            ServiceController[] service = ServiceController.GetServices();
            return ServiceController.GetServices().Select(service => {
                try
                {
                    return new WindowService()
                    {
                        DisplayName = service.DisplayName,
                        ServiceName = service.ServiceName,
                        StartType = service.StartType.ToString(),
                        Status = service.Status.ToString(),
                        Description = (string)new ManagementObject(new ManagementPath(string.Format("Win32_Service.Name='{0}'", service.ServiceName)))["Description"]
        };
                } catch
                {
                    return new WindowService() { };
                }
                
            });
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //Task[] allTasks = new Task;
            
            //foreach(Task task in allTasks)
            //{
            //    var name = task.Name;
            //    TaskDefinition definition = task.Definition;
                
            //    var it = task;
            //}

            ServerManager serverManager = new ServerManager($"{Environment.GetEnvironmentVariable("windir")}\\system32\\inetsrv\\config\\applicationHost.config");

            var p = serverManager.GetAdministrationConfiguration();
            var _ = serverManager.ApplicationPools;

            var _1 = serverManager.Sites;
            
            foreach(var item in _)
            {
                Console.Write(item);
                var i = item.State;
            }

            foreach (var item in _1)
            {
                   
                var atr = item.Attributes;
                try
                {
                    var x = item.State;
                }
                catch (Exception ex)
                {
                    var y = ex;
                }
                var q = item.ChildElements;
                var w = item.Bindings;
                var e = item.Applications;
                var r = item.Name;
                var s = item.Id;
                var t = item.Limits;
            }
            var l = serverManager.ApplicationPoolDefaults;

            var m = serverManager.GetApplicationHostConfiguration();

            var k = serverManager.GetRedirectionConfiguration();
            var h = serverManager.GetMetadata;
            var g = serverManager.SiteDefaults;
            var d = serverManager.WorkerProcesses;
            

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
            })
            .ToArray();
        }

    }
}