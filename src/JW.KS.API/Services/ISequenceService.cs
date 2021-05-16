using System.Threading.Tasks;

namespace JW.KS.API.Services
{
    public interface ISequenceService
    {
        Task<int> GetKnowledgeBaseNewId();
    }
}