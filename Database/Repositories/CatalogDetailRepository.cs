using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            _q_QueryData = _dbContext.CatalogDetail.Include(x => x.catalogTypeNavigation);

        }

        #region query

        public List<CatalogDetail> GetCatalogDetailByType(string Id)
        {

            try
            {
                return _q_QueryData.Where(x => x.CatalogType == Id).OrderBy(x=>x.OrderNo).ToList();
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

        public List<CatalogDetail> QueryCatalogDetails(Expression<Func<CatalogDetail, bool>> expression, int take = 0, int skip = 0)
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

        public CatalogDetail GetCatalogDetailByID(string ID)
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


        public bool IsUniqueKeyCodeExist(string name, string CatalogType, string description, string imgUrl, string enPdfUrl, string idPdfUrl, String idclient)
        {
            return _q_QueryData.Where(x => x.RowStatus == 0).FirstOrDefault(x =>
            x.name == name
            && x.CatalogType == CatalogType
            && x.description == description
            && x.imgUrl == imgUrl
            && x.enPdfUrl == enPdfUrl
            && x.idPdfUrl == idPdfUrl
            && x.Idclient == idclient
            ) != null ? true : false;
        }

        #endregion
    }
}
