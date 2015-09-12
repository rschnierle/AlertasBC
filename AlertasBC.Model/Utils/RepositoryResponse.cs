using System;

namespace AlertasBC.Model.Utils
{
    public class RepositoryResponse<T>
    {
        public bool Success { get; set; }

        public T Data { get; set; }

        public Exception Error { get; set; }
    }
}
