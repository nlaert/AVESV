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
        TDest[] MapToArray(IEnumerable<TSrc> src);
        IEnumerable<TDest> MapLazy(IEnumerable<TSrc> src);
    }

    public class AutoMapper<TSrc, TDest> : Mapper<TSrc, TDest>
    {
        public Dictionary<PropertyInfo, PropertyInfo> PropertiesDictionary { get; set; }
        public List<string> ToBeIgnoredList;
        public static AutoMapper<TSrc, TDest> Build<TOne, Ttwo>()
        {
            return new AutoMapper<TSrc, TDest>();
        }

        //public static AutoMapper<TSrc, TDest> IgnoreMember(this AutoMapper<TSrc, TDest> am, string name)
        //{
        //    am.ToBeIgnoredList.Add(name);
        //    return am;
        //}

        public AutoMapper()
        {
            PropertiesDictionary = new Dictionary<PropertyInfo, PropertyInfo>();
            ToBeIgnoredList = new List<string>();
        }

        public Mapper<TSrc, TDest> CreateMapper()
        {
            Type srcType = typeof(TSrc);
            Type destType = typeof(TDest);
            
            PropertyInfo[] srcProps = srcType.GetProperties().OrderBy(prop => prop.Name).ToArray();
            PropertyInfo[] destProps = destType.GetProperties().OrderBy(prop => prop.Name).ToArray();
            for (int i = 0, j = 0; i < srcProps.Length && j < destProps.Length; i++)
            {
                if (IsCompatible(srcProps[i], destProps[j]))
                {
                    PropertiesDictionary.Add(srcProps[i], destProps[j]);
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
            TColDest destCol;
            destCol = Activator.CreateInstance<TColDest>();
            foreach (var s in src)
            {
                destCol.Add(Map(s));
            }
            return destCol;
        }

        public TDest[] MapToArray(IEnumerable<TSrc> src)
        {
            
            TDest[] aux = new TDest[src.Count()];
            int i = 0;
            foreach (var s in src)
            {
                aux[i] = Map(s);
                i++;
            }
            return aux;

        }

        public IEnumerable<TDest> MapLazy(IEnumerable<TSrc> src)
        {
            foreach (var s in src)
            {
                yield return Map(s);
            }
        }

        private bool IsCompatible(PropertyInfo src, PropertyInfo dest)
        {
            foreach (string str in ToBeIgnoredList)
            {
                if (!str.Equals(src.Name))
                    return false;
            }
            return src.Name.Equals(dest.Name) && src.GetType() == dest.GetType();
        }
    }
}
