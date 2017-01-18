using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
//using System.Xml;

namespace WindowsService_teste
{
    public partial class Service1 : ServiceBase
    {
        private bool isStopped = false;
        NovoProcesso processoLog = new NovoProcesso();
        private Int16 tempo = 5000;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.isStopped = false;
            Thread t = new Thread(new ThreadStart(this.Start));
            t.Start();
        }

        protected override void OnStop()
        {
            this.isStopped = true;
        }

        public void Start()
        {
            int loopDaMorte = 5;

            Wmicon processos = new Wmicon();
            while (!this.isStopped)
            {
                System.Threading.Thread.Sleep(tempo);
            }
            if (true)
            {
                while (loopDaMorte > 0)
                {
                    
                    if (processos.consultarCluster("MSCC ServerControl") && processos.consultarCluster("mosOlcBridge Application") && processos.consultarProc("mosOlcBridge.exe") & processos.consultarProc("mscc_serv.exe") || loopDaMorte == 1)
                    {
                        processoLog.iniciaProcesso("-serverKey MSCC_MOS_SERVER -cmd stop");
                        processoLog.iniciaProcesso("-serverKey MSCC_L2_SERVER -cmd stop");
                        processoLog.iniciaProcesso("-serverKey MSCC_ADM_SERVER -cmd stop");
                        //loopDaMorte = 0;
                        Environment.Exit(0);
                        
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                        loopDaMorte--;
                    }
                }
            } 
        }
    }
}
