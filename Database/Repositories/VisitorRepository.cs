using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class VisitorRepository
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<Visitor> _q_QueryData;
        public VisitorRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.Visitor;

        }

        #region query

        public List<Visitor> GetListVisitor()
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

        public List<Visitor> QueryVisitors(Expression<Func<Visitor, bool>> expression, int take = 0, int skip = 0)
        {

            try
            {
                if (take < 1)
                {
                    return _q_QueryData.Where(x => x.RowStatus == 0).Where(expression).ToList();
                }
                else
                {
                    return _q_QueryData.Where(x => x.RowStatus == 0).Where(expression).Skip(skip).Take(take).ToList(); ;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Visitor GetVisitorByID(string ID)
        {
            try
            {
                return _q_QueryData.FirstOrDefault(x => x.ID == ID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool IsUniqueKeyCodeExist(string name, string PhoneNumber, string Email, string AccessFrom)
        {
            return _q_QueryData.Where(x => x.RowStatus == 0).FirstOrDefault(x =>
            x.Name == name
            && x.PhoneNumber == PhoneNumber
            && x.Email == Email
            && x.AccessFrom == AccessFrom 
            ) != null ? true : false;
        }

        #endregion
    }
}
