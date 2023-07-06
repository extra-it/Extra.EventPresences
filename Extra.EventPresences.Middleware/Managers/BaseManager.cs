using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Managers
{
    public class BaseManager
    {

        protected iConfigurationManager configurationManager;
        protected string ConnectionString { get; }
        protected DBDataContext DataContext { get; private set; }

        protected BaseManager(DBDataContext dataContext)
        {
            DataContext = dataContext;
        }

        protected void HandleException(Exception ex)
        {
            throw ex;
        }
        protected void HandleException(string message, Exception ex)
        {
            throw ex;
        }
    }
}
