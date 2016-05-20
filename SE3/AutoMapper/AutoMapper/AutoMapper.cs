using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapperPrj
{

    public interface Mapper<TSrc, TDest> 
    { 
        TDest Map(TSrc src);
        TColDest Map<TColDest>(IEnumerable<TSrc> src) where TColDest : ICollection<TDest>; 
    }

    public class AutoMapper<TSrc, TDest> : Mapper<TSrc, TDest>
    {
        TSrc SrcType;
        TDest DestType;

        public Mapper<TSrc, TDest> CreateMapper()
        {
            throw new NotImplementedException();
        }

        public static Builder<TOne, TTwo> Build<TOne, TTwo>()//Cria um objecto Builder que tem varios metodos de configuraçao e retorna esse objecto
        {
            Builder<TOne, TTwo> builder = new Builder<TOne, TTwo>();
            return builder;
        }

        public TDest Map(TSrc src)
        {
            Type srcType = typeof(TSrc);
            Type destType = typeof(TDest);
            PropertyInfo[] srcProps = srcType.GetProperties().OrderBy(prop => prop.Name).ToArray();
            PropertyInfo[] destProps = destType.GetProperties().OrderBy(prop => prop.Name).ToArray();
            return CopyValues(src, srcProps, destProps);
            
        }

        public TColDest Map<TColDest>(IEnumerable<TSrc> src) where TColDest : ICollection<TDest>
        {
            throw new NotImplementedException();
        }

        private TDest CopyValues(TSrc src, PropertyInfo[] srcProps, PropertyInfo[] destProps)
        {
            TDest dest;
            dest = (TDest)Activator.CreateInstance(typeof(TDest));
            for (int i = 0, j = 0; i < srcProps.Length && j < destProps.Length; i++)
            {
                if (srcProps[i].Name.Equals(destProps[j].Name))//TODO check if they are compatible
                {
                    object aux = srcProps[i].GetValue(src);
                    destProps[j].SetValue(dest, aux);
                    j++;
                }
            }
            return dest;
        }
    }
}
