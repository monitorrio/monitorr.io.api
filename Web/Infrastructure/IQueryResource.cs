using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Web.Infrastructure
{
    /// <summary>
    /// Simple interface the ability to fire an asynchronous query to a uri
    /// </summary>
    public interface IQueryResource
    {
        Task<Stream> Get(Uri uri);
    }

    public class QueryResourceViaHttp : IQueryResource
    {
        public async Task<Stream> Get(Uri uri)
        {

            using (var client = new HttpClient())
                return await client.GetStreamAsync(uri);
        }
    }
}