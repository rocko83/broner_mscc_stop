using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService_teste;

namespace ConsoleApplication1_argumentos
{
    class Program
    {
        static void Main(string[] args)
        {
            Wmicon processos = new Wmicon();
            System.Console.WriteLine("args=" + processos.consultarProc("args.exe"));
            if (args.Length != 0)
            {
                //System.Console.WriteLine("Entrada={0}" , args[0]);
                System.IO.StreamWriter file = new System.IO.StreamWriter(@"c:\Program Files (x86)\Broner Cluster\logs_arg.txt", true);
                //System.IO.File.WriteAllLines(@"c:\tmp\broner\log_arg.txt",args[0].ToString);
                //file.WriteLine(args[0].ToString());
                //file.Write(args[0].ToString());
                System.DateTime hora_now = System.DateTime.Now;
                string hora_formato_now = "MMM ddd d HH:mm yyyy";
                
            
                foreach (string argumentos in args)
                {
                    file.WriteLine(argumentos + ", " + hora_now.ToString(hora_formato_now) );
                    System.Console.WriteLine("Entrada={0}", argumentos);
                    //file.WriteLine("Entrada=" + argumentos);
                    
                    //System.IO.StreamWriter concatenaLinhas = new System.IO.StreamWriter("c:\tmp\broner\args_log.txt", true);
                    //concatenaLinhas.WriteLine(argumentos + ", " + hora_now.ToString(hora_formato_now)); 
                    //concatenaLinhas.WriteLine("Entrada={0}" + argumentos);

                }
                file.Close();
                if (args.Length == 2)
                {
                    if (args[0] == "tempo")
                    {
                        System.Threading.Thread.Sleep(Int32.Parse(args[1]));
                         
                    }

                }
            }

            //return true;
        }
    }
}
