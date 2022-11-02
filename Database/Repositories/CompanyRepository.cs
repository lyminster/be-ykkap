using Database.Models;
using Database.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ViewModel.Constant;

namespace Database.Repositories
{
    public class CompanyRepository  
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<Company> _q_QueryData;
        public CompanyRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.Companies;

        }

        #region query

        public List<Company> QueryCompanys(Expression<Func<Company, bool>> expression, int take = 0, int skip = 0)
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




        public Company GetCompanyByID(string ID)
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








        public bool IsUniqueKeyCodeExist(string Code, string Name, String idclient)
        {
            return _q_QueryData.Where(x => x.RowStatus == 0).FirstOrDefault(x => x.Code == Code
            && x.Name == Name && x.Idclient == idclient) != null ? true : false;
        }

        #endregion
    }
}