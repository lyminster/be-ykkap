using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class VisitorsRepository
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<SocialMedia> _q_QueryData;
        public VisitorsRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.SocialMedia;

        }

    }
}
