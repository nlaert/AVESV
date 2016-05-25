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
        private Dictionary<PropertyInfo, PropertyInfo> PropertiesDictionary;
        private Dictionary<string, Func<TSrc, object>> ForMemberDictionary;
        private List<string> NamesToBeIgnored;
        private List<Type> AttributesToBeIgnored;
        public AutoMapper()
        {
            PropertiesDictionary = new Dictionary<PropertyInfo, PropertyInfo>();
            ForMemberDictionary = new Dictionary<string, Func<TSrc, object>>();
            NamesToBeIgnored = new List<string>();
            AttributesToBeIgnored = new List<Type>();
        }

        public AutoMapper<TSrc, TDest> IgnoreMember(string name)
        {
            NamesToBeIgnored.Add(name);
            return this;
        }

        public AutoMapper<TSrc, TDest> IgnoreMember<TAttribute>() where TAttribute : System.Attribute
        {
            Type t = typeof(TAttribute);
            AttributesToBeIgnored.Add(t);
            return this;
        }

        public AutoMapper<TSrc, TDest> ForMember(string name, Func<TSrc, object> func)
        {
            ForMemberDictionary.Add(name, func);
            return this;
        }
        
        public Mapper<TSrc, TDest> CreateMapper()
        {
            Type srcType = typeof(TSrc);
            Type destType = typeof(TDest);
            
            PropertyInfo[] destProps = destType.GetProperties().ToArray();
            for (int i = 0, j = 0; i < destProps.Length; i++)
            {
                PropertyInfo srcProp = srcType.GetProperty(destProps[i].Name);
                if (IsCompatible(srcProp, destProps[i], ref j))
                {
                    PropertiesDictionary.Add(destProps[i], srcProp);
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
                object aux = GetValue(p, src);
                p.SetValue(dest, aux);
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

        private bool IsCompatible(PropertyInfo src, PropertyInfo dest, ref int j)
        {
            if (src == null)
            {
                return ForMemberDictionary.ContainsKey(dest.Name);
            }
            if (!src.Name.Equals(dest.Name) || src.GetType() != dest.GetType())
            {
                return false;
            }
            foreach (string str in NamesToBeIgnored)
            {
                if (str.Equals(src.Name))
                {
                    j++;
                    return false;
                }
                    
            }
            foreach (Type t in AttributesToBeIgnored)
            {
                if (src.GetCustomAttribute(t) != null)
                {
                    j++;
                    return false;
                }
            }
            return true;
        }

        private object GetValue(PropertyInfo property, TSrc src)
        {
            string destName = property.Name;
            if (ForMemberDictionary.ContainsKey(destName))
            {
                Func<TSrc, object> func = ForMemberDictionary[destName];
                return func(src);
            }
            return PropertiesDictionary[property].GetValue(src);
        }
    }
}
