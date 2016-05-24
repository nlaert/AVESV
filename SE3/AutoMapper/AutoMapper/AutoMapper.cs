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
        public Dictionary<PropertyInfo, PropertyInfo> PropertiesDictionary { get; set; }
        public Type Src;
        public Type Dest;
        public static AutoMapper<TSrc, TDest> Build<TSrc, TDest>()
        {
            return new AutoMapper<TSrc, TDest>();
        }
        public AutoMapper()
        {
        }

        public Mapper<TSrc, TDest> CreateMapper()
        {
            Type srcType = typeof(TSrc);
            Type destType = typeof(TDest);
            PropertiesDictionary = new Dictionary<PropertyInfo, PropertyInfo>();
            PropertyInfo[] srcProps = srcType.GetProperties().OrderBy(prop => prop.Name).ToArray();
            PropertyInfo[] destProps = destType.GetProperties().OrderBy(prop => prop.Name).ToArray();
            for (int i = 0, j = 0; i < srcProps.Length && j < destProps.Length; i++)
            {
                if (srcProps[i].Name.Equals(destProps[j].Name))
                {
                    PropertiesDictionary.Add(srcProps[i], destProps[i]);
                    j++;
                }
            }
            return this;
        }

        public TDest Map(TSrc src)
        {
            TDest dest = (TDest)Activator.CreateInstance(typeof(TDest));
            PropertyInfo[] props = PropertiesDictionary.Keys.ToArray();
            foreach (PropertyInfo p in props)
            {
                object aux = p.GetValue(src);
                PropertiesDictionary[p].SetValue(dest, aux);
            }
            return dest;
            
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
