using com.knapp.CodingContest.data;
using System.Text;

namespace com.knapp.CodingContest.core
{
    public abstract class WarehouseOperation
    {
        private readonly string resultString;

        protected WarehouseOperation( params object[] args )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( GetType().Name ).Append( ";" );

            foreach ( var arg in args )
            {
                sb.Append( arg.ToString() ).Append( ";" );
            }

            resultString = sb.ToString();
        }

        public override string ToString()
        {
            return resultString;
        }

        public string ToResultString()
        {
            return resultString;
        }
    }
}
