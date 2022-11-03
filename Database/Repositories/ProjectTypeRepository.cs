using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class ProjectTypeRepository
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<ProjectType> _q_QueryData;
        public ProjectTypeRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.ProjectType;

        }

        #region query

        public List<ProjectType> GetListProjectType()
        {

            try
            {
                return _q_QueryData.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
    }
}
