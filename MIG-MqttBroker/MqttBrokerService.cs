using System;

using MIG;
using MIG.Utility;

using uPLibrary.Networking.M2Mqtt;
using System.Collections.Generic;
using MIG.Config;

namespace MIG.Interfaces.Protocols
{
    public class MqttBrokerService : MigInterface
    {
        private MqttBroker mqttService;

        #region MIG Interface members

        public event InterfaceModulesChangedEventHandler InterfaceModulesChanged;
        public event InterfacePropertyChangedEventHandler InterfacePropertyChanged;

        public string Domain
        {
            get
            {
                string domain = this.GetType().Namespace.ToString();
                domain = domain.Substring(domain.LastIndexOf(".") + 1) + "." + this.GetType().Name.ToString();
                return domain;
            }
        }

        public bool IsEnabled { get; set; }

        public List<Option> Options { get; set; }

        public void OnSetOption(Option option)
        {
            if (IsEnabled)
                Connect();
        }

        public List<InterfaceModule> GetModules()
        {
            List<InterfaceModule> modules = new List<InterfaceModule>();
            return modules;
        }

        public bool IsConnected
        {
            get
            {
                return mqttService != null;
            }
        }

        public object InterfaceControl(MigInterfaceCommand request)
        {
            return "";
        }

        public bool Connect()
        {
            bool success = false;
            //
            try
            {
                mqttService = new MqttBroker();
                mqttService.Start();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("MQTT Broker could not be started: " + e.Message);
                Disconnect();
            }
            OnInterfaceModulesChanged(this.GetDomain());
            //
            return success;
        }

        public void Disconnect()
        {
            try
            {
                mqttService.Stop();
            }
            catch
            {
            }
            mqttService = null;
        }

        public bool IsDevicePresent()
        {
            return true;
        }

        #endregion

        #region Events

        protected virtual void OnInterfaceModulesChanged(string domain)
        {
            if (InterfaceModulesChanged != null)
            {
                var args = new InterfaceModulesChangedEventArgs(domain);
                InterfaceModulesChanged(this, args);
            }
        }

        #endregion

    }
}

