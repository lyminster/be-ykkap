using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class ProjectReferencesRepository
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<ProjectReferences> _q_QueryData;
        public ProjectReferencesRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.ProjectReferences;

        }

        public List<ProjectReferences> GetListProjectReferences()
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
    }
}
