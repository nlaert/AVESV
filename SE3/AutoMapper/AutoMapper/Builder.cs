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
        
        public static AutoMapper<TSrc, TDest> Build<TSrc, TDest>()
        {
            
           
            return new AutoMapper<TSrc, TDest>();
        }
    }
}