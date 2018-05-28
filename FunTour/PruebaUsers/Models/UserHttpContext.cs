using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PruebaUsers.ActualModels;

public static class UserHttpContext
    {
            public static bool HasRole(this ControllerBase controller, string Role)
            {
                bool bFound = false;
                try
                {
                    //Verifica que un usuario tenga un rol dado
                    bFound = new ActualUser(controller.ControllerContext.HttpContext.User.Identity.Name).HasRole(Role);
                }
                catch { }
                return bFound;
            }

            public static bool HasRoles(this ControllerBase controller, string Roles)
            {
                bool bFound = false;
                try
                {
                // Verifica que el usuario tiene alguno de los Roles dados.
                //Los Roles deber estar separados por ; (ie "Sales Manager;Sales Operator"
                bFound = new ActualUser(controller.ControllerContext.HttpContext.User.Identity.Name).HasRoles(Roles);
                }
                catch { }
                return bFound;
            }

            public static bool HasPermission(this ControllerBase controller, string permission)
            {
                bool bFound = false;
                try
                {
                    //Check if the requesting user has the specified application permission...
                    bFound = new ActualUser(controller.ControllerContext.HttpContext.User.Identity.Name).HasPermission(permission);
                }
                catch { }
                return bFound;
            }

            public static bool IsSysAdmin(this ControllerBase controller)
            {
                bool bIsSysAdmin = false;
            
                try
                {
                    //Check if the requesting user has the System Administrator privilege...
                    bIsSysAdmin = new ActualUser(controller.ControllerContext.HttpContext.User.Identity.Name).IsSysAdmin;
                }
                catch { }
                return bIsSysAdmin;
            }
    }