using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace service_protect
{
    class Monitor
    {
        List<string> service_list = new List<string>();

        public Monitor()
        {
            init_service_list();

        }

        public void StartMoitor()
        {
            Thread t = new Thread(new ThreadStart(check_service_list));
            t.Start();
        }

        private void check_service_list()
        {
            while (true)
            {
                foreach (string service in service_list)
                {
                    if (!IsServiceStart(service))
                    {
                        StartService(service);
                    }
                }
                Thread.Sleep(5000);
            }
            
        }

        private void init_service_list()
        {
            service_list.Clear();
            StreamReader sr = new StreamReader("service_list.txt", Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                service_list.Add(line);
            }
        }

        public static bool IsServiceStart(string serviceName)
        {
            ServiceController psc = new ServiceController(serviceName);
            bool bStartStatus = false;
            try
            {
                if (!psc.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    bStartStatus = true;
                }
                return bStartStatus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private bool StartService(string serviceName)
        {
            bool flag = true;
            if (true)
            {
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                if (service.Status != System.ServiceProcess.ServiceControllerStatus.Running && service.Status != System.ServiceProcess.ServiceControllerStatus.StartPending)
                {
                    service.Start();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        {
                            break;
                        }
                        if (i == 59)
                        {
                            flag = false;
                        }
                    }
                }
            }
            return flag;
        }
    }
}
