using System.Threading.Tasks;

namespace WebCameraSelfie.Storage
{
    public interface IStorage
    {
        Task<string> UploadAndGetLink(byte[] data);
    }
}
