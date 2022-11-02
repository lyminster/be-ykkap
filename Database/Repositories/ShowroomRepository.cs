using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class ShowroomRepository
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<Showroom> _q_QueryData;
        public ShowroomRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.Showroom;

        }

        #region query

        public List<Showroom> GetListShowroom()
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
