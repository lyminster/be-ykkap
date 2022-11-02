using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class CompanyProfileRepository
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<CompanyProfile> _q_QueryData;
        public CompanyProfileRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.CompanyProfile;

        }

        #region query

        public CompanyProfile GetCompanyProfile()
        {

            try
            {
                    return _q_QueryData.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
    }
}
