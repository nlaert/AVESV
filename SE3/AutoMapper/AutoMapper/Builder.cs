namespace AutoMapperPrj
{
    public class Builder
    {  
        public static AutoMapper<TSrc, TDest> Build<TSrc, TDest>()
        {
            return new AutoMapper<TSrc, TDest>();
        }
    }
}