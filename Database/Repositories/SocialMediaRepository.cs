using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class SocialMediaRepository
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<SocialMedia> _q_QueryData;
        public SocialMediaRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.SocialMedia;

        }

        #region query

        public SocialMedia GetSocialMedia()
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

        public List<SocialMedia> QuerySocialmedias(Expression<Func<SocialMedia, bool>> expression, int take = 0, int skip = 0)
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

        public SocialMedia GetSocialmediaByID(string ID)
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


        public bool IsUniqueKeyCodeExist(string urlfb, string urlig, string urlyt, string urlweb, String idclient)
        {
            return _q_QueryData.Where(x => x.RowStatus == 0).FirstOrDefault(x =>
            x.urlFb == urlfb
            && x.urlIg == urlig
            && x.urlYt == urlyt
            && x.urlWeb == urlweb
            && x.Idclient == idclient
            ) != null ? true : false;
        }

        #endregion
    }
}
