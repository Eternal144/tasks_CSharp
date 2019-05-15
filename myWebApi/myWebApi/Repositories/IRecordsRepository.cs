using myWebApi.Models;

namespace myWebApi.Repositories
{
    //在这里实现了增删改查的方法
    public interface IRecordsRepository
    {
        int AddNewRecord(Record record);
        int DeleteActorById(int id);
        Record GetActorById(int id);
        Record[] GetActors();
        int UpdateActorById(int id, Record record);
        int UpdateActorByIdEntityState(int id, Record record);
    }
}