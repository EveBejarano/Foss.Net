using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FunTour.ActualModels;
using FunTourBusinessLayer.UnitOfWorks;

public static class UserHttpContext
    {
            public static bool HasRole(this ControllerBase controller, string Role, UnitOfWork unitOfWork)
            {
                bool bFound = false;
                try
                {
                    //Verifica que un usuario tenga un rol dado
                    bFound = new ActualUser(controller.ControllerContext.HttpContext.User.Identity.Name, unitOfWork).HasRole(Role);
                }
                catch { }
                return bFound;
            }

            public static bool HasRoles(this ControllerBase controller, string Roles, UnitOfWork unitOfWork)
            {
                bool bFound = false;
                try
                {
                // Verifica que el usuario tiene alguno de los Roles dados.
                //Los Roles deber estar separados por ; (ie "Sales Manager;Sales Operator"
                bFound = new ActualUser(controller.ControllerContext.HttpContext.User.Identity.Name, unitOfWork).HasRoles(Roles);
                }
                catch { }
                return bFound;
            }

            public static bool HasPermission(this ControllerBase controller, string permission, UnitOfWork unitOfWork)
            {
                bool bFound = false;
                try
                {
                    //Check if the requesting user has the specified application permission...
                    bFound = new ActualUser(controller.ControllerContext.HttpContext.User.Identity.Name, unitOfWork).HasPermission(permission);
                }
                catch { }
                return bFound;
            }

            public static bool IsSysAdmin(this ControllerBase controller, UnitOfWork unitOfWork)
            {
                bool bIsSysAdmin = false;
            
                try
                {
                    //Check if the requesting user has the System Administrator privilege...
                    bIsSysAdmin = new ActualUser(controller.ControllerContext.HttpContext.User.Identity.Name, unitOfWork).IsSysAdmin;
                }
                catch { }
                return bIsSysAdmin;
            }
    }