using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Repositories
{
    public class HelperTableRepository
    {
        private readonly BusinessModelContext MasterEntities;

        public HelperTableRepository(BusinessModelContext _MasterEntities)
        {
            MasterEntities = _MasterEntities;
        }

        public List<HelperTable> GetAll(HelperTable filter)
        {
            try
            {
                IEnumerable<HelperTable> Result = MasterEntities.HelperTables;
                if(filter != null)
                {
                    if (filter.ID != null)
                    {
                        Result = Result.Where(x => x.ID.ToLower() == filter.ID.ToLower()).ToList();
                    }
                    if (filter.Code != null)
                    {
                        Result = Result.Where(x => x.Code != null && x.Code.ToLower() == filter.Code.ToLower()).ToList();
                    }
                    if (filter.Name != null)
                    {
                        Result = Result.Where(x => x.Name != null && x.Name.ToLower() == filter.Name.ToLower()).ToList();
                    }
                }
               

                return Result.ToList();
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }
        public HelperTable GetHelperTable(string id)
        {
            try
            {
                return MasterEntities.HelperTables.FirstOrDefault(x => x.ID == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddNewRequest(HelperTable newRequest)
        {
            try
            {
                MasterEntities.HelperTables.Add(newRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Commit()
        {
            try
            {
                MasterEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
