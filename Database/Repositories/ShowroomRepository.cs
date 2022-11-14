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

        public List<Showroom> QueryShowrooms(Expression<Func<Showroom, bool>> expression, int take = 0, int skip = 0)
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

        public Showroom GetShowroomByID(string ID)
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


        public bool IsUniqueKeyCodeExist(string name, string urlImg,string workingHour,string address,string telephone, String idclient)
        {
            return _q_QueryData.Where(x => x.RowStatus == 0).FirstOrDefault(x => 
            x.name == name
            && x.urlImage == urlImg
            && x.workingHour == workingHour
            && x.address == address
            && x.telephone == telephone
            && x.Idclient == idclient
            ) != null ? true : false;
        }

        #endregion
    }
}
