
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Net;
using System.Management;
using System.Configuration;


namespace WindowsService_teste
{
    class NovoProcesso
    {
        private static string log_nome = "Broner";
        private static string proc_nome = ConfigurationManager.AppSettings["mscc_path"].ToString() + ConfigurationManager.AppSettings["mscc_bin"].ToString();

        public void log(string mensagem)
        {
            string registroNome = log_nome;
            if (!EventLog.SourceExists(registroNome))
            {
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
            pararMscc.CreateNoWindow = true;
            pararMscc.UseShellExecute = true;
            pararMscc.WindowStyle = ProcessWindowStyle.Hidden;
            //pararMscc.FileName = proc_nome + " " + argumentosMscc;
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
            if (ConfigurationManager.AppSettings["mscc_srv"].ToString() == "false")
            {
                return true;
            }
            else
            {

                bool retorno = false;
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope("\\\\" + ConfigurationManager.AppSettings["mscc_srv_host"].ToString() + "\\root\\mscluster", options);
                scope.Connect();
                
                ObjectQuery query = new ObjectQuery("Select State from MSCluster_Resource where name = '" + recurso_cluster + "'");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    if (Convert.ToInt16(m["state"]) == 2)
                    {
                        retorno = true; //serviços OK
                    }
                    else
                    {
                        retorno = false;
                    }
                }
                return !retorno;
            }
        }
        public bool consultarProc(string procName)
        {
            
            bool retorno = false;
            ConnectionOptions options = new ConnectionOptions();
            ManagementScope scope = new ManagementScope("\\\\" + ConfigurationManager.AppSettings["mscc_proc_host"].ToString() + "\\root\\CIMV2", options);
            scope.Connect();
            ObjectQuery query = new ObjectQuery("SELECT ProcessId FROM Win32_Process where name = '" + procName + "'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection queryCollection = searcher.Get();
            foreach (ManagementObject m in queryCollection)
            {
                if (Convert.ToInt16(m["ProcessId"]) != null)
                {
                    retorno = true; //processo OK
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
