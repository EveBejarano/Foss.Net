using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FunTourBusinessLayer.UnitOfWorks;

namespace FunTour.ActualModels
{
    public class UserAuthorizationAttribute: AuthorizeAttribute
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        ////
        //// Resumen:
        ////     Especifica que el acceso a un método de acción o controlador está restringido
        ////     a usuarios que cumplieron el requisito de autorización.
        //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
        //public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
        //{
        //    //
        //    // Resumen:
        //    //     Inicializa una nueva instancia de la clase System.Web.Mvc.AuthorizeAttribute.
        //    public AuthorizeAttribute();

        //    //
        //    // Resumen:
        //    //     Obtiene o establece los roles de usuario autorizados a obtener acceso al método
        //    //     de acción o controlador.
        //    //
        //    // Devuelve:
        //    //     Los roles de usuario autorizados para obtener acceso al método de acción o controlador.
        //    public string Roles { get; set; }
        //    //
        //    // Resumen:
        //    //     Obtiene un identificador único para este atributo.
        //    //
        //    // Devuelve:
        //    //     Identificador único para este atributo.
        //    public override object TypeId { get; }
        //    //
        //    // Resumen:
        //    //     Obtiene o establece los usuarios autorizados a obtener acceso al método de acción
        //    //     o controlador.
        //    //
        //    // Devuelve:
        //    //     Los usuarios autorizados para obtener acceso al método de acción o controlador.
        //    public string FunTour { get; set; }

        //    //
        //    // Resumen:
        //    //     Se llama cuando un proceso solicita autorización.
        //    //
        //    // Parámetros:
        //    //   filterContext:
        //    //     Contexto del filtro, que encapsula la información para usar System.Web.Mvc.AuthorizeAttribute.
        //    //
        //    // Excepciones:
        //    //   T:System.ArgumentNullException:
        //    //     El parámetro filterContext es null.
        //    public virtual void OnAuthorization(AuthorizationContext filterContext);
        //    //
        //    // Resumen:
        //    //     Cuando se reemplaza, proporciona un punto de entrada para las comprobaciones
        //    //     de autorización personalizada.
        //    //
        //    // Parámetros:
        //    //   httpContext:
        //    //     Contexto HTTP, que encapsula toda la información específica de HTTP acerca de
        //    //     una solicitud HTTP individual.
        //    //
        //    // Devuelve:
        //    //     true si el usuario está autorizado; de lo contrario, false.
        //    //
        //    // Excepciones:
        //    //   T:System.ArgumentNullException:
        //    //     El parámetro httpContext es null.
        //    protected virtual bool AuthorizeCore(HttpContextBase httpContext);
        //    //
        //    // Resumen:
        //    //     Procesa las solicitudes HTTP que producen un error en la autorización.
        //    //
        //    // Parámetros:
        //    //   filterContext:
        //    //     Encapsula la información para usar System.Web.Mvc.AuthorizeAttribute.El objeto
        //    //     filterContext contiene el controlador, el contexto HTTP, el contexto de la solicitud,
        //    //     el resultado de la acción y los datos de ruta.
        //    protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext);
        //    //
        //    // Resumen:
        //    //     Se llama cuando el módulo de almacenamiento en caché solicita autorización.
        //    //
        //    // Parámetros:
        //    //   httpContext:
        //    //     Contexto HTTP, que encapsula toda la información específica de HTTP acerca de
        //    //     una solicitud HTTP individual.
        //    //
        //    // Devuelve:
        //    //     Referencia al estado de validación.
        //    //
        //    // Excepciones:
        //    //   T:System.ArgumentNullException:
        //    //     El parámetro httpContext es null.
        //    protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext);
        //}


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // Crea un permiso en formato String con nombre del controlador y el nombre de la accion "controlador-Accion"
            String RequiredPermission = String.Format("{0}-{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);

            //Crea una instancia del usuario que necesita autorizacion requiriendo el nombre de usuario para pasarlo al constructor de ActualUser(string UserName)
            ActualUser RequestedUser = new ActualUser(filterContext.RequestContext.HttpContext.User.Identity.Name, UnitOfWork);

            if (!RequestedUser.HasPermission(RequiredPermission) & !RequestedUser.IsSysAdmin)
            {
                // si el usuario no posee el permiso necesaro y no es Sysadmin, se lo redirige 
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Index" }, { "controller", "NotAuthorizedUser" } });
            }
        }
    }
}