using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
namespace AutoMapperPrj
{
    public class Builder
    {
        public Dictionary<PropertyInfo, PropertyInfo> PropertiesDictionary { get; set; }
        public Type Src;
        public Type Dest;
        
        public Builder Build<TSrc, TDest>(this Builder builder)
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
            //return new Builder<TSrc,TDest>();
        }

        public Mapper<TSrc, TDest> CreateMapper(this Builder builder)
        {
            AutoMapper<TSrc, TDest> m = new AutoMapper<TSrc, TDest>(builder);
            return m;

        }
    }
}