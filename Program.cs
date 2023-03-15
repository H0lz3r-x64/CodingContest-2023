using System;
using System.Collections.Generic;
using System.IO;
using com.knapp.CodingContest.core;
using com.knapp.CodingContest.solution;
using System.Diagnostics;
using com.knapp.CodingContest.operations;

namespace com.knapp.CodingContest
{
    static class Program
    {

        /// <summary>
        /// you may change any code you like
        ///      => but changing the output may lead to invalid results!
        /// </summary>
        static void Main( )
        {
            Console.Out.WriteLine("KNAPP Coding Contest #11: Starting...");
            MyInputData iinput;
            InputStat istat;

            try
            {
                Console.Out.WriteLine("#... LOADING Input ...");
                iinput = new MyInputData( new CostFactors() );
                iinput.Load( );

                istat = new InputStat(iinput);

                Console.Out.WriteLine();

                Console.Out.WriteLine( "#... DATA LOADED" );
            }
            catch ( Exception e )
            {
                ShowException( e, "Exception in startup code" );
                Console.Out.WriteLine( "Press <enter>" );
                Console.In.ReadLine( );
                throw;
            }

            Console.Out.WriteLine( "### Your output starts here" );

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Solution solution = new Solution(iinput, iinput.GetOperations());
            try
            {

                solution.Run();
                stopwatch.Stop();
            }
            catch (Exception e)
            {
                ShowException(e, "Exception in your code");
                Console.Out.WriteLine("Press <enter>");
                Console.In.ReadLine();
                throw;
            }

            WriteProperties(solution, Settings.outPropertyFilename);

            Console.Out.WriteLine($"### DONE in {stopwatch.ElapsedMilliseconds}ms");
            Console.Out.WriteLine($"------------------------------------------------");
            Console.Out.WriteLine($"   Results/ Cost for your solution");
            Console.Out.WriteLine($"   {solution.ParticipantName} / {solution.Institute}");

            WriteResults( solution, istat, iinput.GetOperations() );

            try
            {
                PrepareUpload.WriteResult(solution, iinput.GetOperations());
                PrepareUpload.CreateZipFile();
                Console.Out.WriteLine( ">>> Created " + Settings.outZipFilename );
            }
            catch ( Exception e )
            {
                ShowException( e, "Exception in shutdown code" );
                throw;
            }

            Console.Out.WriteLine( "Press <enter>" );
            Console.In.ReadLine( );
        }


        private static void WriteResults( Solution solution, InputStat istat, MyOperations operations )
        {
            InfoSnapshotInternal info = (InfoSnapshotInternal)operations.GetInfoSnapshot();
            CostFactors c = operations.Costs;


            long t_sh = info.TotalShipmentCount;
            double t_szxd = info.TotalSizeByDistance;
            double t_d = info.TotalDistance;


            int uo = info.UnfinishedOrderLineCount;
            double c_uo_p = uo > 0 ? c.UnfinishedOrderLinesPenalty : 0;
            double c_uo_ = info.UnfinishedOrderLinesCost - c_uo_p;
            double c_s = info.ShipmentsCosts;
            double c_t = info.TotalCost;

            double c_sh = info.TotalShipmentCount * c.ShipmentBaseCosts;
            double c_szxd = c_s - c_sh;

            Console.Out.WriteLine("");
            Console.Out.WriteLine("  --------------------------------------------------------------");
            Console.Out.WriteLine("    INPUT STATISTICS                                            ");
            Console.Out.WriteLine("  ------------------------------------- : ----------------------");
            Console.Out.WriteLine("      #warehouses                       :  {1,8}", "", istat.countWarehouses );
            Console.Out.WriteLine("      #customers                        :  {1,8}", "", istat.countCustomers );
            Console.Out.WriteLine("      #order-lines                      :  {1,8}", "", istat.countOrderLines);
            Console.Out.WriteLine("      #products                         :  {1,8}", "", istat.countUniqueProducts);
            Console.Out.WriteLine("      #products / order                 :  {1,10:0.0}", "", istat.avgOrderLinesPerCustomer);
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  --------------------------------------------------------------");
            Console.Out.WriteLine("    RESULT STATISTICS                                           ");
            Console.Out.WriteLine("  ------------------------------------- : ----------------------");
            Console.Out.WriteLine("      total shipment count              :  {0,8}", t_sh );
            Console.Out.WriteLine("      sum( size*distance )              :  {0,8}", t_szxd);
            Console.Out.WriteLine("      total distance                    :  {0,8}", t_d);
            Console.Out.WriteLine("");
            Console.Out.WriteLine("      Calc for penalty costs            :  " + MyOperations.COST_EVAL_OPEN_LINES[uo == 0 ? 0 : 1]);
            Console.Out.WriteLine("      Calc for per shipment costs       :  " + MyOperations.COST_EVAL_SHIPMENT);
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  =============================================================================");
            Console.Out.WriteLine("    RESULTS                                                                    ");
            Console.Out.WriteLine("  ===================================== : ============ | ======================");
            Console.Out.WriteLine("      what                              :       costs  |  (details: count,...)");
            Console.Out.WriteLine("  ------------------------------------- : ------------ | ----------------------");
            Console.Out.WriteLine("   -> penalty unfinished orders         :  {0,10:0.0}  |   {1,6}", c_uo_p, uo);
            Console.Out.WriteLine("   -> costs/unfinished order-lines      :  {0,10:0.0}  |   {1,6}", c_uo_, uo);
            Console.Out.WriteLine("   -> #shipments                        :  {0,10:0.0}  |   {1,6}", c_sh, t_sh);
            Console.Out.WriteLine("   -> #sum(dist*size)                   :  {0,10:0.0}  |   {1,6}", c_szxd, t_szxd);
            Console.Out.WriteLine("  ------------------------------------- : ------------ | ----------------------");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("   => TOTAL COST                           {0,10:0.0}", c_t);
            Console.Out.WriteLine("                                          ============");

        }

        /// <summary>
        /// Helper function to write the properties to the file
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="outFilename"></param>
        /// <exception cref="ArgumentException">when either solution.InstituteId or solution.ParticipantName is not valid</exception>
        private static void WriteProperties(Solution solution, string outFilename)
        {
            if (File.Exists(outFilename))
            {
                File.Delete( outFilename);
            }

            using ( StreamWriter stream = new StreamWriter( outFilename, false, System.Text.Encoding.GetEncoding( "ISO-8859-1" ) ) )
            {
                stream.WriteLine( "# -*- conf-javaprop -*-" );
                stream.WriteLine( $"participant = {solution.ParticipantName}" );
                stream.WriteLine( $"institution = {solution.Institute}" );
                stream.WriteLine( "technology = c#" );
            }

        }

        /// <summary>
        /// write exception to console.error
        /// includes inner exception and data
        /// </summary>
        /// <param name="e">exception that should be shown</param>
        /// <param name="codeSegment">segment where the exception was caught</param>
        public static void ShowException( Exception e, string codeSegment )
        {
            Console.Out.WriteLine( "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" );
            Console.Out.WriteLine(  codeSegment );
            Console.Out.WriteLine( "[{0}]: {1}", e.GetType( ).Name, e.Message );

            for ( Exception inner = e.InnerException
                ; inner != null
                ; inner = inner.InnerException )
            {
                System.Console.WriteLine( ">>[{0}] {1}"
                                                , inner.GetType( ).Name
                                                , inner.Message
                                            );
            }


            if ( e.Data != null && e.Data.Count > 0 )
            {
                Console.Error.WriteLine( "------------------------------------------------" );
                Console.Error.WriteLine( "Data in exception:" );
                foreach( KeyValuePair<string, string> elem in e.Data )
                {
                    Console.Error.WriteLine( "[{0}] : '{1}'", elem.Key, elem.Value );
                }
            }
            Console.Out.WriteLine( "------------------------------------------------" );
            Console.Out.WriteLine( e.StackTrace );
            Console.Out.WriteLine( "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" );
        }
    }
}
