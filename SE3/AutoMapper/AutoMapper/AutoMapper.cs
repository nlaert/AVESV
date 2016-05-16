using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{

    interface Mapper<TSrc, TDest> 
    { 
        TDest Map(TSrc src);
        TColDest Map<TColDest>(IEnumerable<TSrc> src) where TColDest : ICollection<TDest>; 
    }

    public class AutoMapper<TSrc, TDest> : Mapper<TSrc, TDest>
    {
        public static void Build<TSrc, TDest>()
        {
            throw new NotImplementedException();
        }

        public TDest Map(TSrc src)
        {
            TDest dest;
            Type srcType = typeof(TSrc);
            Type destType = typeof(TDest);
            PropertyInfo[] srcProps = srcType.GetProperties().OrderBy(prop => prop.Name).ToArray();
            PropertyInfo[] destProps = destType.GetProperties().OrderBy(prop => prop.Name).ToArray();
            //iterate both arrays and setValue on dest using GetValue of src
            dest = Activator.CreateInstance<TDest>();
            
        }

        public TColDest Map<TColDest>(IEnumerable<TSrc> src) where TColDest : ICollection<TDest>
        {
            throw new NotImplementedException();
        }
    }
}
