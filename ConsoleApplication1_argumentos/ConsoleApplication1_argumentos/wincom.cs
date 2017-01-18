
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Net;
using System.Management;


namespace WindowsService_teste
{
    class NovoProcesso
    {
        private static string log_nome = "Broner";
        private static string proc_nome = @"C:\tmp\broner\args.exe";
        public void log(string mensagem)
        {
            string registroNome = log_nome;
            if (!EventLog.SourceExists(registroNome))
            {
                //EventSourceCreationData registro = new EventSourceCreationData(registroNome, registroNome);
                EventLog.CreateEventSource(registroNome, registroNome);
            }
            EventLog registroLog = new EventLog();
            registroLog.Source = registroNome;
            registroLog.WriteEntry(mensagem, EventLogEntryType.Information);

        }
        public void log_erro(string mensagem)
        {
            string registroNome = log_nome;
            if (!EventLog.SourceExists(registroNome))
            {
                //EventSourceCreationData registro = new EventSourceCreationData(registroNome, registroNome);
                EventLog.CreateEventSource(registroNome, registroNome);
            }
            EventLog registroLog = new EventLog();
            registroLog.Source = registroNome;
            registroLog.WriteEntry(mensagem, EventLogEntryType.Error);

        }
        public void iniciaProcesso(string argumentosMscc)
        {

            System.DateTime hora_now = System.DateTime.Now;
            //string hora_formato_now = "MMM ddd d HH:mm yyyy";
            //string hora_final = @"/c echo stop " + hora_now.ToString(hora_formato_now) + " Stop >> C:\\tmp\\broner\\aaa.txt\n";
            ProcessStartInfo pararMscc = new ProcessStartInfo();
            pararMscc.CreateNoWindow = false;
            pararMscc.UseShellExecute = false;
            pararMscc.WindowStyle = ProcessWindowStyle.Hidden;
            pararMscc.FileName = proc_nome;
            pararMscc.Arguments = argumentosMscc;
            using (Process p = Process.Start(pararMscc))
            {
                p.WaitForExit();
                if (p.HasExited && p.ExitCode == 0)
                {
                    this.log(proc_nome + " " + argumentosMscc + " Foi executado com sucesso.");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                else if (p.ExitCode != 0)
                {
                    this.log_erro(proc_nome + " " + argumentosMscc + " Foi executado com falha.");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }



    }

    class Wmicon
    {
        public bool consultarCluster(string recurso_cluster)
        {
            bool retorno = false;
            ConnectionOptions options = new ConnectionOptions();
            ManagementScope scope = new ManagementScope("\\\\localhost\\root\\mscluster", options);
            scope.Connect();
            ObjectQuery query = new ObjectQuery("Select State from MSCluster_Resource where name = '" + recurso_cluster + "'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection queryCollection = searcher.Get();
            foreach (ManagementObject m in queryCollection)
            {
                //Console.WriteLine(Convert.ToInt16(m["state"]));
                if (Convert.ToInt16(m["state"]) == 2)
                {
                    retorno = true;
                }
                else
                {
                    retorno = false;
                }
            }
            return !retorno;
        }
        public bool consultarProc(string procName)
        {
            bool retorno = false;
            ConnectionOptions options = new ConnectionOptions();
            ManagementScope scope = new ManagementScope("\\\\localhost\\root\\CIMV2", options);
            scope.Connect();
            ObjectQuery query = new ObjectQuery("SELECT ProcessId FROM Win32_Process where name = '" + procName + "'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection queryCollection = searcher.Get();
            foreach (ManagementObject m in queryCollection)
            {
                Console.WriteLine((m["processid"]));
                if (Convert.ToInt16(m["ProcessId"]) != null)
                {
                    retorno =true;
                }
                else
                {
                    retorno = false;
                }
            }
            return !retorno;
        }

    }
}
