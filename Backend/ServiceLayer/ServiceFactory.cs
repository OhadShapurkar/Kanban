using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Authenticator authenticator = new();
        private readonly BoardFacade boardFacade; 
        private readonly UserFacade userFacade;
        public readonly UserService _us;
        public readonly BoardService _bs;
        public readonly TaskService _ts;
        public ServiceFactory()
        {

            var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepository, new System.IO.FileInfo("log4net.config"));
            userFacade = new UserFacade(authenticator);
            boardFacade = new BoardFacade(authenticator);
            
            _us = new UserService(userFacade);
            _bs = new BoardService(boardFacade);
            _ts = new TaskService(boardFacade);

        }
        
        public string LoadData()
        {
            try
            {
                userFacade.LoadData();
                boardFacade.LoadData();
                Response ret = new(null, null);
                log.Info($"User has load data");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response ret = new(null, ex.Message);
                log.Warn($"User has filed to load data: {ex.Message}");
                return ret.GetSerilizeResponse();
            }
        }

        public string DeleteData()
        {
            try
            {
                TaskController tc = new();
                tc.Claer();
                boardFacade.DeleteData();
                userFacade.DeleteData();
                Response ret = new(null, null);
                log.Info($"User has delete data");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response ret = new(null, ex.Message);
                log.Warn($"User has filed to delete data: {ex.Message}");
                return ret.GetSerilizeResponse();
            }
        }

            
    }
}
