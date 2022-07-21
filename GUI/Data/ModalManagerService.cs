using Blazored.Modal;
using Blazored.Modal.Services;
using GUI.Components.Modal;
using Microsoft.AspNetCore.Components;

namespace GUI.Data
{
    public class ModalManagerService
    {
        [Inject]
        private IModalService _Modal { get; set; }

        // Constructor

        public ModalManagerService(IModalService modal)
        {
            _Modal = modal;
        }

        // Methods

        private Dictionary<string, object> ConvertModalParametersToDictionary<Component>(ModalParameters parameters)
        {
            var output = new Dictionary<string, object>();

            foreach (var propertyInfo in typeof(Component).GetProperties())
            {
                string name = propertyInfo.Name;
                var value = typeof(ModalParameters).GetMethod("Get").MakeGenericMethod(propertyInfo.PropertyType).Invoke(parameters, new object[] { name });

                Console.Write($"{name}, {value}");

                output.Add(name, value);
            }

            return output;
        }

        public IModalReference ShowModal<Component>()
        {
            return ShowModal<Component>(new ModalParameters());
        }

        public IModalReference ShowModal<Component>(ModalParameters parameters)
        {
            var modalParameters = new ModalParameters();
            modalParameters.Add("ModalParameters", ConvertModalParametersToDictionary<Component>(parameters));
            modalParameters.Add("ModalComponent", typeof(Component));

            var modalOptions = new ModalOptions()
            {
                HideHeader = true,
                Class = "modal"
            };

            return _Modal.Show<Modal>("Modal", modalParameters, modalOptions);
        }
    }
}
