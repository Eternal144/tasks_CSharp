using myWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//仓库
namespace myWebApi.Repositories
{
    public class RecordsRepository : IRecordsRepository
    {
        private IsakilaContext _context;

        public RecordsRepository(IsakilaContext context)
        {
            _context = context;
        }

        public Record[] GetActors()
        {
            return _context.Records.ToArray();
        }

        public Record GetActorById(int id)
        {
            var actor = _context.Records.SingleOrDefault(a => a.rid == id);
            return actor;
        }

        // Better to use EntityState.Modified to update for unit testing
        public int UpdateActorById(int id, Record record)
        {
            int updateSuccess = 0;
            var target = _context.Records.SingleOrDefault(a => a.rid == id);
            if(target != null)
            {
                _context.Entry(target).CurrentValues.SetValues(record);
                updateSuccess =_context.SaveChanges();
            }
            return updateSuccess;
        }

        // This is a better approach for unit testing
        public int UpdateActorByIdEntityState(int id, Record record)
        {
            int updateSuccess = 0;
            if (id != record.rid)
            {
                return updateSuccess;
            }
            _context.MarkAsModified(record);
            updateSuccess = _context.SaveChanges();
            return updateSuccess;
        }

        public int AddNewRecord(Record actor)
        {
            int insertSuccess = 0;
            int maxId = _context.Records.Max(p => p.rid);

            actor.rid = (short) (maxId + 1);
            _context.Records.Add(actor);
            insertSuccess = _context.SaveChanges();

            return insertSuccess;

        }

        public int DeleteActorById(int id)
        {
            int deleteSuccess = 0;
            var actor = _context.Records.SingleOrDefault(a => a.rid == id);
            if (actor != null)
            {
                _context.Records.Remove(actor);
                deleteSuccess = _context.SaveChanges();
            }
            return deleteSuccess;
        }
    }
}
