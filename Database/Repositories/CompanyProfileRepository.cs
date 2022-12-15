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

        public List<CompanyProfile> QueryCompanyProfiles(Expression<Func<CompanyProfile, bool>> expression, int take = 0, int skip = 0)
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

        public CompanyProfile GetCompanyProfileByID(string ID)
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

        public CompanyProfile GetCompanyProfileFirst()
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


        public bool IsUniqueKeyCodeExist(string about, string visionMission, string imgUrl, string youtubeId, String idclient)
        {
            return _q_QueryData.Where(x => x.RowStatus == 0).FirstOrDefault(x =>
            x.about == about
            && x.visionMission == visionMission
            && x.imgUrl == imgUrl
            && x.youtubeId == youtubeId
            && x.Idclient == idclient
            ) != null ? true : false;
        }

        #endregion
    }
}
