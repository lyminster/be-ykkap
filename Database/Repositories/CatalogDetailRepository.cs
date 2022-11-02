using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class CatalogDetailRepository
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<CatalogDetail> _q_QueryData;
        public CatalogDetailRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.CatalogDetail;

        }

        #region query

        public List<CatalogDetail> GetCatalogDetailByType(string Id)
        {

            try
            {
                return _q_QueryData.Where(x => x.CatalogType == Id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<CatalogDetail> GetAll()
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
