using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text;

namespace Projects.Application.Validations
{
    public class NamespaceRoutingConvention : Attribute, IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var hasRouteAttributes = controller.Selectors.Any(selector =>
                                                    selector.AttributeRouteModel != null);
            var namespc = controller.ControllerType.Namespace;

            if (hasRouteAttributes || namespc == null) return;

            var template = new StringBuilder("/Api");
            template.Replace('.', '/');
            template.Append("/" + controller.ControllerName);

            foreach (var selector in controller.Selectors)
            {
                selector.AttributeRouteModel = new AttributeRouteModel()
                {
                    Template = template.ToString()
                };
            }
        }
    }
}
