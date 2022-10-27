using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Administration;
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
    public class WindowPlugs : ControllerBase
    {
        [HttpGet]
        [Route("/TaskSchedule")]
        [ActionName("GetWindowTaskSchedule")]
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
        [Route("/Services")]
        [ActionName("GetWindowServicesInformation")]
        public IEnumerable<WindowService> GetWindowServices()
        {
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
        [Route("/SitesIIS")]
        [ActionName("GetSitesIIS")]
        public IEnumerable<IISSite> GetSitesIIS()
        {

            //ServerManager serverManager = new ServerManager($"{Environment.GetEnvironmentVariable("windir")}\\system32\\inetsrv\\config\\applicationHost.config");
            ServerManager serverManager = new ServerManager();
            SiteCollection sites = serverManager.Sites;
            return sites.Select(site => { return new IISSite() {
                Id = site.Id,
                SiteName = site.Name,
                Schema = site.Schema.Name,
                State = site.State.ToString(),
                LogFileDirectory = site.LogFile.Directory,
                ServerAutoStart = site.ServerAutoStart,
                //Pool = site.Applications.Select(pool => { return new IISPool() {
                //    PoolName = pool.Name,
                //    AutoStart = pool.AutoStart,
                //    Schema = pool.Schema.Name,
                //    Enable32Bit = pool.Enable32BitAppOnWin64,
                //    IsLocallyStored = pool.IsLocallyStored,
                //    State = pool.State.ToString(),
                //    User = pool.ProcessModel.UserName,
                //    Password = pool.ProcessModel.Password,
                //    StartMode = pool.StartMode.ToString(),
                //    Version = pool.ManagedRuntimeVersion,
                //}; })
            };
            });
        }

        [HttpGet]
        [Route("/PoolsIIS")]
        [ActionName("GetPoolsIIS")]
        public IEnumerable<IISPool> GetPoolsIIS()
        {
            ServerManager serverManager = new ServerManager();
            ApplicationPoolCollection pools = serverManager.ApplicationPools;
            return pools.Select(pool => { return new IISPool() {
                PoolName = pool.Name,
                AutoStart = pool.AutoStart,
                Schema = pool.Schema.Name,
                Enable32Bit = pool.Enable32BitAppOnWin64,
                IsLocallyStored = pool.IsLocallyStored,
                State = pool.State.ToString(),
                User = pool.ProcessModel.UserName,
                Password = pool.ProcessModel.Password,
                StartMode = pool.StartMode.ToString(),
                Version = pool.ManagedRuntimeVersion,
                Applications = serverManager.Sites.Where(site => site.Applications.Select(app => app.ApplicationPoolName).Contains(pool.Name)).Select(site => {
                    return new IISSite
                    {
                        Id = site.Id,
                        SiteName = site.Name,
                        Schema = site.Schema.Name,
                        State = site.State.ToString(),
                        LogFileDirectory = site.LogFile.Directory,
                        ServerAutoStart = site.ServerAutoStart
                    };
                    })
                }; 
            });
        }

    }
}