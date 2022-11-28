using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                return _q_QueryData.OrderBy(x => x.OrderNo).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<CatalogType> QueryCatalogTypes(Expression<Func<CatalogType, bool>> expression, int take = 0, int skip = 0)
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

        public CatalogType GetCatalogTypeByID(string ID)
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


        public bool IsUniqueKeyCodeExist(string name,string description, string imgUrl, String idclient)
        {
            return _q_QueryData.Where(x => x.RowStatus == 0).FirstOrDefault(x =>
            x.name == name
            && x.description == description
            && x.imgUrl == imgUrl
            ) != null ? true : false;
        }

        #endregion
    }
}
