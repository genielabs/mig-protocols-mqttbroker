using System;

using MIG;
using MIG.Utility;
using MIG.Interfaces.HomeAutomation.Commons;

using uPLibrary.Networking.M2Mqtt;
using System.Collections.Generic;

namespace MIG.Interfaces.Protocols
{
    public class MqttBrokerService : MIGInterface
    {
        private MqttBroker mqttService;

        #region MIG Interface members

        public event Action<InterfaceModulesChangedAction> InterfaceModulesChangedAction;
        public event Action<InterfacePropertyChangedAction> InterfacePropertyChangedAction;

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

        public List<MIGServiceConfiguration.Interface.Option> Options { get; set; }

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

        public object InterfaceControl(MIGInterfaceCommand request)
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
            if (InterfaceModulesChangedAction != null) InterfaceModulesChangedAction(new InterfaceModulesChangedAction() { Domain = this.Domain });
            //
            return success;
        }

        public void Disconnect()
        {
            try
            {
                mqttService.Stop();
            } catch { }
            mqttService = null;
        }

        public bool IsDevicePresent()
        {
            return true;
        }

        #endregion

    }
}

