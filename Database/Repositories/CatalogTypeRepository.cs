using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public  class CatalogTypeRepository
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<CatalogType> _q_QueryData;
        public CatalogTypeRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.CatalogType;

        }

        #region query

        public List<CatalogType> GetCatalogType()
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
