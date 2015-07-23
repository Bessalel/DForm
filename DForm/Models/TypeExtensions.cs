using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Collections;


namespace DForm
{
    public static class TypeExtensions
    {
        public static T CloneSerializableObject<T>( T o, IFormatter formatter = null )
        {
            using( var s = new MemoryStream() )
            {
                if( formatter == null ) formatter = new BinaryFormatter();
                formatter.Serialize( s, o );
                s.Position = 0;
                return (T)formatter.Deserialize( s );
            }
        }
    }
}
