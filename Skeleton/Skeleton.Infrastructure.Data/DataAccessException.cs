using System;
using System.Data.Common;
using System.Runtime.Serialization;

namespace Skeleton.Infrastructure.Data
{
    [Serializable]
    public class DataAccessException : DbException
    {
        public DataAccessException()
        {
        }

        public DataAccessException(string message)
            : base(message)
        {
        }

        public DataAccessException(Exception ex)
            : base(ex.Message, ex)
        {
        }

        public DataAccessException(string source, Exception ex)
            : base(source, ex)
        {
        }

        protected DataAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}