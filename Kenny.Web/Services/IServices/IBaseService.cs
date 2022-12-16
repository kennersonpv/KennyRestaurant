using Kenny.Web.Models;
using Kenny.Web.Models.Dto;

namespace Kenny.Web.Services.IServices
{
    public interface IBaseService : IDisposable
    {
        public ResponseDto responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);

    }
}
